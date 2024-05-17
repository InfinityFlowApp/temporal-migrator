// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Abstractions;
using Microsoft.Extensions.Logging;
using Temporalio.Workflows;

/// <summary>
/// Migration Workflow.
/// </summary>
[Workflow]
public class MigrationWorkflow
{
    /// <summary>
    /// Default Task Queue Name.
    /// </summary>
    internal const string DefaultTaskQueueName = "migration";

    private readonly Type _migrationType = typeof(IMigration);
    private readonly List<Type> _loadedTypes = [];

    /// <summary>
    /// Run Migration.
    /// </summary>
    /// <para>
    /// For <see cref="RunType.Bootstrap"/> objects expects assembly list. If objects is null it will scan all of
    /// the assemblies available to it.
    /// </para>
    /// <para>
    /// For <see cref="RunType.Migration"/> objects expects a type list. Only one is allowed, and should not be
    /// used outside of internal calls.
    /// </para>
    /// <param name="runType">The run type.</param>
    /// <param name="objects">The objects.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    [WorkflowRun]
    public async Task RunAsync(RunType runType = RunType.Bootstrap, string[]? objects = null)
    {
        switch (runType)
        {
            case RunType.Bootstrap:
                await RunEntryAsync(objects ?? [], Workflow.CancellationToken);
                break;
            case RunType.Migration:
                ArgumentNullException.ThrowIfNull(objects);
                await RunMigrationAsync(objects[0], Workflow.CancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(runType), runType, "Invalid MigrationRunner option");
        }
    }

    private async Task RunEntryAsync(string[] assemblies, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var workflowId = Workflow.Info.WorkflowId;
        var logger = Workflow.Logger;

        if (assemblies.Length == 0)
        {
            // scan all domain
            _loadedTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()));
        }
        else
        {
            // only load what is needed by configuration.
            var loadedAssemblies = new List<Assembly>();
            foreach (var assembly in assemblies)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var loadedAssembly = Assembly.Load(assembly);
                    loadedAssemblies.Add(loadedAssembly);
                }
                catch (ArgumentNullException)
                {
                    logger.LogDebug("{Assembly} string is null", assembly);
                }
                catch (ArgumentException)
                {
                    logger.LogDebug("{Assembly} string is empty", assembly);
                }
                catch (FileNotFoundException)
                {
                    logger.LogDebug("{Assembly} could not be found", assembly);
                }
                catch (FileLoadException)
                {
                    logger.LogDebug("{Assembly} could not be loaded", assembly);
                }
                catch (BadImageFormatException)
                {
                    logger.LogDebug("{Assembly} corrupted", assembly);
                }
            }

            _loadedTypes.AddRange(loadedAssemblies.SelectMany(assembly => assembly.GetTypes()));
        }

        var migrationTypes = _loadedTypes.Where(t => _migrationType.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });
        var richTypes = migrationTypes.Select(t => new { Type = t, Version = t.GetCustomAttribute<MigrationAttribute>()!.Version, });
        var sortedTypes = richTypes.OrderBy(t => t.Version);

        foreach (var migrationType in sortedTypes.Select(s => s.Type))
        {
            var nextType = migrationType.ToString();

            await Workflow.ExecuteChildWorkflowAsync<MigrationWorkflow>(
                s => s.RunAsync(
                    RunType.Migration,
                    new[] { nextType }),
                new ChildWorkflowOptions
                {
                    Id = $"{workflowId}_{migrationType.Name}",
                    CancellationToken = cancellationToken,
                    Memo = new Dictionary<string, object>
                    {
                        { "Parent", workflowId },
                    },
                });
        }
    }

    /// <summary>
    /// Run Migration.
    /// </summary>
    /// <param name="type">The type to run.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    private async Task RunMigrationAsync(string type, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var potentialMatches = _loadedTypes.ToList();

        if (potentialMatches.Count != 0)
        {
            var first = potentialMatches[0];

            if (Activator.CreateInstance(first) is not IMigration migration)
            {
                throw new InvalidOperationException("Type is not a migration object");
            }

            await migration.ExecuteAsync(cancellationToken);
        }
        else
        {
            Workflow.Logger.LogInformation("No type was found for: {Type}", type);
        }
    }
}

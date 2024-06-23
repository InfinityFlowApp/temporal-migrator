// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

#pragma warning disable CA1812

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Abstractions;
using Microsoft.Extensions.Logging;
using Temporalio.Workflows;

/// <summary>
/// Migration Workflow.
/// </summary>
[Workflow]
internal class MigrationWorkflow
{
    /// <summary>
    /// Default Task Queue Name.
    /// </summary>
    internal const string DefaultTaskQueueName = "migration";

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

        var types = await Workflow.ExecuteLocalActivityAsync<MigrationActivities, IEnumerable<Type>>(
            act => act.GetTypesAsync(),
            new LocalActivityOptions
            {
                ActivityId = $"{workflowId}_{nameof(MigrationActivities)}",
                ScheduleToCloseTimeout = TimeSpan.FromMinutes(5),
                CancellationToken = cancellationToken,
            });

        foreach (var migrationType in types)
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

        var potentialMatches = GlobalReflector.GetMigrations(Array.Empty<Assembly>());

        if (potentialMatches.Count() != 0)
        {
            var first = potentialMatches.First(w => w.ToString() == type);

            if (Activator.CreateInstance(first) is not IMigration migration)
            {
                throw new InvalidOperationException("Type is not a migration object");
            }

            await migration.ExecuteAsync(cancellationToken);
        }
        else
        {
            Workflow.Logger.LogWarning("No type was found for: {Type}", type);
        }
    }
}

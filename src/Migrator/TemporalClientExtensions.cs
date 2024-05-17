// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace Temporalio.Client;

using System.Reflection;
using InfinityFlow.Temporal.Migrator;

/// <summary>
/// Temporal Client Extensions.
/// </summary>
public static class TemporalClientExtensions
{
    /// <summary>
    /// Bootstrap Migrations.
    /// </summary>
    /// <param name="temporalClient">The temporal client.</param>
    /// <param name="workflowOptions">The workflow options.</param>
    /// <param name="assemblies">The assemblies.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task RunMigrator(this ITemporalClient temporalClient, WorkflowOptions workflowOptions, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(temporalClient);
        var assemblyParams = assemblies.Select(s => s.ToString());
        return temporalClient
            .ExecuteWorkflowAsync<MigrationWorkflow>(
                wf => wf.RunAsync(RunType.Bootstrap, assemblyParams.ToArray()),
                workflowOptions);
    }

    /// <summary>
    /// Bootstrap Migrations.
    /// </summary>
    /// <param name="temporalClient">The temporal client.</param>
    /// <param name="assemblies">The assemblies.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task RunMigrator(this ITemporalClient temporalClient, params Assembly[] assemblies)
    {
        return RunMigrator(
            temporalClient,
            new WorkflowOptions
            {
                Id = Guid.NewGuid().ToString(),
                TaskQueue = MigrationWorkflow.DefaultTaskQueueName,
            },
            assemblies.ToArray());
    }

    /// <summary>
    /// Bootstrap Migrations.
    /// </summary>
    /// <param name="temporalClient">The temporal client.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task RunMigrator(this ITemporalClient temporalClient)
    {
        return RunMigrator(temporalClient, []);
    }
}

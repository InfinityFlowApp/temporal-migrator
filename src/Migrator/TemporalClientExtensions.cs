// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace Temporalio.Client;

using InfinityFlow.Temporal.Migrator;

/// <summary>
/// Temporal Client Extensions.
/// </summary>
public static class TemporalClientExtensions
{
    /// <summary>
    /// Bootstrap Blocking Migrations.
    /// </summary>
    /// <param name="temporalClient">The temporal client.</param>
    /// <param name="workflowOptions">The workflow options.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task ExecuteMigrationAsync(this ITemporalClient temporalClient, WorkflowOptions workflowOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalClient);
        ArgumentNullException.ThrowIfNull(workflowOptions);
        return temporalClient.ExecuteWorkflowAsync<MigrationWorkflow>(
            wf => wf.RunAsync(RunType.Bootstrap, null),
            workflowOptions);
    }

    /// <summary>
    /// Bootstrap Non-Blocking Migrations.
    /// </summary>
    /// <param name="temporalClient">The temporal client.</param>
    /// <param name="workflowOptions">The workflow options.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task<WorkflowHandle> StartMigrationAsync(this ITemporalClient temporalClient, WorkflowOptions workflowOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalClient);
        ArgumentNullException.ThrowIfNull(workflowOptions);
        return await temporalClient.StartWorkflowAsync<MigrationWorkflow>(
            wf => wf.RunAsync(RunType.Bootstrap, null),
            workflowOptions);
    }
}

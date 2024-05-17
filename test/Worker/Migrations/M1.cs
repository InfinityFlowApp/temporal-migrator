// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Worker.Migrations;

using InfinityFlow.Temporal.Migrator.Abstractions;
using Microsoft.Extensions.Logging;
using Temporalio.Workflows;

/// <summary>
/// M1.
/// </summary>
[Migration(1)]
public class M1 : IMigration
{
    /// <inheritdoc />
    public ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Workflow.Logger.LogInformation("In M1 - {InWorkflow}", Workflow.InWorkflow);
        if (!Workflow.InWorkflow)
        {
            throw new InvalidOperationException("Not in workflow");
        }

        return ValueTask.CompletedTask;
    }
}

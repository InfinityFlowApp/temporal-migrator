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
public partial class M1 : IMigration
{
    private readonly ILogger _logger = Workflow.Logger;

    /// <inheritdoc />
    public ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        LogM1(Workflow.InWorkflow);
        if (!Workflow.InWorkflow)
        {
            throw new InvalidOperationException("Not in workflow");
        }

        return ValueTask.CompletedTask;
    }

    [LoggerMessage(
        EventId = 0,
        EventName = "M1",
        Level = LogLevel.Information,
        Message = "In M1 - {inWorkflow}")]
    private partial void LogM1(bool inWorkflow);
}

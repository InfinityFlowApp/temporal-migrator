// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Worker.Migrations;

using Abstractions;
using Microsoft.Extensions.Logging;
using Temporalio.Workflows;

/// <summary>
/// M2.
/// </summary>
[Migration(2)]
public partial class M2 : IMigration
{
    private readonly ILogger _logger = Workflow.Logger;

    /// <inheritdoc />
    public ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        LogM2();
        return ValueTask.CompletedTask;
    }

    [LoggerMessage(
        EventId = 0,
        EventName = "M2",
        Level = LogLevel.Information,
        Message = "In M2")]
    private partial void LogM2();
}

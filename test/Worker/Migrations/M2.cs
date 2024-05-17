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
public class M2 : IMigration
{
    /// <inheritdoc />
    public ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        Workflow.Logger.LogInformation("In M2");
        return ValueTask.CompletedTask;
    }
}

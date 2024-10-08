﻿// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests;

using Collections;
using Fixtures;
using Microsoft.Extensions.Logging;
using Temporalio.Client;
using Temporalio.Worker;

/// <summary>
/// Temporal Tests.
/// </summary>
[Collection(TemporalCollectionFixture.Name)]
public partial class TemporalTests
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<TemporalTests>();
    private readonly ITemporalClient _temporalClient;
    private readonly TemporalWorker _temporalWorker;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalTests"/> class.
    /// </summary>
    /// <param name="fixture">The temporal fixture.</param>
    public TemporalTests(TemporalFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _temporalClient = fixture.TemporalClient!;
        _temporalWorker = fixture.TemporalWorker!;
    }

    /// <summary>
    /// Test 1.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TemporalWorkflowFromStartToFinish()
    {
        LogTestStarted();

        await _temporalWorker.ExecuteAsync(async () =>
        {
            await _temporalClient
                .ExecuteMigrationAsync(
                    new WorkflowOptions
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskQueue = "test",
                    });
        });

        Assert.True(true);
    }

    [LoggerMessage(
        EventId = 100,
        EventName = "Starting Test",
        Level = LogLevel.Information,
        Message = "Starting Test")]
    private partial void LogTestStarted();
}

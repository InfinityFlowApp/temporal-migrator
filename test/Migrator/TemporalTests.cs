// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests;

using Collections;
using Fixtures;
using Grpc.Net.Client.Configuration;
using Temporalio.Client;
using RetryPolicy = Temporalio.Common.RetryPolicy;

/// <summary>
/// Temporal Tests.
/// </summary>
[Collection(TemporalCollectionFixture.Name)]
public class TemporalTests
{
    private readonly ITemporalClient _temporalClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalTests"/> class.
    /// </summary>
    /// <param name="fixture">The temporal fixture.</param>
    public TemporalTests(TemporalFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _temporalClient = fixture.TemporalClient!;
    }

    /// <summary>
    /// Test 1.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TemporalWorkflowFromStartToFinish()
    {
        await _temporalClient
            .RunMigrator(
                new WorkflowOptions
                {
                    Id = Guid.NewGuid().ToString(),
                    TaskQueue = "test",
                });

        Assert.True(true);
    }
}

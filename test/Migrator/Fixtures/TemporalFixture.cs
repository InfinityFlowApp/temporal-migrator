// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests.Fixtures;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Temporalio.Client;
using Temporalio.Converters;
using Temporalio.Testing;
using Temporalio.Worker;

/// <summary>
/// Temporal Fixture.
/// </summary>
public sealed partial class TemporalFixture : IAsyncLifetime
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<TemporalTests>();
    private WorkflowEnvironment? _workflowEnvironment;
    private TemporalWorker? _temporalWorker;

    /// <summary>
    /// Gets the temporal client.
    /// </summary>
    /// <value>The temporal client.</value>
    public ITemporalClient? TemporalClient => _workflowEnvironment?.Client;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        LogInitialized();
        var serializerOptions = new JsonSerializerOptions();
        serializerOptions.Converters.Add(new TypeJsonConverter());
        _workflowEnvironment = await WorkflowEnvironment.StartLocalAsync(new WorkflowEnvironmentStartLocalOptions
        {
            Namespace = "test",
            LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole()),
            DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter()),
        });

        var options = new TemporalWorkerOptions
            {
                TaskQueue = "test",
                DebugMode = true,
            }
            .AddWorkflow<MigrationWorkflow>()
            .AddAllActivities(new MigrationActivities());

        _temporalWorker = new TemporalWorker(TemporalClient!, options);

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    /// <inheritdoc/>
    public Task DisposeAsync()
    {
        LogDisposed();
        return _workflowEnvironment?.DisposeAsync().AsTask() ?? Task.CompletedTask;
    }

    [LoggerMessage(
        EventId = 100,
        EventName = "Initialize",
        Level = LogLevel.Information,
        Message = $"Initialize {nameof(TemporalFixture)}")]
    private partial void LogInitialized();

    [LoggerMessage(
        EventId = 999,
        EventName = "Dispose",
        Level = LogLevel.Information,
        Message = $"Dispose {nameof(TemporalFixture)}")]
    private partial void LogDisposed();
}

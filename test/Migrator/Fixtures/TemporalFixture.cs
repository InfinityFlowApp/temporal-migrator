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
public sealed partial class TemporalFixture : IAsyncLifetime, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<TemporalTests>();
    private WorkflowEnvironment? _workflowEnvironment;
    private TemporalWorker? _temporalWorker;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalFixture"/> class.
    /// </summary>
    public TemporalFixture()
    {
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="TemporalFixture"/> class.
    /// </summary>
    ~TemporalFixture()
    {
        Dispose(false);
    }

    /// <summary>
    /// Gets the temporal client.
    /// </summary>
    /// <value>The temporal client.</value>
    public ITemporalClient? TemporalClient => _workflowEnvironment?.Client;

    /// <summary>
    /// Gets the temporal worker.
    /// </summary>
    /// <value>The temporal worker.</value>
    public TemporalWorker? TemporalWorker => _temporalWorker;

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
                LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole()),
            }
            .AddWorkflow<MigrationWorkflow>()
            .AddAllActivities(new MigrationActivities());

        _temporalWorker = new TemporalWorker(TemporalClient!, options);

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        LogDisposed();

        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose.
    /// </summary>
    /// <param name="disposing">The disposing.</param>
    public void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _temporalWorker?.Dispose();
        _temporalWorker = null;
    }

    private async ValueTask DisposeAsyncCore()
    {
        if (_workflowEnvironment is not null)
        {
            await _workflowEnvironment.DisposeAsync().ConfigureAwait(false);
        }

        _workflowEnvironment = null;
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

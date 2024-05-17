// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests.Fixtures;

using System.Text.Json;
using Abstractions;
using Temporalio.Client;
using Temporalio.Converters;

/// <summary>
/// Temporal Fixture.
/// </summary>
public sealed class TemporalFixture : IAsyncLifetime
{
    private DistributedApplication? _application;
    private ITemporalClient? _temporalClient;

    /// <summary>
    /// Gets the application.
    /// </summary>
    /// <value>The application.</value>
    public DistributedApplication? Application => _application;

    /// <summary>
    /// Gets the temporal client.
    /// </summary>
    /// <value>The temporal client.</value>
    public ITemporalClient? TemporalClient => _temporalClient;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.InfinityFlow_Temporal_Migrator_AppHost>();
        _application = await appHost.BuildAsync();
        await _application.StartAsync();
        var targetHost = _application.GetEndpoint("temporal", "server");
        _temporalClient =
            new TemporalClient(
                await TemporalConnection.ConnectAsync(new TemporalConnectionOptions(targetHost.Authority)),
                new TemporalClientOptions
                {
                    Namespace = "test",
                });
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        if (_application is not null)
        {
            await _application.StopAsync();
            await _application.DisposeAsync();
        }

        _temporalClient = null;
        _application = null;
    }
}

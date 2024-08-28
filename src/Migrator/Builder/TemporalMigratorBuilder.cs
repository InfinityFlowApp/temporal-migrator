// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Builder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

/// <summary>
/// Temporal Migrator Builder.
/// </summary>
internal class TemporalMigratorBuilder : ITemporalMigratorBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalMigratorBuilder"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="clientOptionsBuilder">The client options builder.</param>
    /// <param name="migratorOptionsBuilder">The migrator options builder.</param>
    /// <param name="workerOptionsBuilder">The worker options builder.</param>
    public TemporalMigratorBuilder(
        string name,
        IServiceCollection serviceCollection,
        OptionsBuilder<TemporalClientConnectOptions> clientOptionsBuilder,
        OptionsBuilder<MigratorOptions> migratorOptionsBuilder,
        ITemporalWorkerServiceOptionsBuilder workerOptionsBuilder)
    {
        Name = name;
        Services = serviceCollection;
        ClientOptionsBuilder = clientOptionsBuilder;
        MigratorOptionsBuilder = migratorOptionsBuilder;
        WorkerOptionsBuilder = workerOptionsBuilder;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <inheritdoc/>
    public OptionsBuilder<MigratorOptions> MigratorOptionsBuilder { get; }

    /// <inheritdoc/>
    public ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}

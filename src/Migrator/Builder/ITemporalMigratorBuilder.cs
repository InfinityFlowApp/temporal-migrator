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
public interface ITemporalMigratorBuilder
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>The services.</value>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the client options builder.
    /// </summary>
    /// <value>The client options.</value>
    internal OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <summary>
    /// Gets the migrator options builder.
    /// </summary>
    /// <value>The migrator options builder.</value>
    internal OptionsBuilder<MigratorOptions> MigratorOptionsBuilder { get; }

    /// <summary>
    /// Gets the worker builder.
    /// </summary>
    /// <value>The workflow builder.</value>
    internal ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}

// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using Builder;
using Microsoft.Extensions.Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

/// <summary>
/// Temporal Migrator Builder Extensions.
/// </summary>
public static class TemporalMigratorBuilderExtensions
{
    /// <summary>
    /// Configure Client Options.
    /// </summary>
    /// <param name="temporalMigratorBuilder">The initial temporal migrator builder.</param>
    /// <param name="configureOptions">The configure client options.</param>
    /// <returns>The updated temporal migrator builder.</returns>
    public static ITemporalMigratorBuilder ConfigureClientOptions(
        this ITemporalMigratorBuilder temporalMigratorBuilder,
        Action<OptionsBuilder<TemporalClientConnectOptions>> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalMigratorBuilder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        configureOptions.Invoke(temporalMigratorBuilder.ClientOptionsBuilder);
        return temporalMigratorBuilder;
    }

    /// <summary>
    /// Configure Client Options.
    /// </summary>
    /// <param name="temporalMigratorBuilder">The initial temporal migrator builder.</param>
    /// <param name="configureOptions">The configure client options.</param>
    /// <returns>The updated temporal migrator builder.</returns>
    public static ITemporalMigratorBuilder ConfigureClientOptions(
        this ITemporalMigratorBuilder temporalMigratorBuilder,
        Action<TemporalClientConnectOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalMigratorBuilder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        return ConfigureClientOptions(temporalMigratorBuilder, builder => builder.Configure(configureOptions));
    }

    /// <summary>
    /// Configure Migrator Options.
    /// </summary>
    /// <param name="temporalMigratorBuilder">The initial temporal migrator builder.</param>
    /// <param name="configureOptions">The configure migrator options.</param>
    /// <returns>The updated temporal migrator builder.</returns>
    public static ITemporalMigratorBuilder ConfigureMigratorOptions(
        this ITemporalMigratorBuilder temporalMigratorBuilder,
        Action<OptionsBuilder<MigratorOptions>> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalMigratorBuilder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        configureOptions.Invoke(temporalMigratorBuilder.MigratorOptionsBuilder);
        return temporalMigratorBuilder;
    }

    /// <summary>
    /// Configure Migrator Options.
    /// </summary>
    /// <param name="temporalMigratorBuilder">The initial migrator builder.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns>The updated migrator builder.</returns>
    public static ITemporalMigratorBuilder ConfigureMigratorOptions(
        this ITemporalMigratorBuilder temporalMigratorBuilder,
        Action<MigratorOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalMigratorBuilder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        return ConfigureMigratorOptions(temporalMigratorBuilder, builder => builder.Configure(configureOptions));
    }

    /// <summary>
    /// Configure Worker Options.
    /// </summary>
    /// <param name="temporalMigratorBuilder">The initial temporal migrator builder.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns>The updated temporal migrator builder.</returns>
    public static ITemporalMigratorBuilder ConfigureWorkerOptions(
        this ITemporalMigratorBuilder temporalMigratorBuilder,
        Action<ITemporalWorkerServiceOptionsBuilder> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(temporalMigratorBuilder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        configureOptions.Invoke(temporalMigratorBuilder.WorkerOptionsBuilder);
        return temporalMigratorBuilder;
    }
}

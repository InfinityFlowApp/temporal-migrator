// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace Microsoft.Extensions.Hosting;

using System.Text.Json;
using DependencyInjection;
using DependencyInjection.Extensions;
using InfinityFlow.Temporal.Migrator;
using InfinityFlow.Temporal.Migrator.Builder;
using Logging;
using Options;
using Temporalio.Client;
using Temporalio.Converters;
using Temporalio.Extensions.Hosting;
using Temporalio.Extensions.OpenTelemetry;

/// <summary>
/// Service Collection Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Temporal Migrations.
    /// </summary>
    /// <param name="services">The initial service collection.</param>
    /// <param name="taskQueue">The task queue.</param>
    /// <param name="clientTargetHost">The client target host.</param>
    /// <param name="clientNamespace">The client namespace.</param>
    /// <param name="buildId">The build id.</param>
    /// <returns>The updated service collection.</returns>
    public static ITemporalMigratorBuilder AddTemporalMigrations(
        this IServiceCollection services,
        string taskQueue = MigratorOptions.DefaultTaskQueueName,
        string clientTargetHost = MigratorOptions.DefaultClientTargetHost,
        string clientNamespace = MigratorOptions.DefaultClientNamespace,
        string? buildId = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(taskQueue);
        ArgumentNullException.ThrowIfNull(clientTargetHost);
        ArgumentNullException.ThrowIfNull(clientNamespace);

        var clientOptionsBuilder = services
            .AddKeyedTemporalClient(taskQueue, clientTargetHost, clientNamespace)
            .Configure(options =>
            {
                var serializerOptions = new JsonSerializerOptions();
                serializerOptions.Converters.Add(new TypeJsonConverter());
                options.DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter());
                options.Interceptors = [new TracingInterceptor()];
            });

        var workerOptionsBuilder = services
            .AddHostedTemporalWorker(clientTargetHost, clientNamespace, taskQueue, buildId)
            .AddWorkflow<MigrationWorkflow>()
            .AddSingletonActivities<MigrationActivities>();

        var migratorOptionsBuilder = services
            .AddOptions<MigratorOptions>(taskQueue);

        return new TemporalMigratorBuilder(
            taskQueue,
            services,
            clientOptionsBuilder,
            migratorOptionsBuilder,
            workerOptionsBuilder);
    }

    /// <summary>
    /// Add Keyed Temporal Client.
    /// </summary>
    /// <param name="services">The initial service collection.</param>
    /// <param name="taskQueue">The task queue.</param>
    /// <param name="clientTargetHost">The client target host.</param>
    /// <param name="clientNamespace">The client namespace.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentException">Throws exception when service key is invalid type.</exception>
    private static OptionsBuilder<TemporalClientConnectOptions> AddKeyedTemporalClient(
        this IServiceCollection services,
        string taskQueue,
        string? clientTargetHost = default,
        string? clientNamespace = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddKeyedSingleton<ITemporalClient>(taskQueue, (serviceProvider, _) =>
            TemporalClient.CreateLazy(serviceProvider.GetRequiredService<IOptionsMonitor<TemporalClientConnectOptions>>().Get(taskQueue)));

        var builder = services.AddOptions<TemporalClientConnectOptions>(taskQueue);

        if (clientTargetHost is not null || clientNamespace is not null)
        {
            builder.Configure(options =>
            {
                options.TargetHost = clientTargetHost;

                if (clientNamespace is not null)
                {
                    options.Namespace = clientNamespace;
                }
            });
        }

        builder.Configure<IServiceProvider>((options, provider) =>
        {
            if (provider.GetService<ILoggerFactory>() is { } loggerFactory)
            {
                options.LoggerFactory = loggerFactory;
            }
        });

        return builder;
    }
}

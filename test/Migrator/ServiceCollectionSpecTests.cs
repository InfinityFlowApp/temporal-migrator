// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests;

using Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;
using Worker.Migrations;

/// <summary>
/// Builder Usage Tests.
/// </summary>
public class ServiceCollectionSpecTests
{
    /// <summary>
    /// The most simple test case for using temporal migrations.
    /// </summary>
    [Fact]
    public void TestAddTemporalMigrations()
    {
        var services = InitServiceCollection();
        var migratorBuilder = services.AddTemporalMigrations();
        Assert.NotNull(migratorBuilder);
        Assert.IsType<TemporalMigratorBuilder>(migratorBuilder);
    }

    /// <summary>
    /// The test case where adding only one additional assembly.
    /// </summary>
    [Fact]
    public void TestAddTemporalMigrationsWithAssembly()
    {
        var services = InitServiceCollection();
        var migratorBuilder = services
            .AddTemporalMigrations();

        migratorBuilder
            .ConfigureMigratorOptions(builder => builder
                .AddAssembly(typeof(M1).Assembly));
        Assert.NotNull(migratorBuilder);
        Assert.IsType<TemporalMigratorBuilder>(migratorBuilder);

        var sp = services.BuildServiceProvider();
        var migratorOptions = sp.GetRequiredService<IOptionsMonitor<MigratorOptions>>().Get(MigratorOptions.DefaultTaskQueueName);
        Assert.Single(migratorOptions.Assemblies);
    }

    /// <summary>
    /// Test case where adding multiple assemblies.
    /// </summary>
    [Fact]
    public void TestAddTemporalMigrationsWithAssemblies()
    {
        var services = InitServiceCollection();
        var migratorBuilder = services
            .AddTemporalMigrations()
            .ConfigureMigratorOptions(builder => builder
                .AddAssemblies(typeof(M1).Assembly, typeof(ServiceCollection).Assembly));
        Assert.NotNull(migratorBuilder);
        Assert.IsType<TemporalMigratorBuilder>(migratorBuilder);

        var sp = services.BuildServiceProvider();
        var migratorOptions = sp.GetRequiredService<IOptionsMonitor<MigratorOptions>>().Get(MigratorOptions.DefaultTaskQueueName);
        Assert.Equal(2, migratorOptions.Assemblies.Count);
    }

    /// <summary>
    /// Test case where setting identity in client options.
    /// </summary>
    [Fact]
    public void TestAddTemporalMigrationsWithSettingIdentityInClientOptions()
    {
        var services = InitServiceCollection();
        var migratorBuilder = services
            .AddTemporalMigrations()
            .ConfigureClientOptions(builder =>
            {
                builder.Identity = "data";
            });
        Assert.NotNull(migratorBuilder);
        Assert.IsType<TemporalMigratorBuilder>(migratorBuilder);

        var sp = services.BuildServiceProvider();
        var migratorOptions = sp.GetRequiredService<IOptionsMonitor<TemporalClientConnectOptions>>().Get(MigratorOptions.DefaultTaskQueueName);
        Assert.Equal("data", migratorOptions.Identity);
    }

    /// <summary>
    /// Test case where setting identity in workflow options.
    /// </summary>
    [Fact]
    public void TestAddTemporalMigrationsWithSingleHostedService()
    {
        var services = InitServiceCollection();
        var migratorBuilder = services
            .AddTemporalMigrations()
            .ConfigureWorkerOptions(builder =>
            {
                builder.AddWorkflow<MigrationWorkflow>();
            });
        Assert.NotNull(migratorBuilder);
        Assert.IsType<TemporalMigratorBuilder>(migratorBuilder);

        var sp = services.BuildServiceProvider();
        Assert.Single(sp.GetServices<IHostedService>());
    }

    private static ServiceCollection InitServiceCollection()
    {
        return [];
    }
}

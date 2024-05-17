// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

using InfinityFlow.Temporal.Migrator;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalClient(options =>
    {
        options.Namespace = "test";
        options.TargetHost = builder.Configuration.GetConnectionString("temporal");
    })
    .AddHostedTemporalWorker(
        builder.Configuration.GetConnectionString("temporal")!,
        "test",
        "test")
    .AddWorkflow<MigrationWorkflow>();

var host = builder.Build();

var temporalClient = host.Services.GetRequiredService<ITemporalClient>();

temporalClient.RunMigrator(new WorkflowOptions { Id = Guid.NewGuid().ToString(), TaskQueue = "test" });

host.Run();

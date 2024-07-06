// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

using System.Text.Json;
using InfinityFlow.Temporal.Migrator;
using Temporalio.Converters;
using Temporalio.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations(taskQueue: "test", clientNamespace: "test");

var host = builder.Build();

// var temporalClient = host.Services.GetRequiredService<ITemporalClient>();
// temporalClient.RunMigrator(new WorkflowOptions { Id = Guid.NewGuid().ToString(), TaskQueue = "test" });
host.Run();

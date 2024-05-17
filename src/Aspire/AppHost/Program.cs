// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

using InfinityFlow.Aspire.Temporal;

var builder = DistributedApplication.CreateBuilder(args);

var temporal = await builder
    .AddTemporalServerContainer("temporal", temporal => temporal
        .WithLogFormat(LogFormat.Pretty)
        .WithLogLevel(LogLevel.Debug)
        .WithNamespace("test"));

builder
    .AddProject<Projects.InfinityFlow_Temporal_Migrator_Tests_Worker>("worker")
    .WithReference(temporal);

await builder.Build().RunAsync();

# InfinityFlow Temporal Migrator

[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/InfinityFlowApp/temporal-migrator/release.yml)](https://github.com/InfinityFlowApp/temporal-migrator/actions?query=branch%3Amain)
[![GitHub Release](https://img.shields.io/github/v/release/InfinityFlowApp/temporal-migrator)](https://github.com/InfinityFlowApp/temporal-migrator/releases)
[![GitHub License](https://img.shields.io/github/license/InfinityFlowApp/temporal-migrator)](https://github.com/InfinityFlowApp/temporal-migrator?tab=Apache-2.0-1-ov-file#readme)

[![Discord](https://discordapp.com/api/guilds/1148334798524383292/widget.png?style=banner2)](https://discord.gg/zyJQx44q)

The Temporal Migrations Framework allows you to create and manage migration across your cloud-native solution.
This framework, inspired by FluentMigrator, leverages the power of Temporal.io for building reliable and scalable workflows.

# Table of Contents

* [Introduction](#introduction)
* [Features](#features)
* [Installation](#installation)
* [Setup](#setup)
  * [Customize Client Options](#customize-client-options)
  * [Limiting access to migrations](#limiting-access-to-migrations)
  * [Create a migration class](#create-a-migration-class)
  * [Running a migration](#running-a-migration)
  * [A migration is Temporal.io workflow](#a-migration-is-temporalio-workflow)
    * [Register workflows](#register-workflows)
    * [Using workflows](#using-workflows)
  * [Migrations can contain activities](#migrations-can-contain-activities)
    * [Register activities](#register-activities)
    * [Using activities](#using-activities)
* [Contributing](#contributing)
* [License](#license)

# Introduction

Temporal Migrations Framework is designed to facilitate the creation, management, and execution of migrations in cloud-native applications.
By using Temporal.io's workflow and activity model, this framework ensures migrations are executed reliably and can be monitored effectively.

# Features

- **Seamless Migration Creation**: Define migrations using simple interface and attribute system.
- **Temporal.io Integration**: Leverage Temporal.io workflows and activities for reliable migration execution.

# Installation

```powershell
Install-Package InfinityFlow.Temporal.Migrator
```

# Setup

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations();
```

## Customize Client Options

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations()
    .ConfigureClientOptions(builder => ...);
```

Default options include `OpenTelemetry` interceptor, and `Type` serializer needed for migrations to work.

## Limiting access to migrations

It is possible to specify in which assemblies to scan for migrations

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations()
    .ConfigureMigratorOptions(builder => builder
        .AddAssemblies(typeof(M1).Assembly, typeof(M2).Assembly));
```

## Create a migration class

```csharp
[Migration(1)]
public class M1 : IMigration
{
    public ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        // Code to run

        return ValueTask.CompletedTask;
    }
}
```

## Running a migration

```csharp
var client = serviceProvider.GetRequiredKeyedService<ITemporalClient>(MigrationOptions.DefaultTaskQueue);
await client.ExecuteMigration(new WorkflowOptions { Id = "migration", });
```

## A migration is Temporal.io workflow

As migration is workflow class, you can start or execute child workflows, or use activities, in this case you have to
register your workflows and activities

### Register workflows

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations()
    .ConfigureWorkerOptions(options => options
        .AddWorkflow<MyWorkflow>());
```

### Using workflows
```csharp
[Migration(1)]
public class M1 : IMigration
{
    /// <inheritdoc />
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Workflow.ExecuteChildWorkflowAsync<MyWorkflow>(workflow => workflow.MyMethod(),
            new ChildWorkflowOptions { Id = "my_child_workflow_id" });
        return ValueTask.CompletedTask;
    }
}
```

## Migrations can contain activities

### Register activities

Unlike workflow which only support deterministic execution, activity allows dependency injection with all lifetimes:
- Singleton `AddSingletonActivities<T>`
- Transient `AddTransientActivities<T>`
- Scoped `AddScopedActivities<T>`

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder
    .Services
    .AddTemporalMigrations()
    .ConfigureWorkerOptions(options => options
        .AddScopedActivities<MyActivities>());
```

### Using activities

```csharp
[Migration(1)]
public class M1 : IMigration
{
    /// <inheritdoc />
    public async ValueTask ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Workflow.ExecuteActivityAsync<MyActivities>(activity => activity.MyMethod(),
            new ActivityOptions { ActivityId = "my_activitiy_id" });
        return ValueTask.CompletedTask;
    }
}
```

# Contributing

If you'd like to contribute to temporal-migrator please fork the repository and make changes as you'd like.
Pull requests are warmly welcome.

# License
This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

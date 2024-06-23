// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

#pragma warning disable CA1812

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Abstractions;
using Microsoft.Extensions.Options;
using Temporalio.Activities;

/// <summary>
/// Migration Activities.
/// </summary>
internal class MigrationActivities
{
    private readonly IOptions<MigratorOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationActivities"/> class.
    /// </summary>
    /// <param name="options">The migrator options.</param>
    public MigrationActivities(IOptions<MigratorOptions> options)
    {
        _options = options;
    }

    /// <summary>
    /// Get Types.
    /// </summary>
    /// <returns>The <see cref="Task"/>.</returns>
    [Activity]
    public Task<IEnumerable<Type>> GetTypesAsync()
    {
        var assemblies = _options.Value.Assemblies;
        var migrationTypes = GlobalReflector.GetMigrations(assemblies.ToArray());
        var richTypes = migrationTypes.Select(t => new { Type = t, t.GetCustomAttribute<MigrationAttribute>()!.Version, });
        var sortedTypes = richTypes.OrderBy(t => t.Version);
        var returnedTypes = sortedTypes.Select(type => type.Type);
        return Task.FromResult(returnedTypes);
    }
}

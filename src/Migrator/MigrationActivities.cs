// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Abstractions;
using Temporalio.Activities;

/// <summary>
/// Migration Activities.
/// </summary>
public class MigrationActivities
{
    /// <summary>
    /// Get Types.
    /// </summary>
    /// <param name="assemblies">The assemblies.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    [Activity]
    public Task<IEnumerable<Type>> GetTypesAsync(string[]? assemblies = null)
    {
        assemblies ??= [];
        var migrationTypes = GlobalReflector.GetMigrations(assemblies);
        var richTypes = migrationTypes.Select(t => new { Type = t, t.GetCustomAttribute<MigrationAttribute>()!.Version, });
        var sortedTypes = richTypes.OrderBy(t => t.Version);
        var returnedTypes = sortedTypes.Select(type => type.Type);
        return Task.FromResult(returnedTypes);
    }
}

// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Microsoft.Extensions.Options;

/// <summary>
/// Migrator Options Builder Extensions.
/// </summary>
public static class MigratorOptionsBuilderExtensions
{
    /// <summary>
    /// Add Assembly.
    /// </summary>
    /// <param name="builder">The initial migrator options builder.</param>
    /// <param name="assembly">The assembly.</param>
    /// <returns>The updated migrator options builder.</returns>
    public static OptionsBuilder<MigratorOptions> AddAssembly(this OptionsBuilder<MigratorOptions> builder, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assembly);
        return builder.Configure(options => options.Assemblies.Add(assembly));
    }

    /// <summary>
    /// Add Assemblies.
    /// </summary>
    /// <param name="builder">The initial migrator options builder.</param>
    /// <param name="assemblies">The assemblies to add.</param>
    /// <returns>The updated migrator options builder.</returns>
    public static OptionsBuilder<MigratorOptions> AddAssemblies(
        this OptionsBuilder<MigratorOptions> builder,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assemblies);
        return builder.Configure(options =>
            {
                foreach (var assembly in assemblies)
                {
                    options.Assemblies.Add(assembly);
                }
            });
    }

    /// <summary>
    /// Add Assemblies.
    /// </summary>
    /// <param name="builder">The initial migrator options builder.</param>
    /// <param name="assemblies">The assemblies to add.</param>
    /// <returns>The updated migrator options builder.</returns>
    public static OptionsBuilder<MigratorOptions> AddAssemblies(
        this OptionsBuilder<MigratorOptions> builder,
        IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assemblies);
        return builder.Configure(options =>
        {
            foreach (var assembly in assemblies)
            {
                options.Assemblies.Add(assembly);
            }
        });
    }
}

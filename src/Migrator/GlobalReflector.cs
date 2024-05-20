// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Reflection;
using Abstractions;

/// <summary>
/// Global Reflector.
/// </summary>
internal static class GlobalReflector
{
    /// <summary>
    /// Gets the migration type.
    /// </summary>
    /// <value>The migration type.</value>
    public static Type Migration => typeof(IMigration);

    /// <summary>
    /// Get All Types.
    /// </summary>
    /// <returns>The list of types.</returns>
    public static AppDomain GetDomain() => AppDomain
        .CurrentDomain;

    /// <summary>
    /// Get Migrations.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The migration types.</returns>
    public static IEnumerable<Type> GetMigrations(Assembly[]? assemblies = null) => (assemblies is null || assemblies.Length == 0
            ? GetDomain().GetAssemblies()
            : assemblies)
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => Migration.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

    /// <summary>
    /// Get Migrations.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The migration types.</returns>
    public static IEnumerable<Type> GetMigrations(string[]? assemblies = null) =>
        GetMigrations(assemblies is null ? null : ConvertAssembliesFromString(assemblies));

    private static Assembly[] ConvertAssembliesFromString(string[] assemblies)
    {
        return assemblies.Select(Assembly.Load).ToArray();
    }
}

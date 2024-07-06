// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Diagnostics;
using System.Reflection;
using Abstractions;
using Microsoft.Extensions.DependencyModel;

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
    /// Get Migrations.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The migration types.</returns>
    public static IEnumerable<Type> GetMigrations(Assembly[]? assemblies = null) => (assemblies is null || assemblies.Length == 0
            ? GetRuntimeAssemblies()
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

    /// <summary>
    /// Get Runtime Assemblies.
    /// </summary>
    /// <returns>A list of assemblies that the application depends on.</returns>
    public static Assembly[] GetRuntimeAssemblies()
    {
        var assemblies = new List<Assembly>();
        var dependencies = DependencyContext.Default?.RuntimeLibraries;

        if (dependencies is null)
        {
            return assemblies.ToArray();
        }

        foreach (var library in dependencies)
        {
            try
            {
                var assembly = Assembly.Load(library.Name);
                assemblies.Add(assembly);
            }
            catch (TypeLoadException tle)
            {
                Debug.WriteLine("Loading {0} failed", (object)tle.TypeName);
            }
            catch (FileNotFoundException fnfe)
            {
                Debug.WriteLine("Loading {0} failed (Not Found)", (object?)fnfe.FileName);
            }
        }

        Debug.WriteLine("Loaded {0} Assemblies", assemblies.Count);

        return assemblies.ToArray();
    }

    private static Assembly[] ConvertAssembliesFromString(string[] assemblies)
    {
        return assemblies.Select(Assembly.Load).ToArray();
    }
}

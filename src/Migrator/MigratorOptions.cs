// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator;

using System.Collections.ObjectModel;
using System.Reflection;

/// <summary>
/// Migrator Options.
/// </summary>
public class MigratorOptions
{
    /// <summary>
    /// Gets the assemblies.
    /// </summary>
    /// <value>The assemblies.</value>
    public Collection<Assembly> Assemblies { get; init; } = [];
}

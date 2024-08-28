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
    /// Default Task Queue Name.
    /// </summary>
    public const string DefaultTaskQueueName = "migration";

    /// <summary>
    /// Default Client Target Host.
    /// </summary>
    public const string DefaultClientTargetHost = "localhost:7233";

    /// <summary>
    /// Default Client Namespace.
    /// </summary>
    public const string DefaultClientNamespace = "default";

    /// <summary>
    /// Gets the assemblies.
    /// </summary>
    /// <value>The assemblies.</value>
    internal Collection<Assembly> Assemblies { get; init; } = [];
}

// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Abstractions;

/// <summary>
/// Migration.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class MigrationAttribute(long version) : Attribute
{
    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>The version.</value>
    public long Version { get; } = version;
}

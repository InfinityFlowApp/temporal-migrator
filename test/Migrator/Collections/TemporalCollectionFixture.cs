// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Tests.Collections;

using Fixtures;

/// <summary>
/// Temporal Collection Fixture.
/// </summary>
[CollectionDefinition(Name)]
public class TemporalCollectionFixture : ICollectionFixture<TemporalFixture>
{
    /// <summary>
    /// Temporal Collection Name.
    /// </summary>
    public const string Name = "TemporalCollection";
}

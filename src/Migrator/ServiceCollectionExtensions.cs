// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace Microsoft.Extensions.Hosting;

using DependencyInjection;

/// <summary>
/// Service Collection Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Temporal Migrations.
    /// </summary>
    /// <param name="services">The initial service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddTemporalMigrations(this IServiceCollection services)
    {
        return services;
    }
}

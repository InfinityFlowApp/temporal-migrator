// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace Aspire.Hosting;

using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// An annotation that associates a health check factory with a resource.
/// </summary>
/// <param name="healthCheckFactory">A function that creates the health check.</param>
public class HealthCheckAnnotation(Func<IResource, CancellationToken, Task<IHealthCheck?>> healthCheckFactory) : IResourceAnnotation
{
    /// <summary>
    /// Gets the health check factory.
    /// </summary>
    /// <value>
    /// The health check factory.
    /// </value>
    public Func<IResource, CancellationToken, Task<IHealthCheck?>> HealthCheckFactory { get; } = healthCheckFactory;

    /// <summary>
    /// Create Health Check Annotation.
    /// </summary>
    /// <param name="connectionStringFactory">The connection string factory.</param>
    /// <returns>The health check annotation.</returns>
    public static HealthCheckAnnotation Create(Func<string, IHealthCheck> connectionStringFactory)
    {
        return new(async (resource, token) =>
        {
            if (resource is not IResourceWithConnectionString c)
            {
                return null;
            }

            return await c.GetConnectionStringAsync(token) is not { } cs
                ? null
                : connectionStringFactory(cs);
        });
    }
}

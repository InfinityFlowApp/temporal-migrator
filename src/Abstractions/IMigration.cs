// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

namespace InfinityFlow.Temporal.Migrator.Abstractions;

/// <summary>
/// Migration.
/// </summary>
public interface IMigration
{
    /// <summary>
    /// Execute.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    ValueTask ExecuteAsync(CancellationToken cancellationToken);
}

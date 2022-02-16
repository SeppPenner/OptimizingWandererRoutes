// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimizeNotCalledException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exception that is thrown whenever the optimize function was not called yet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The exception that is thrown whenever the optimize function was not called yet.
/// </summary>
public class OptimizeNotCalledException : Exception
{
    /// <inheritdoc cref="Exception"/>
    public OptimizeNotCalledException()
    {
    }

    /// <inheritdoc cref="Exception"/>
    public OptimizeNotCalledException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="Exception"/>
    public OptimizeNotCalledException(string message, Exception inner) : base(message, inner)
    {
    }
}

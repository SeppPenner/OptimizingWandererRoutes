// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TooLessStagesException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exception that is thrown whenever the file was not read yet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The exception that is thrown whenever the file was not read yet.
/// </summary>
public class TooLessStagesException : Exception
{
    /// <inheritdoc cref="Exception"/>
    public TooLessStagesException()
    {
    }

    /// <inheritdoc cref="Exception"/>
    public TooLessStagesException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="Exception"/>
    public TooLessStagesException(string message, Exception inner) : base(message, inner)
    {
    }
}

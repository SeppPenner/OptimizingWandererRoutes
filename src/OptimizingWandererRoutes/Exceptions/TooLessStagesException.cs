// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TooLessStagesException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exception that is thrown whenever the input file holds fewer stages than it announces.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Exceptions;

/// <inheritdoc cref="Exception"/>
/// <summary>
/// The exception that is thrown whenever the input file holds fewer stages than it announces.
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

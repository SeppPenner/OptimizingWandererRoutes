// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNotReadException.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exception that is thrown whenever the file was not read yet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace OptimizingWandererRoutes.Exceptions
{
    using System;

    /// <inheritdoc cref="Exception"/>
    /// <summary>
    /// The exception that is thrown whenever the file was not read yet.
    /// </summary>
    public class FileNotReadException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        // ReSharper disable once UnusedMember.Global
        public FileNotReadException()
        {
        }

        /// <inheritdoc cref="Exception"/>
        public FileNotReadException(string message) : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        // ReSharper disable once UnusedMember.Global
        public FileNotReadException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
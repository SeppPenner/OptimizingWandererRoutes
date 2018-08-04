using System;

namespace OptimizingWandererRoutes.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The exception that is thrown whenever the file was not read yet.
    /// </summary>
    public class FileNotReadException : Exception
    {
        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public FileNotReadException()
        {
        }

        /// <inheritdoc />
        public FileNotReadException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public FileNotReadException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
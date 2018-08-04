using System;

namespace OptimizingWandererRoutes.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The exception that is thrown whenever the file was not read yet.
    /// </summary>
    public class TooLessStagesException : Exception
    {
        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public TooLessStagesException()
        {
        }

        /// <inheritdoc />
        public TooLessStagesException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public TooLessStagesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
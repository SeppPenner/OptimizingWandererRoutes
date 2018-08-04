using System;

namespace OptimizingWandererRoutes.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The exception that is thrown whenever the optimize function was not called yet.
    /// </summary>
    public class OptimizeNotCalledException : Exception
    {
        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public OptimizeNotCalledException()
        {
        }

        /// <inheritdoc />
        public OptimizeNotCalledException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        // ReSharper disable once UnusedMember.Global
        public OptimizeNotCalledException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
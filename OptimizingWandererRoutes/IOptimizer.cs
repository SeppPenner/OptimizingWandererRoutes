using System;
using System.IO;
using OptimizingWandererRoutes.Exceptions;

namespace OptimizingWandererRoutes
{
    /// <summary>
    ///     Class to optimize the wanderers route according to their preferences.
    /// </summary>
    public interface IOptimizer
    {
        /// <summary>
        ///     Prints out the results. Throws an <see cref="Exception" /> of type <see cref="FileNotReadException" /> if the
        ///     file was not read yet. Throws an <see cref="Exception" /> of type <see cref="OptimizeNotCalledException" /> if
        ///     the optimize function was not called yet.
        /// </summary>
        void PrintResults();

        /// <summary>
        ///     Optimizes the wanderer's routes. Throws an <see cref="Exception" /> of type <see cref="FileNotReadException" />
        ///     if the file was not read yet.
        /// </summary>
        void Optimize();

        /// <summary>
        ///     Reads an input file. Throws an <see cref="Exception" /> of type if <see cref="FileNotFoundException" /> if the
        ///     given file name was not found. Throws am <see cref="Exception"/> of type <see cref="TooLessStagesException"/> if
        ///     the number of stages exceeds the available stages in the file.
        /// </summary>
        /// <param name="fileName">The input file to be read.</param>
        void ReadFile(string fileName);
    }
}
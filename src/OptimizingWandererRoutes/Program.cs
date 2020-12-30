// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes
{
    using System;
    using System.IO;

    using OptimizingWandererRoutes.Exceptions;

    /// <summary>
    /// The main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        public static void Main()
        {
            try
            {
                Console.WriteLine("Please type in a file name to read the data from and press return:");
                var fileName = Console.ReadLine();
                IOptimizer optimizer = new Optimizer();
                optimizer.ReadFile(fileName);
                optimizer.Optimize();
                optimizer.PrintResults();
            }
            catch (FileNotReadException ex1)
            {
                Console.WriteLine(ex1.Message);
                ShowEndMessage();
            }
            catch (OptimizeNotCalledException ex2)
            {
                Console.WriteLine(ex2.Message);
                ShowEndMessage();
            }
            catch (FileNotFoundException ex3)
            {
                Console.WriteLine(ex3.Message);
                ShowEndMessage();
            }
            catch (TooLessStagesException ex4)
            {
                Console.WriteLine(ex4.Message);
                ShowEndMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ShowEndMessage();
            }
        }

        /// <summary>
        /// Shows the end message.
        /// </summary>
        private static void ShowEndMessage()
        {
            Console.WriteLine("Press any key to stop the program...");
            Console.ReadLine();
        }
    }
}
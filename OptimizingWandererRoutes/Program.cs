using System;
using System.IO;
using OptimizingWandererRoutes.Exceptions;

namespace OptimizingWandererRoutes
{
    public static class Program
    {
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
            catch (FileNotReadException fnrex)
            {
                Console.WriteLine(fnrex.Message);
                ShowEndMessage();
            }
            catch (OptimizeNotCalledException oncex)
            {
                Console.WriteLine(oncex.Message);
                ShowEndMessage();
            }
            catch (FileNotFoundException fnfex)
            {
                Console.WriteLine(fnfex.Message);
                ShowEndMessage();
            }
            catch (TooLessStagesException tlsex)
            {
                Console.WriteLine(tlsex.Message);
                ShowEndMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ShowEndMessage();
            }
        }

        private static void ShowEndMessage()
        {
            Console.WriteLine("Press any key to stop the programm...");
            Console.ReadLine();
        }
    }
}
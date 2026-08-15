// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to provide the test data used in the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Tests;

/// <summary>
/// A class to provide the test data used in the tests.
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// The number of days of the example in Hausaufgabe-Programmierer.pdf.
    /// </summary>
    public const int TaskExampleNumberOfDays = 3;

    /// <summary>
    /// Gets the stages of the example in Hausaufgabe-Programmierer.pdf. These are the same values the file
    /// in2.txt of the program holds, which is what makes the result comparable to the task.
    /// </summary>
    /// <returns>The stages of the example in the task.</returns>
    public static int[] GetTaskExampleStages()
    {
        return new[] { 11, 16, 5, 5, 12, 10 };
    }

    /// <summary>
    /// Writes an input file in the format the program expects: the number of stages, the number of days and
    /// then one stage per line.
    /// </summary>
    /// <param name="directory">The directory the file is written to.</param>
    /// <param name="numberOfStages">The number of stages written into the first line.</param>
    /// <param name="numberOfDays">The number of days written into the second line.</param>
    /// <param name="stages">The stages written into the remaining lines.</param>
    /// <returns>The full name of the written file.</returns>
    public static string WriteInputFile(string directory, int numberOfStages, int numberOfDays, params int[] stages)
    {
        var fileName = Path.Combine(directory, $"in_{Guid.NewGuid():N}.txt");
        var lines = new List<string>
        {
            numberOfStages.ToString(CultureInfo.InvariantCulture),
            numberOfDays.ToString(CultureInfo.InvariantCulture)
        };
        lines.AddRange(stages.Select(stage => stage.ToString(CultureInfo.InvariantCulture)));
        File.WriteAllLines(fileName, lines);
        return fileName;
    }
}

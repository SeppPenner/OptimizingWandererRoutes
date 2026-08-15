// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimizerTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="Optimizer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Tests;

/// <summary>
/// A class to test the <see cref="Optimizer"/> class.
/// </summary>
[TestClass]
public class OptimizerTests
{
    /// <summary>
    /// The console output of the running test.
    /// </summary>
    private readonly StringWriter capturedOutput = new();

    /// <summary>
    /// The directory the input files of a single test are written to.
    /// </summary>
    private string testDirectory = string.Empty;

    /// <summary>
    /// The console output of the test host, restored after the test.
    /// </summary>
    private TextWriter originalOutput = TextWriter.Null;

    /// <summary>
    /// The console input of the test host, restored after the test.
    /// </summary>
    private TextReader originalInput = TextReader.Null;

    /// <summary>
    /// Creates an empty directory outside of the repository for the input files of the running test and takes
    /// the console over. The console input has to be replaced as well, because PrintResults waits for a line.
    /// </summary>
    [TestInitialize]
    public void RedirectTheConsole()
    {
        this.testDirectory = Path.Combine(Path.GetTempPath(), $"OptimizingWandererRoutes_{Guid.NewGuid():N}");
        Directory.CreateDirectory(this.testDirectory);
        this.originalOutput = Console.Out;
        this.originalInput = Console.In;
        Console.SetOut(this.capturedOutput);
        Console.SetIn(new StringReader(string.Empty));
    }

    /// <summary>
    /// Gives the console back to the test host and removes the directory of the finished test.
    /// </summary>
    [TestCleanup]
    public void RestoreTheConsole()
    {
        Console.SetOut(this.originalOutput);
        Console.SetIn(this.originalInput);
        this.capturedOutput.Dispose();

        if (Directory.Exists(this.testDirectory))
        {
            Directory.Delete(this.testDirectory, true);
        }
    }

    /// <summary>
    /// Checks whether the example of the task is split exactly like the PDF shows it. This is the one input with
    /// a result that is known to be correct, so it is the test that must never be adjusted to a new behaviour.
    /// </summary>
    [TestMethod]
    public void TheExampleOfTheTaskIsSplitLikeInThePdf()
    {
        this.Run(
            TestDataProvider.GetTaskExampleStages().Length,
            TestDataProvider.TaskExampleNumberOfDays,
            TestDataProvider.GetTaskExampleStages());

        CollectionAssert.AreEqual(
            new[] { "1.Tag: 11 km", "2.Tag: 26 km", "3.Tag: 22 km" },
            this.GetPrintedDays());
        StringAssert.Contains(this.capturedOutput.ToString(), "Maximum: 26 km");
    }

    /// <summary>
    /// Checks whether an input whose stage count is not a multiple of the day count stays within the number of
    /// days. Up to version 1.0.7.0 this printed a third day.
    /// </summary>
    [TestMethod]
    public void FiveStagesOverTwoDaysStayTwoDays()
    {
        this.Run(5, 2, 1, 2, 3, 4, 5);

        CollectionAssert.AreEqual(new[] { "1.Tag: 6 km", "2.Tag: 9 km" }, this.GetPrintedDays());
    }

    /// <summary>
    /// Checks the same rule for a second uneven split. Up to version 1.0.7.0 this printed a fourth day.
    /// </summary>
    [TestMethod]
    public void SevenStagesOverThreeDaysStayThreeDays()
    {
        this.Run(7, 3, 1, 2, 3, 4, 5, 6, 7);

        CollectionAssert.AreEqual(new[] { "1.Tag: 6 km", "2.Tag: 9 km", "3.Tag: 13 km" }, this.GetPrintedDays());
    }

    /// <summary>
    /// Checks whether a single day gets every stage without an optimization run in between.
    /// </summary>
    [TestMethod]
    public void ASingleDayGetsEveryStage()
    {
        this.Run(4, 1, 3, 4, 5, 6);

        CollectionAssert.AreEqual(new[] { "1.Tag: 18 km" }, this.GetPrintedDays());
    }

    /// <summary>
    /// Checks whether the stages beyond the announced number are dropped. The file in.txt of the program holds
    /// seven stages for six announced ones, the last one has to stay out of the result.
    /// </summary>
    [TestMethod]
    public void StagesBeyondTheAnnouncedNumberAreIgnored()
    {
        this.Run(6, 3, 11, 5, 3, 7, 8, 12, 20);

        CollectionAssert.AreEqual(
            new[] { "1.Tag: 11 km", "2.Tag: 15 km", "3.Tag: 20 km" },
            this.GetPrintedDays());
    }

    /// <summary>
    /// Checks whether reading a file that does not exist is reported.
    /// </summary>
    [TestMethod]
    public void ReadFileThrowsIfTheFileDoesNotExist()
    {
        IOptimizer optimizer = new Optimizer();

        Assert.ThrowsExactly<FileNotFoundException>(
            () => optimizer.ReadFile(Path.Combine(this.testDirectory, "MissingFile.txt")));
    }

    /// <summary>
    /// Checks whether a file holding fewer stages than it announces is reported.
    /// </summary>
    [TestMethod]
    public void ReadFileThrowsIfTheFileHoldsFewerStagesThanAnnounced()
    {
        var fileName = TestDataProvider.WriteInputFile(this.testDirectory, 6, 3, 11, 16, 5, 5);
        IOptimizer optimizer = new Optimizer();

        Assert.ThrowsExactly<TooLessStagesException>(() => optimizer.ReadFile(fileName));
    }

    /// <summary>
    /// Checks whether optimizing without a file read before is reported.
    /// </summary>
    [TestMethod]
    public void OptimizeThrowsIfTheFileWasNotRead()
    {
        IOptimizer optimizer = new Optimizer();

        Assert.ThrowsExactly<FileNotReadException>(optimizer.Optimize);
    }

    /// <summary>
    /// Checks whether printing without a file read before is reported.
    /// </summary>
    [TestMethod]
    public void PrintResultsThrowsIfTheFileWasNotRead()
    {
        IOptimizer optimizer = new Optimizer();

        Assert.ThrowsExactly<FileNotReadException>(optimizer.PrintResults);
    }

    /// <summary>
    /// Checks whether printing without an optimization run before is reported.
    /// </summary>
    [TestMethod]
    public void PrintResultsThrowsIfOptimizeWasNotCalled()
    {
        var fileName = TestDataProvider.WriteInputFile(
            this.testDirectory,
            TestDataProvider.GetTaskExampleStages().Length,
            TestDataProvider.TaskExampleNumberOfDays,
            TestDataProvider.GetTaskExampleStages());
        IOptimizer optimizer = new Optimizer();
        optimizer.ReadFile(fileName);

        Assert.ThrowsExactly<OptimizeNotCalledException>(optimizer.PrintResults);
    }

    /// <summary>
    /// Writes an input file and runs the whole program flow against it.
    /// </summary>
    /// <param name="numberOfStages">The number of stages written into the first line.</param>
    /// <param name="numberOfDays">The number of days written into the second line.</param>
    /// <param name="stages">The stages written into the remaining lines.</param>
    private void Run(int numberOfStages, int numberOfDays, params int[] stages)
    {
        var fileName = TestDataProvider.WriteInputFile(this.testDirectory, numberOfStages, numberOfDays, stages);
        IOptimizer optimizer = new Optimizer();
        optimizer.ReadFile(fileName);
        optimizer.Optimize();
        optimizer.PrintResults();
    }

    /// <summary>
    /// Gets the printed day lines of the captured console output, without the maximum and the closing message.
    /// </summary>
    /// <returns>The printed day lines.</returns>
    private string[] GetPrintedDays()
    {
        return this.capturedOutput.ToString()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.Contains(".Tag:", StringComparison.Ordinal))
            .ToArray();
    }
}

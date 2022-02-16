// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Optimizer.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   Class to optimize the wanderers route according to their preferences.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes;

/// <inheritdoc cref="IOptimizer"/>
/// <summary>
///     Class to optimize the wanderers route according to their preferences.
/// </summary>
/// <seealso cref="IOptimizer"/>
public class Optimizer : IOptimizer
{
    /// <summary>
    /// The buckets.
    /// </summary>
    private readonly List<Bucket> buckets = new();

    /// <summary>
    /// A value indicating whether the change is done or not.
    /// </summary>
    private bool changeDone = true;

    /// <summary>
    /// The number of days.
    /// </summary>
    private int numberOfDays;

    /// <summary>
    /// The number of stages.
    /// </summary>
    private int numberOfStages;

    /// <summary>
    /// A value indicating whether the optimize method was called or not.
    /// </summary>
    private bool optimizeCalled;

    /// <summary>
    /// A value indicating whether the read file method was called or not.
    /// </summary>
    private bool readFileCalled;

    /// <summary>
    /// The stages,
    /// </summary>
    private List<int> stages = new();

    /// <inheritdoc cref="IOptimizer"/>
    /// <summary>
    ///     Optimizes the wanderer's routes. Throws an <see cref="Exception" /> of type <see cref="FileNotReadException" />
    ///     if the file was not read yet.
    /// </summary>
    /// <seealso cref="IOptimizer"/>
    public void Optimize()
    {
        if (!this.readFileCalled)
        {
            throw new FileNotReadException("The file was not read yet");
        }

        this.FillBuckets();
        this.OptimizeRoutes();
        this.optimizeCalled = true;
    }

    /// <inheritdoc cref="IOptimizer"/>
    /// <summary>
    ///     Prints out the results. Throws an <see cref="Exception" /> of type <see cref="FileNotReadException" /> if the
    ///     file was not read yet. Throws an <see cref="Exception" /> of type <see cref="OptimizeNotCalledException" /> if
    ///     the optimize function was not called yet.
    /// </summary>
    /// <seealso cref="IOptimizer"/>
    public void PrintResults()
    {
        // Check if the prior needed methods are called yet
        if (!this.readFileCalled)
        {
            throw new FileNotReadException("The file was not read yet");
        }

        if (!this.optimizeCalled)
        {
            throw new OptimizeNotCalledException("The optimize method was not called yet");
        }

        // Iterate over the buckets, print the sum out and get the maximum sum of the buckets
        var maximum = 0;
        for (var i = 0; i < this.buckets.Count; i++)
        {
            var currentBucket = this.buckets[i];
            Console.WriteLine(i + 1 + ".Tag: " + currentBucket.Sum() + " km");

            if (maximum < currentBucket.Sum())
            {
                maximum = currentBucket.Sum();
            }
        }

        Console.WriteLine("Maximum: " + maximum + " km");
        Console.WriteLine("Press any key to stop the program...");
        Console.ReadLine();
    }

    /// <inheritdoc cref="IOptimizer"/>
    /// <summary>
    ///     Reads an input file. Throws an <see cref="Exception" /> of type if <see cref="FileNotFoundException" /> if the
    ///     given file name was not found. Throws am <see cref="Exception"/> of type <see cref="TooLessStagesException"/> if
    ///     the number of stages exceeds the available stages in the file.
    /// </summary>
    /// <param name="fileName">The input file to be read.</param>
    /// <seealso cref="IOptimizer"/>
    public void ReadFile(string fileName)
    {
        // Check if the file exists
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException("The file does not exists");
        }

        var counter = 0;

        // Read all lines from the file
        var lines = File.ReadAllLines(fileName);
        foreach (var line in lines)
        {
            switch (counter)
            {
                case 0:
                    // Read number of stages from the first line in the file
                    this.numberOfStages = Convert.ToInt32(line);
                    break;
                case 1:
                    // Read number of days from the first line in the file
                    this.numberOfDays = Convert.ToInt32(line);
                    break;
                default:
                    // Read the different stages from the rest of the file
                    this.stages.Add(Convert.ToInt32(line));
                    break;
            }

            counter++;
        }

        // Check if the file is valid: Throw error if the number of stages exceeds the available stages
        if (this.numberOfStages > this.stages.Count)
        {
            throw new TooLessStagesException("The stages provided are less than the limit provided");
        }

        // Remove all stages that are more than the wished ones from "numberOfStages"
        this.stages = this.stages.Take(this.numberOfStages).ToList();
        this.readFileCalled = true;
    }

    /// <summary>
    /// Optimizes the routes.
    /// </summary>
    private void OptimizeRoutes()
    {
        // While a change in the buckets is done
        while (this.changeDone)
        {
            // Return if we only have one bucket, no optimization needed
            if (this.buckets.Count <= 1)
            {
                return;
            }

            // Save if there are changes done in the current run
            var localChangesDone = new List<bool>();

            // Run over all buckets
            for (var i = 0; i < this.buckets.Count - 1; i++)
            {
                // Get the left and right bucket to compare them
                var leftBucket = this.buckets[i];
                var rightBucket = this.buckets[i + 1];

                // If the left bucket + the next element in sum is smaller than the sum of the right bucket
                if (leftBucket.Sum() + rightBucket.GetLeftMostElement() < rightBucket.Sum())
                {
                    // Remove item from the right bucket
                    var removedItem = rightBucket.RemoveLeftMostElement();

                    // If there is no item in the bucket or the bucket has only one item left, jump out
                    if (removedItem == -1)
                    {
                        continue;
                    }

                    // Add item to the left bucket
                    leftBucket.AddRightMostElement(removedItem);

                    // Tell the program that there where local changes
                    localChangesDone.Add(true);
                }
                else if (rightBucket.Sum() + leftBucket.GetRightMostElement() < leftBucket.Sum())
                {
                    // If the right bucket + the prior element in sum is smaller than the sum of the left bucket
                    // Remove item from the left bucket
                    var removedItem = leftBucket.RemoveRightMostElement();

                    // If there is no item in the bucket or the bucket has only one item left, jump out
                    if (removedItem == -1)
                    {
                        continue;
                    }

                    // Add item to the right bucket
                    rightBucket.AddLeftMostElement(removedItem);

                    // Tell the program that there where local changes
                    localChangesDone.Add(true);
                }
                else
                {
                    // Tell the program that there where no local changes
                    localChangesDone.Add(false);
                }
            }

            // Set the global change parameter if there where no local changes
            this.changeDone = localChangesDone.Any(x => x);
        }
    }

    /// <summary>
    /// Fills the buckets.
    /// </summary>
    private void FillBuckets()
    {
        // Calculate how many elements are in a bucket (The last one of course may have less than the others)
        var elementsPerBucket = (double)this.stages.Count / this.numberOfDays;
        int elementsPerBucketInt;

        // If the variable ends to .5: Subtract 1 and round up
        if (elementsPerBucket.ToString(CultureInfo.InvariantCulture).EndsWith(".5"))
        {
            elementsPerBucketInt = (int)Math.Ceiling(elementsPerBucket - 1);
        }
        else
        {
            // Else round down
            elementsPerBucketInt = (int)elementsPerBucket;
        }

        var counter = 0;
        foreach (var stage in this.stages)
        {
            // Special case if the elements per bucket is 1 and it's the last possible bucket: Only add one stage and reset counter
            if (elementsPerBucketInt == 1 && this.buckets.Count == this.numberOfDays)
            {
                // Add the stage as rightmost element to the last bucket
                this.buckets[^1].AddRightMostElement(stage);

                // Reset counter
                counter = 0;
            }
            else if (elementsPerBucketInt == 1)
            {
                // Special case if the elements per bucket is 1: Add new bucket and only add one stage and reset counter
                // Create a new bucket
                this.buckets.Add(new Bucket());

                // Add the stage as rightmost element
                this.buckets[^1].AddRightMostElement(stage);

                // Reset counter
                counter = 0;
            }
            else if (counter == 0)
            {
                // First stage of each bucket: Create a new bucket and add the stage to this bucket as
                // rightmost element (Buckets are filled from left to right) and increment the counter
                // Create a new bucket
                this.buckets.Add(new Bucket());

                // Add the stage as rightmost element
                this.buckets[^1].AddRightMostElement(stage);

                // Increment counter
                counter++;
            }
            else if (counter == elementsPerBucketInt - 1)
            {
                // Last stage of each bucket: Add the stage as rightmost element (Buckets are filled from left to right)
                // and reset the counter
                // Add the stage as rightmost element
                this.buckets[^1].AddRightMostElement(stage);

                // Reset counter
                counter = 0;
            }
            else
            {
                // Any other stage: add the stage to this bucket as rightmost element (Buckets are filled from left to right)
                // and increment the counter
                // Add the stage as rightmost element
                this.buckets[^1].AddRightMostElement(stage);

                // Increment counter
                counter++;
            }
        }
    }
}

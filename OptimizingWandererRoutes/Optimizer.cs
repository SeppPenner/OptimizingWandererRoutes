using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OptimizingWandererRoutes.Exceptions;

namespace OptimizingWandererRoutes
{
    /// <inheritdoc />
    public class Optimizer : IOptimizer
    {
        private readonly List<Bucket> _buckets = new List<Bucket>();
        private bool _changeDone = true;
        private int _numberOfDays;
        private int _numberOfStages;
        private bool _optimizeCalled;
        private bool _readFileCalled;
        private List<int> _stages = new List<int>();

        /// <inheritdoc />
        public void Optimize()
        {
            if (!_readFileCalled)
                throw new FileNotReadException("The file was not read yet");
            FillBuckets();
            OptimizeRoutes();
            _optimizeCalled = true;
        }

        /// <inheritdoc />
        public void PrintResults()
        {
            //Check if the prior needed methods are called yet
            if (!_readFileCalled)
                throw new FileNotReadException("The file was not read yet");
            if (!_optimizeCalled)
                throw new OptimizeNotCalledException("The optimize method was not called yet");
            //Iterate over the buckets, print the sum out and get the maximum sum of the buckets
            var maximum = 0;
            for (var i = 0; i < _buckets.Count; i++)
            {
                var currentBucket = _buckets[i];
                Console.WriteLine(i + 1 + ".Tag: " + currentBucket.Sum() + " km");
                if (maximum < currentBucket.Sum())
                    maximum = currentBucket.Sum();
            }
            Console.WriteLine("Maximum: " + maximum + " km");
            Console.WriteLine("Press any key to stop the programm...");
            Console.ReadLine();
        }

        /// <inheritdoc />
        public void ReadFile(string fileName)
        {
            //Check if the file exists
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The file does not exists");
            var counter = 0;
            //Read all lines from the file
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                switch (counter)
                {
                    case 0:
                        //Read number of stages from the first line in the file
                        _numberOfStages = Convert.ToInt32(line);
                        break;
                    case 1:
                        //Read number of days from the first line in the file
                        _numberOfDays = Convert.ToInt32(line);
                        break;
                    default:
                        //Read the different stages from the rest of the file
                        _stages.Add(Convert.ToInt32(line));
                        break;
                }
                counter++;
            }
            //Check if the file is valid: Throw error if the number of stages exceeds the available stages
            if (_numberOfStages > _stages.Count)
            {
                throw new TooLessStagesException("The stages provided are less than the limit provided");
            }
            //Remove all stages that are more than the wished ones from "_numberOfStages"
            _stages = _stages.Take(_numberOfStages).ToList();
            _readFileCalled = true;
        }

        private void OptimizeRoutes()
        {
            //While a change in the buckets is done
            while (_changeDone)
            {
                //Return if we only have one bucket, no optimization needed
                if (_buckets.Count <= 1)
                    return;
                //Save if there are changes done in the current run
                var localChangesDone = new List<bool>();
                //Run over all buckets
                for (var i = 0; i < _buckets.Count - 1; i++)
                {
                    //Get the left and right bucket to compare them
                    var leftBucket = _buckets[i];
                    var rightBucket = _buckets[i + 1];
                    //If the left bucket + the next element in sum is smaller than the sum of the right bucket
                    if (leftBucket.Sum() + rightBucket.GetLeftMostElement() < rightBucket.Sum())
                    {
                        //Remove item from the right bucket
                        var removedItem = rightBucket.RemoveLeftMostElement();
                        //If there is no item in the bucket or the bucket has only one item left, jump out
                        if (removedItem == -1)
                            continue;
                        //Add item to the left bucket
                        leftBucket.AddRightMostElement(removedItem);
                        //Tell the programm that there where local changes
                        localChangesDone.Add(true);
                    }
                    //If the right bucket + the prior element in sum is smaller than the sum of the left bucket
                    else if (rightBucket.Sum() + leftBucket.GetRightMostElement() < leftBucket.Sum())
                    {
                        //Remove item from the left bucket
                        var removedItem = leftBucket.RemoveRightMostElement();
                        //If there is no item in the bucket or the bucket has only one item left, jump out
                        if (removedItem == -1)
                            continue;
                        //Add item to the right bucket
                        rightBucket.AddLeftMostElement(removedItem);
                        //Tell the programm that there where local changes
                        localChangesDone.Add(true);
                    }
                    else
                    {
                        //Tell the programm that there where no local changes
                        localChangesDone.Add(false);
                    }
                }
                //Set the global change parameter if there where no local changes
                _changeDone = localChangesDone.Any(x => x);
            }
        }

        private void FillBuckets()
        {
            //Calculate how many elements are in a bucket (The last one of course may have less than the others)
            var elementsPerBucket = (double)_stages.Count / _numberOfDays;
            int elementsPerBucketInt;
            //If the variable ends to .5: Substract 1 and round up
            if (elementsPerBucket.ToString(CultureInfo.InvariantCulture).EndsWith(".5"))
            {
                elementsPerBucketInt = (int) Math.Ceiling(elementsPerBucket -1);
            }
            //Else round down
            else
            {
                elementsPerBucketInt = (int) elementsPerBucket;
            }
            var counter = 0;
            foreach (var stage in _stages)
            {
                //Special case if the elements per bucket is 1 and it's the last possible bucket: Only add one stage and reset counter
                if (elementsPerBucketInt == 1 && _buckets.Count == _numberOfDays)
                {
                    //Add the stage as rightmost element to the last bucket
                    _buckets[_buckets.Count - 1].AddRightMostElement(stage);
                    //Reset counter
                    counter = 0;
                }
                //Special case if the elements per bucket is 1: Add new bucket and only add one stage and reset counter
                else if (elementsPerBucketInt == 1)
                {
                    //Create a new bucket
                    _buckets.Add(new Bucket());
                    //Add the stage as rightmost element
                    _buckets[_buckets.Count - 1].AddRightMostElement(stage);
                    //Reset counter
                    counter = 0;
                }
                //First stage of each bucket: Create a new bucket and add the stage to this bucket as
                //rightmost element (Buckets are filled from left to right) and increment the counter
                else if (counter == 0)
                {
                    //Create a new bucket
                    _buckets.Add(new Bucket());
                    //Add the stage as rightmost element
                    _buckets[_buckets.Count - 1].AddRightMostElement(stage);
                    //Increment counter
                    counter++;
                }
                //Last stage of each bucket: Add the stage as rightmost element (Buckets are filled from left to right)
                //and reset the counter
                else if (counter == elementsPerBucketInt - 1)
                {
                    //Add the stage as rightmost element
                    _buckets[_buckets.Count - 1].AddRightMostElement(stage);
                    //Reset counter
                    counter = 0;
                }
                //Any other stage: add the stage to this bucket as rightmost element (Buckets are filled from left to right)
                //and increment the counter
                else
                {
                    //Add the stage as rightmost element
                    _buckets[_buckets.Count - 1].AddRightMostElement(stage);
                    //Increment counter
                    counter++;
                }
            }
        }
    }
}
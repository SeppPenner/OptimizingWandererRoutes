// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BucketTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="Bucket" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes.Tests;

/// <summary>
/// A class to test the <see cref="Bucket"/> class.
/// </summary>
[TestClass]
public class BucketTests
{
    /// <summary>
    /// Checks whether an empty bucket answers with the -1 sentinel and sums up to zero.
    /// </summary>
    [TestMethod]
    public void AnEmptyBucketReturnsMinusOneAndSumsToZero()
    {
        var bucket = new Bucket();

        Assert.AreEqual(0, bucket.Size());
        Assert.AreEqual(0, bucket.Sum());
        Assert.AreEqual(-1, bucket.GetLeftMostElement());
        Assert.AreEqual(-1, bucket.GetRightMostElement());
    }

    /// <summary>
    /// Checks whether the stages keep the order they were added in, from both sides.
    /// </summary>
    [TestMethod]
    public void TheStagesKeepTheirOrder()
    {
        var bucket = new Bucket();
        bucket.AddRightMostElement(16);
        bucket.AddRightMostElement(5);
        bucket.AddLeftMostElement(11);

        Assert.AreEqual("[11,16,5]", bucket.ToString());
        Assert.AreEqual(3, bucket.Size());
        Assert.AreEqual(32, bucket.Sum());
        Assert.AreEqual(11, bucket.GetLeftMostElement());
        Assert.AreEqual(5, bucket.GetRightMostElement());
    }

    /// <summary>
    /// Checks whether removing returns the outer stages and takes them out of the bucket.
    /// </summary>
    [TestMethod]
    public void RemovingReturnsTheOuterStages()
    {
        var bucket = new Bucket();
        bucket.AddRightMostElement(11);
        bucket.AddRightMostElement(16);
        bucket.AddRightMostElement(5);

        Assert.AreEqual(11, bucket.RemoveLeftMostElement());
        Assert.AreEqual(5, bucket.RemoveRightMostElement());
        Assert.AreEqual("[16]", bucket.ToString());
    }

    /// <summary>
    /// Checks whether the last stage of a day stays where it is. A day without a stage would be a day the
    /// wanderer does not walk at all, so the removal is refused with the -1 sentinel.
    /// </summary>
    [TestMethod]
    public void TheLastStageOfADayIsNeverRemoved()
    {
        var bucket = new Bucket();
        bucket.AddRightMostElement(11);

        Assert.AreEqual(-1, bucket.RemoveLeftMostElement());
        Assert.AreEqual(-1, bucket.RemoveRightMostElement());
        Assert.AreEqual(1, bucket.Size());
        Assert.AreEqual(11, bucket.GetLeftMostElement());
    }
}

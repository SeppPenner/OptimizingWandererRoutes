// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bucket.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The bucket class to split the initial <see cref="List{T}" /> into different buckets.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OptimizingWandererRoutes;

/// <summary>
/// The bucket class to split the initial <see cref="List{T}"/> into different buckets.
/// </summary>
public class Bucket
{
    /// <summary>
    /// The internal elements of the bucket. Represents a list of stages in kilometers for the wanderer's route.
    /// </summary>
    private readonly List<int> elements = new();

    /// <summary>
    /// Gets the leftmost element of the bucket.
    /// </summary>
    /// <returns>Returns the leftmost element of the bucket or -1 if the size of the bucket is 0.</returns>
    public int GetLeftMostElement()
    {
        // First element can't be found if the bucket is empty.
        if (this.Size() == 0)
        {
            return -1;
        }

        return this.elements.First();
    }

    /// <summary>
    /// Gets the rightmost element of the bucket.
    /// </summary>
    /// <returns>Returns the rightmost element of the bucket or -1 if the size of the bucket is 0.</returns>
    public int GetRightMostElement()
    {
        // Last element can't be found if the bucket is empty.
        if (this.Size() == 0)
        {
            return -1;
        }

        return this.elements.Last();
    }

    /// <summary>
    /// Gets the size of the bucket.
    /// </summary>
    /// <returns>Returns the size of the bucket.</returns>
    public int Size()
    {
        return this.elements.Count;
    }

    /// <summary>
    /// Gets the sum of the bucket's elements' values.
    /// </summary>
    /// <returns>Returns the sum of the bucket's elements' values.</returns>
    public int Sum()
    {
        return this.elements.Sum();
    }

    /// <summary>
    /// Removes the leftmost element of the bucket.
    /// </summary>
    /// <returns>Returns the leftmost element of the bucket or -1 if the bucket has only one element left.</returns>
    public int RemoveLeftMostElement()
    {
        // Cannot remove the rightmost item if the bucket is empty.
        if (this.Size() <= 1)
        {
            return -1;
        }

        var element = this.elements[0];
        this.elements.RemoveAt(0);
        return element;
    }

    /// <summary>
    /// Removes the rightmost element of the bucket.
    /// </summary>
    /// <returns>Returns the rightmost element of the bucket or -1 if the bucket has only one element left.</returns>
    public int RemoveRightMostElement()
    {
        // Cannot remove the rightmost item if the bucket is empty.
        if (this.Size() <= 1)
        {
            return -1;
        }

        var element = this.elements[this.Size() - 1];
        this.elements.RemoveAt(this.Size() - 1);
        return element;
    }

    /// <summary>
    /// Adds an element to the bucket as leftmost element.
    /// </summary>
    /// <param name="element">The element to add.</param>
    public void AddLeftMostElement(int element)
    {
        this.elements.Insert(0, element);
    }

    /// <summary>
    /// Adds an element to the bucket as rightmost element.
    /// </summary>
    /// <param name="element">The element to add.</param>
    public void AddRightMostElement(int element)
    {
        this.elements.Add(element);
    }

    /// <summary>
    /// Overrides the custom ToString() method and returns a nice string representation of the bucket.
    /// </summary>
    /// <returns>Returns a nice string representation of the bucket.</returns>
    public override string ToString()
    {
        return "[" + string.Join(",", this.elements.Select(n => n.ToString())) + "]";
    }
}

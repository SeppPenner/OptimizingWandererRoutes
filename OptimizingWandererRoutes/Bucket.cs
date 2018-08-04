using System.Collections.Generic;
using System.Linq;

namespace OptimizingWandererRoutes
{
    /// <summary>
    /// The bucket class to split the initial <see cref="List{T}"/> into different buckets.
    /// </summary>
    public class Bucket
    {
        /// <summary>
        /// The internal elements of the bucket. Represents a list of stages in kilometers for the wanderer's route.
        /// </summary>
        private readonly List<int> _elements = new List<int>();

        /// <summary>
        /// Gets the leftmost element of the bucket.
        /// </summary>
        /// <returns>Returns the leftmost element of the bucket or -1 if the size of the bucket is 0.</returns>
        public int GetLeftMostElement()
        {
            //First element can't be found if the bucket is empty.
            if (Size() == 0)
            {
                return -1;
            }
            return _elements.First();
        }

        /// <summary>
        /// Gets the rightmost element of the bucket.
        /// </summary>
        /// <returns>Returns the rightmost element of the bucket or -1 if the size of the bucket is 0.</returns>
        public int GetRightMostElement()
        {
            //Last element can't be found if the bucket is empty.
            if (Size() == 0)
            {
                return -1;
            }
            return _elements.Last();
        }

        /// <summary>
        /// Gets the size of the bucket.
        /// </summary>
        /// <returns>Returns the size of the bucket.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public int Size()
        {
            return _elements.Count;
        }

        /// <summary>
        /// Gets the sum of the bucket's elements' values.
        /// </summary>
        /// <returns>Returns the sum of the bucket's elements' values.</returns>
        public int Sum()
        {
            return _elements.Sum();
        }

        /// <summary>
        /// Removes the leftmost element of the bucket.
        /// </summary>
        /// <returns>Returns the leftmost element of the bucket or -1 if the bucket has only one element left.</returns>
        public int RemoveLeftMostElement()
        {
            //Cannot remove the rightmost item if the bucket is empty.
            if (Size() <= 1)
            {
                return -1;
            }
            var element = _elements[0];
            _elements.RemoveAt(0);
            return element;
        }

        /// <summary>
        /// Removes the rightmost element of the bucket.
        /// </summary>
        /// <returns>Returns the rightmost element of the bucket or -1 if the bucket has only one element left.</returns>
        public int RemoveRightMostElement()
        {
            //Cannot remove the rightmost item if the bucket is empty.
            if (Size() <= 1)
            {
                return -1;
            }
            var element = _elements[Size() - 1];
            _elements.RemoveAt(Size() - 1);
            return element;
        }

        /// <summary>
        /// Adds an element to the bucket as leftmost element.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void AddLeftMostElement(int element)
        {
            _elements.Insert(0, element);
        }

        /// <summary>
        /// Adds an element to the bucket as rightmost element.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void AddRightMostElement(int element)
        {
            _elements.Add(element);
        }

        /// <summary>
        /// Overrides the custom ToString() method and returns a nice string representation of the bucket.
        /// </summary>
        /// <returns>Returns a nice string representation of the bucket.</returns>
        public override string ToString()
        {
            return "[" + string.Join(",", _elements.Select(n => n.ToString())) + "]";
        }
    }
}

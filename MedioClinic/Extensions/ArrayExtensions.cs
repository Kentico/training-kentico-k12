using System;

namespace MedioClinic.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gets a contiguous portion of an array and copies it into a new array.
        /// </summary>
        /// <typeparam name="T">Type of the array items.</typeparam>
        /// <param name="array">The array to operate upon.</param>
        /// <param name="start">The starting position.</param>
        /// <param name="length">The length of the new array.</param>
        /// <returns></returns>
        public static T[] GetArrayRange<T>(this T[] array, int start, int length = 0)
        {
            int end = length == 0 ? array.Length - start : length;
            T[] copy = new T[end];
            Array.Copy(array, start, copy, 0, end);

            return copy;
        }
    }
}
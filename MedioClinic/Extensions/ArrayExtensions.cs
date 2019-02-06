using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedioClinic.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] GetArrayRange<T>(this T[] array, int start, int length = 0)
        {
            int end = length == 0 ? array.Length - start : length;
            T[] copy = new T[end];
            Array.Copy(array, start, copy, 0, end);

            return copy;
        }
    }
}
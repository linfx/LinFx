using System;
using System.Collections.Generic;
using System.Linq;

namespace LinFx.Utils
{
	public static class RandomUtils
	{
		private static readonly Random rnd = new Random();
		private readonly static object obj = new object();

		/// <summary>
		/// Returns a random number within a specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to minValue and less than maxValue; 
		/// that is, the range of return values includes minValue but not maxValue. 
		/// If minValue equals maxValue, minValue is returned.
		/// </returns>
		public static int GetRandom(int minValue, int maxValue)
		{
			lock (rnd)
			{
				return rnd.Next(minValue, maxValue);
			}
		}
		/// <summary>
		/// Returns a nonnegative random number less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to zero, and less than maxValue; 
		/// that is, the range of return values ordinarily includes zero but not maxValue. 
		/// However, if maxValue equals zero, maxValue is returned.
		/// </returns>
		public static int GetRandom(int maxValue)
		{
			lock (rnd)
			{
				return rnd.Next(maxValue);
			}
		}
		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.</returns>
		public static int GetRandom()
		{
			lock (rnd)
			{
				return rnd.Next();
			}
		}
		/// <summary>
		/// Gets random of given objects.
		/// </summary>
		/// <typeparam name="T">Type of the objects</typeparam>
		/// <param name="objs">List of object to select a random one</param>
		public static T GetRandomOf<T>(params T[] objs)
		{
			if (objs.IsNullOrEmpty())
				throw new ArgumentException("objs can not be null or empty!", "objs");

			return objs[GetRandom(0, objs.Length)];
		}
		/// <summary>
		/// Generates a randomized list from given enumerable.
		/// </summary>
		/// <typeparam name="T">Type of items in the list</typeparam>
		/// <param name="items">items</param>
		public static IList<T> GenerateRandomizedList<T>(IEnumerable<T> items)
		{
			var currentList = new List<T>(items);
			var randomList = new List<T>();

			while (currentList.Any())
			{
				var randomIndex = GetRandom(0, currentList.Count);
				randomList.Add(currentList[randomIndex]);
				currentList.RemoveAt(randomIndex);
			}

			return randomList;
		}

		/// <summary>
		/// 生成长度{length}随机数字组合
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GenerateRandomNumber(int length = 6)
		{
			var code = string.Empty;
			if (length <= 0)
				return code;

			var start = Convert.ToInt32(Math.Pow(10, length - 1));
			var end = Convert.ToInt32(Math.Pow(10, length));
			lock (obj)
			{
				code = new Random().Next(start, end).ToString();
			}
			return code;
		}
	}
}

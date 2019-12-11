using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Funcular.DomainTools.Utilities
{
	public static partial class EnumerableExtensions
	{
		/// <summary>
		///     Returns the maximal element of the given sequence, based on
		///     the given projection.
		/// </summary>
		/// <remarks>
		///     If more than one element has the maximal projected value, the first
		///     one encountered will be returned. This overload uses the default comparer
		///     for the projected type. This operator uses immediate execution, but
		///     only buffers a single result (the current maximal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="values">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <returns>The maximal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="values" /> or <paramref name="selector" /> is null
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="values" /> is empty
		/// </exception>
		public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> values, Func<TSource, TKey> selector)
		{
			return values.MaxBy(selector, Comparer<TKey>.Default);
		}

		/// <summary>
		///     Returns the maximal element of the given sequence, based on
		///     the given projection and the specified comparer for projected values.
		/// </summary>
		/// <remarks>
		///     If more than one element has the maximal projected value, the first
		///     one encountered will be returned. This overload uses the default comparer
		///     for the projected type. This operator uses immediate execution, but
		///     only buffers a single result (the current maximal element).
		/// </remarks>
		/// <typeparam name="TSource">Type of the source sequence</typeparam>
		/// <typeparam name="TKey">Type of the projected element</typeparam>
		/// <param name="source">Source sequence</param>
		/// <param name="selector">Selector to use to pick the results to compare</param>
		/// <param name="comparer">Comparer to use to compare projected values</param>
		/// <returns>The maximal element, according to the projection.</returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="source" />, <paramref name="selector" />
		///     or <paramref name="comparer" /> is null
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="source" /> is empty
		/// </exception>
		public static TSource MaxBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (selector == null) throw new ArgumentNullException("selector");
			if (comparer == null) throw new ArgumentNullException("comparer");
			using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
			{
				if (!sourceIterator.MoveNext())
					throw new InvalidOperationException("Sequence contains no elements");
				TSource max = sourceIterator.Current;
				TKey maxKey = selector(max);
				while (sourceIterator.MoveNext())
				{
					TSource candidate = sourceIterator.Current;
					TKey candidateProjected = selector(candidate);
					if (comparer.Compare(candidateProjected, maxKey) > 0)
					{
						max = candidate;
						maxKey = candidateProjected;
					}
				}
				return max;
			}
		}

		/// <summary>
		///     Syntactic sugar for iterating over a collection in batches. Usage is, e.g.:
		///     var productBatches = products.Batch(25);
		///     foreach(var productBatch in productBatches)
		///     {
		///     foreach(var product in productBatch)
		///     {
		///     foo(product);
		///     }
		///     }
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryable"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> queryable, int pageSize)
		{
			IEnumerable<T> results = queryable;
			// ReSharper disable PossibleMultipleEnumeration
			while (results.HasContents())
			{
				yield return results.Take(pageSize);
				results = results.Skip(pageSize);
			}
			// ReSharper restore PossibleMultipleEnumeration
		}

		/// <summary>
		///     Safely determine if the sequence is non-null AND contains elements.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		public static bool HasContents<T>(this IEnumerable<T> enumerable)
		{
			return enumerable != null && enumerable.Any();
		}

		/// <summary>
		///     Adds <paramref name="item" /> if not already present in the list according to <paramref name="duplicationTest" />.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="list"></param>
		/// <param name="item"></param>
		/// <param name="duplicationTest"></param>
		/// <returns></returns>
		public static IList<TSource> AddDistinct<TSource>(
			this IList<TSource> list, TSource item, Expression<Func<TSource, bool>> duplicationTest)
		{
			Func<TSource, bool> func = duplicationTest.Compile();
			if (!list.Any(func))
				list.Add(item);
			return list;
		}

	    public static IList<TSource> AddRange<TSource>(this IList<TSource> list, IEnumerable<TSource> items)
	    {
	        var ret = list as List<TSource> ?? list.ToList();
	        ret.AddRange(items);
	        return ret;
	    }

		public static IEnumerable<TEnum> GetFlags<TEnum>(
			this TEnum input, bool checkZero = false, bool checkFlags = true, bool checkCombinators = true) where TEnum : struct
		{
			Type enumType = typeof(TEnum);
			if (!enumType.IsEnum)
				yield break;

			ulong setBits = Convert.ToUInt64(input);
			// if no flags are set, return empty
			if (!checkZero && (0 == setBits))
				yield break;

			// if it's not a flag enum, return empty
			if (checkFlags && !input.GetType()
									.IsDefined(typeof(FlagsAttribute), false))
				yield break;

			if (checkCombinators)
			{
				// check each enum value mask if it is in input bits
				foreach (TEnum value in Enum<TEnum>.GetValues())
				{
					ulong valMask = Convert.ToUInt64(value);

					if ((setBits & valMask) == valMask)
						yield return value;
				}
			}
			else
			{
				// check each enum value mask if it is in input bits
				foreach (TEnum value in Enum<TEnum>.GetValues())
				{
					ulong valMask = Convert.ToUInt64(value);

					if ((setBits & valMask) == valMask)
						yield return value;
				}
			}
		}
	}
}

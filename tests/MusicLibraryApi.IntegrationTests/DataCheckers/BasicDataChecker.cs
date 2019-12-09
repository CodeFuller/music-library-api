using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public abstract class BasicDataChecker<TData> : IDataChecker<TData>
		where TData : class
	{
		protected abstract IEnumerable<Action<TData, TData, string>> PropertiesCheckers { get; }

		public void CheckData(TData? expected, TData? actual, string dataPath)
		{
			if (Object.ReferenceEquals(expected, null) && Object.ReferenceEquals(actual, null))
			{
				return;
			}

			if (Object.ReferenceEquals(expected, null) || Object.ReferenceEquals(actual, null))
			{
				Assert.AreEqual(expected, actual, $"Data at {dataPath} differs");
				return;
			}

			foreach (var propertyChecker in PropertiesCheckers)
			{
				propertyChecker(expected, actual, dataPath);
			}
		}

		public void CheckData(IReadOnlyCollection<TData?>? expected, IReadOnlyCollection<TData?>? actual, string dataPath)
		{
			if (Object.ReferenceEquals(expected, null) && Object.ReferenceEquals(actual, null))
			{
				return;
			}

			if (Object.ReferenceEquals(expected, null) || Object.ReferenceEquals(actual, null))
			{
				Assert.AreEqual(expected, actual, $"Data at {dataPath} differs");
				return;
			}

			Assert.AreEqual(expected.Count, actual.Count, $"Size of collections at {dataPath} differ");

			foreach (var (pair, i) in expected.Zip(actual).Select((p, i) => (p, i)))
			{
				CheckData(pair.First, pair.Second, $"{dataPath}[{i}]");
			}
		}

		protected Action<TData, TData, string> FieldChecker(Expression<Func<TData, string?>> expression, string fieldName)
		{
			var f = expression.Compile();
			return (x, y, dataPath) => Assert.AreEqual(f(x), f(y), $"Data at {AppendFieldName(dataPath, fieldName)} differs");
		}

		protected Action<TData, TData, string> FieldChecker<T>(Expression<Func<TData, T?>> expression, string fieldName)
			where T : struct
		{
			var f = expression.Compile();
			return (x, y, dataPath) => Assert.AreEqual(f(x), f(y), $"Data at {AppendFieldName(dataPath, fieldName)} differs");
		}

		protected Action<TData, TData, string> FieldChecker<T>(Expression<Func<TData, T?>> expression, IDataChecker<T> dataChecker, string fieldName)
			where T : class
		{
			var f = expression.Compile();
			return (x, y, dataPath) => dataChecker.CheckData(f(x), f(y), AppendFieldName(dataPath, fieldName));
		}

		protected Action<TData, TData, string> FieldChecker<TItem>(Expression<Func<TData, IReadOnlyCollection<TItem?>?>> expression, IDataChecker<TItem> itemsChecker, string fieldName)
			where TItem : class
		{
			var f = expression.Compile();
			return (x, y, dataPath) => itemsChecker.CheckData(f(x), f(y), AppendFieldName(dataPath, fieldName));
		}

		private string AppendFieldName(string dataPath, string fieldName)
		{
			var prefix = String.IsNullOrEmpty(dataPath) ? String.Empty : $"{dataPath}.";
			return $"{prefix}{fieldName}";
		}
	}
}

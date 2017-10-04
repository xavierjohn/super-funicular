using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuperFuncular.Matrix
{
    public class SparseColumnMatrix<T>
        where T : IEquatable<T>
    {
        IDictionary<int, IDictionary<int, T>> matrix;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public SparseColumnMatrix()
        {
            matrix = new Dictionary<int, IDictionary<int, T>>();
            Rows = 0;
            Columns = 0;
        }

        public T this[int row, int column]
        {
            get
            {
                if (matrix.TryGetValue(column, out IDictionary<int, T> rows))
                    if (rows.TryGetValue(row, out T value))
                        return value;
                return default(T);
            }

            set
            {
                if (value.Equals(default(T)))
                {
                    if (matrix.TryGetValue(column, out IDictionary<int, T> rows))
                        if (rows.ContainsKey(row))
                            rows.Remove(row);
                    return;
                }
                else
                {
                    if (matrix.TryGetValue(column, out IDictionary<int, T> rows))
                        rows[row] = value;
                    else
                        matrix[column] = new Dictionary<int, T>() {
                        {row, value }
                    };
                    if (row >= Rows) Rows = row + 1;
                    if (column >= Columns) Columns = column + 1;
                }
            }
        }
        public void CopyColumn(int source, int destination)
        {
            if (matrix.ContainsKey(source))
            {
                matrix[destination] = new Dictionary<int, T>(matrix[source]);
            }
        }

        public void SparsePrint(TextWriter tw)
        {
            tw.WriteLine("Format (row, column, value)");

            foreach (var column in matrix.Keys.OrderBy(k => k))
            {
                foreach (var row in matrix[column].OrderBy(k => k.Key))
                    tw.Write($"({row.Key}, {column}, {row.Value}) ");
                tw.WriteLine();
            }
        }
    }

    [TestClass]
    public class SparseColumnMatrixTester
    {

        [TestMethod]
        public void CanRetrieveTheValueStored()
        {
            var sm = new SparseColumnMatrix<int>();
            sm[5, 10] = 5;
            sm[1000, 2000] = 2;

            sm[5, 10].Should().Be(5);
            sm[1000, 2000].Should().Be(2);
            sm[5, 5].Should().Be(0);
            sm[1000000, 1000000].Should().Be(0);
            sm.Rows.Should().Be(1001);
            sm.Columns.Should().Be(2001);
        }

        [TestMethod]
        public void CanCopyColumn()
        {
            var sm = new SparseColumnMatrix<int>();
            sm[0, 2] = 1;
            sm[3, 2] = 3;

            sm.CopyColumn(2, 3);
            sm[0, 3].Should().Be(1);
            sm[1, 3].Should().Be(0);
            sm[3, 3].Should().Be(3);
        }
    }
}

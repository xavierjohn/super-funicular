using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using SuperFuncular.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SuperFuncular.Matrix
{
    public class SparseMatrix<T>
        where T : IEquatable<T>
    {
        IDictionary<int, IDictionary<int, T>> matrix;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public SparseMatrix()
        {
            matrix = new Dictionary<int, IDictionary<int, T>>();
            Rows = 0;
            Columns = 0;
        }

        public T this[int row, int column]
        {
            get
            {
                if (matrix.TryGetValue(row, out IDictionary<int, T> columnDict))
                    if (columnDict.TryGetValue(column, out T value))
                        return value;
                return default(T);
            }

            set
            {
                if (value.Equals(default(T)))
                {
                    if (matrix.TryGetValue(row, out IDictionary<int, T> columnDict))
                        if (columnDict.ContainsKey(column))
                            columnDict.Remove(column);
                    return;
                }
                else
                {
                    if (matrix.TryGetValue(row, out IDictionary<int, T> columnDict))
                        columnDict[column] = value;
                    else
                        matrix[row] = new Dictionary<int, T>() {
                        {column, value }
                    };
                    if (row >= Rows) Rows = row + 1;
                    if (column >= Columns) Columns = column + 1;
                }
            }
        }

        public void SparsePrint(TextWriter tw)
        {
            tw.WriteLine("Format (row, column, value)");

            foreach (var row in matrix.Keys.OrderBy(k => k))
            {
                foreach (var column in matrix[row].OrderBy(k => k.Key))
                    tw.Write($"({row},{column.Key}, {column.Value}) ");
                tw.WriteLine();
            }
        }
    }

    [Trait("Chapter", "Matrix")]
    public class SparseMatrixTester
    {
        private readonly ITestOutputHelper output;

        public SparseMatrixTester(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        void CanRetrieveTheValueStored()
        {
            var sm = new SparseMatrix<int>();
            sm[5, 10] = 5;
            sm[1000, 2000] = 2;

            sm[5, 10].Should().Be(5);
            sm[1000, 2000].Should().Be(2);
            sm[5, 5].Should().Be(0);
            sm[1000000, 1000000].Should().Be(0);
            sm.Rows.Should().Be(1001);
            sm.Columns.Should().Be(2001);
        }

        [Fact]
        void CanSparePrint()
        {
            var sm = new SparseMatrix<int>();
            sm[5, 10] = 5;
            sm[1000, 2000] = 2;

            var tw = new XUnitTextWriter(output);
            sm.SparsePrint(tw);
        }
    }
}

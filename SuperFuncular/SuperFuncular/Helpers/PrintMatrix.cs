using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace SuperFuncular.Helpers
{
    static public class PrintMatrixExtension
    {
        static public void Print<T>(this T[,] matrix, ITestOutputHelper output)
        {
            var sb = new StringBuilder();
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                sb.Clear();
                for (var column = 0; column < matrix.GetLength(1); column++)
                    sb.Append($"{matrix[row, column],3}");
                output.WriteLine(sb.ToString());
            }
        }
    }
}

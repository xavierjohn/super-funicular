using System;
using System.Collections.Generic;
using System.Text;

namespace SuperFuncular.Strings
{
    public class BruteForceSearch
    {
        public static int Search(String pattern, String text, int startIndex = 0)
        {
            if (startIndex >= text.Length) throw new ArgumentOutOfRangeException();
            int i, N = text.Length;
            int j, M = pattern.Length;
            for (i = startIndex, j = 0; i < N && j < M; i++)
            {
                if (text[i] == pattern[j]) j++;
                else { i -= j; j = 0; }
            }
            if (j == M) return i - M;
            else return -1;
        }
    }
}

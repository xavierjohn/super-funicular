using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace SuperFuncular.Strings
{
    public class Substring_Search
    {
        public class KnuthMorrisPrattSearch
        {
            int[,] deterministicFiniteStateAutomaton;
            public KnuthMorrisPrattSearch(String pattern)
            { // Build DFA from pattern.
                int columns = pattern.Length;
                int rows = 256;
                int columnIndex = 0;
                deterministicFiniteStateAutomaton = new int[rows, columns];
                deterministicFiniteStateAutomaton[pattern[0], columnIndex] = 1;
                for (int column = 1; column < columns; column++)
                {
                    for (int row = 0; row < rows; row++)
                        deterministicFiniteStateAutomaton[row, column] = deterministicFiniteStateAutomaton[row, columnIndex]; // Copy mismatch cases.
                    deterministicFiniteStateAutomaton[pattern[column], column] = column + 1; // Set match case.
                    columnIndex = deterministicFiniteStateAutomaton[pattern[column], columnIndex]; // Update restart state.
                }
            }
            public int Search(String txt)
            { // Simulate operation of DFA on txt.
                int i, j, N = txt.Length, M = deterministicFiniteStateAutomaton.GetLength(1);
                for (i = 0, j = 0; i < N && j < M; i++)
                    j = deterministicFiniteStateAutomaton[txt[i], j];
                if (j == M) return i - M; // found (hit end of pattern)
                else return -1; // not found
            }

        }

        [Theory]
        [InlineData("Hello There", "There", 6)]
        [InlineData("AABRAACADABRAACAADABRA", "AACAA", 12)]
        public void SubStringShouldExist(string text, string pattern, int expectedIndex)
        {
            var kmp = new KnuthMorrisPrattSearch(pattern);
            kmp.Search(text).Should().Be(expectedIndex);
        }

        [Theory]
        [InlineData("Hello", "There")]
        [InlineData("AABRAACADABRAACADABRA", "AACAA")]
        public void SubStringShouldNotExist(string text, string pattern)
        {
            var kmp = new KnuthMorrisPrattSearch(pattern);
            kmp.Search(text).Should().Be(-1);
        }
    }
}

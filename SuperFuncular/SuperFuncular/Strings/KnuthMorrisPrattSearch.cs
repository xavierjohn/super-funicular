using System;
using SuperFuncular.Helpers;
using SuperFuncular.Matrix;

namespace SuperFuncular.Strings
{
    public class KnuthMorrisPrattSearch
    {
        SparseColumnMatrix<int> deterministicFiniteStateAutomaton = new SparseColumnMatrix<int>();
        public KnuthMorrisPrattSearch(String pattern)
        { // Build DFA from pattern.
            int columns = pattern.Length;
            int columnIndex = 0;
            deterministicFiniteStateAutomaton[pattern[0], columnIndex] = 1;
            for (int column = 1; column < columns; column++)
            {
                deterministicFiniteStateAutomaton.CopyColumn(columnIndex, column);
                deterministicFiniteStateAutomaton[pattern[column], column] = column + 1; // Set match case.
                columnIndex = deterministicFiniteStateAutomaton[pattern[column], columnIndex]; // Update restart state.
            }
        }

        internal void SparsePrint(XUnitTextWriter tw)
        {
            deterministicFiniteStateAutomaton.SparsePrint(tw);
        }

        public int Search(String txt, int startIndex = 0)
        {
            if (startIndex >= txt.Length) throw new ArgumentOutOfRangeException();
            int i, j, N = txt.Length, M = deterministicFiniteStateAutomaton.Columns;
            for (i = startIndex, j = 0; i < N && j < M; i++)
                j = deterministicFiniteStateAutomaton[txt[i], j];
            if (j == M) return i - M; // found (hit end of pattern)
            else return -1; // not found
        }
    }
}

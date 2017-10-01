using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using SuperFuncular.Matrix;
using SuperFuncular.Helpers;

namespace SuperFuncular.Strings
{

    [Trait("Chapter", "Strings")]
    public class SubstringStringTester
    {
        private readonly ITestOutputHelper output;

        public SubstringStringTester(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("Hello There", "There", 6)]
        [InlineData("AABRAACADABRAACAADABRA", "AACAA", 12)]
        public void SubStringShouldExist(string text, string pattern, int expectedIndex)
        {
            var kmp = new KnuthMorrisPrattSearch(pattern);
            var tw = new XUnitTextWriter(output);

            kmp.SparsePrint(tw);
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

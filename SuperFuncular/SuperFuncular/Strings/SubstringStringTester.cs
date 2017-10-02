using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using SuperFuncular.Matrix;
using SuperFuncular.Helpers;
using System.Threading;

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

            BruteForceSearch.Search(pattern, text).Should().Be(expectedIndex);
        }

        [Theory]
        [InlineData("Hello", "There")]
        [InlineData("AABRAACADABRAACADABRA", "AACAA")]
        public void SubStringShouldNotExist(string text, string pattern)
        {
            var kmp = new KnuthMorrisPrattSearch(pattern);
            kmp.Search(text).Should().Be(-1);

            BruteForceSearch.Search(pattern, text).Should().Be(-1);
        }

        [Theory, MemberData("PerformanceData")]
        public void SubStingPerformaceTest(string pattern, string text)
        {
            var tw = new XUnitTextWriter(output);
            using (var timeKeeper = new TimeKeeper(tw))
            {
                var kmp = new KnuthMorrisPrattSearch(pattern);

                using (new PerformanceTimer(nameof(KnuthMorrisPrattSearch), timeKeeper))
                {
                    var currentIndex = 0;
                    while (true)
                    {
                        currentIndex = kmp.Search(text, currentIndex);
                        if (currentIndex == -1) break;
                        currentIndex++;
                    }
                }

                using (new PerformanceTimer(nameof(BruteForceSearch), timeKeeper))
                {
                    var currentIndex = 0;

                    while (true)
                    {
                        currentIndex = BruteForceSearch.Search(pattern, text, currentIndex);
                        if (currentIndex == -1) break;
                        currentIndex++;
                    }
                }

                using (new PerformanceTimer(nameof(System.String), timeKeeper))
                {
                    var currentIndex = 0;
                    while (true)
                    {
                        currentIndex = text.IndexOf(pattern, currentIndex);
                        if (currentIndex == -1) break;
                        currentIndex++;
                    }
                }
            }

        }

        public static IEnumerable<object[]> PerformanceData
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                new object[] { "ipsum",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec non consectetur ligula, rhoncus sodales libero. Etiam lectus sapien, eleifend non faucibus laoreet, scelerisque in ante. Donec nec mauris at augue fermentum lacinia vel eget eros. Praesent et blandit magna. Nulla viverra mi justo. Suspendisse potenti. Interdum et malesuada fames ac ante ipsum primis in faucibus. Pellentesque iaculis consectetur egestas. Suspendisse eu enim in augue placerat imperdiet. Integer nec iaculis orci. Ut neque tortor, accumsan non purus non, feugiat tempor risus. Etiam libero orci, dictum id aliquam vitae, interdum quis nisi." +
                "Proin ultrices ac sem ac interdum. Quisque ipsum nisl, mattis vel nisi vitae, aliquet viverra nibh. Fusce vel diam in augue condimentum viverra. Morbi ut ante egestas, gravida ipsum sed, laoreet risus. In viverra tristique mollis. Sed eget convallis odio, eget iaculis orci. Praesent porta in mi ac euismod. Nunc id ipsum id turpis mattis posuere. Integer ullamcorper eleifend dolor, ac suscipit diam pretium pretium. Sed at sem augue. Proin lobortis venenatis augue eu pretium. Morbi varius, orci sit amet aliquet vulputate, elit tellus mattis augue, sodales vulputate enim felis ut elit. Suspendisse dignissim lorem nec aliquam tempor. Donec ornare ut dui eget varius. Integer vestibulum iaculis urna nec pharetra." +
                "Fusce vitae ornare diam. Pellentesque quis rutrum libero. Duis iaculis euismod urna nec sagittis. Nam a purus lorem. Curabitur risus velit, faucibus at tristique non, feugiat id metus. Nam eget commodo ligula. Proin placerat justo eget sagittis auctor. Sed placerat tortor diam, at euismod risus egestas eu. Mauris mi quam, varius non erat quis, lacinia faucibus enim." +
                "Integer dapibus non erat et ultrices. Pellentesque sagittis egestas risus eget consectetur. Etiam euismod quis ligula ac pretium. Duis vestibulum venenatis tortor, ut consequat ligula lobortis vitae. Fusce euismod tincidunt ex, non gravida ipsum congue id. Phasellus convallis, urna quis viverra egestas, ligula sapien cursus sapien, ut pretium erat ante eu ex. Nullam pulvinar leo eu elementum eleifend. Ut id odio sit amet nibh faucibus sagittis semper ut augue. Fusce fringilla sit amet lacus vitae porttitor. Curabitur blandit nisl nisl, at efficitur lectus tristique interdum. Sed tincidunt eget lacus quis lacinia." +
                "Quisque rhoncus elementum erat quis scelerisque. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus auctor vitae dui eget convallis. Quisque sed justo nec tellus mollis consectetur. Donec quis efficitur ipsum, eget elementum dui. Maecenas efficitur dolor vel lorem accumsan sollicitudin. Praesent accumsan erat in metus aliquet, eget luctus orci rutrum. Aenean sit amet vestibulum diam, blandit blandit augue. Vivamus sodales nisl at vestibulum elementum. Nunc at hendrerit purus. Donec mattis laoreet odio, ut semper turpis fermentum ac."
                },
            };
            }
        }
    }
}

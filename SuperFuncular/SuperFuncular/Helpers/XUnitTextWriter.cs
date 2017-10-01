using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace SuperFuncular.Helpers
{
    public class XUnitTextWriter : TextWriter
    {
        private readonly ITestOutputHelper testOutputHelper;
        private StringBuilder sb;

        public XUnitTextWriter(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            sb = new StringBuilder();
        }

        public override Encoding Encoding => throw new NotImplementedException();

        public override void Write(char value)
        {
            if (value == '\r') return;
            if (value == '\n')
            {
                testOutputHelper.WriteLine(sb.ToString());
                sb.Clear();
                return;
            }
            sb.Append(value);
        }
    }
}

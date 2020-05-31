using System;
using System.Collections.Generic;
using System.Text;

namespace PPTReport
{
    public static class StringBuilderExtensions
    {
        public static void AddHeader(this StringBuilder sb, string title)
        {
            sb.AppendLine(title);
            sb.AddHR();
        }

        public static void AddHR(this StringBuilder sb)
        {
            sb.AppendLine("***");
        }
    }
}

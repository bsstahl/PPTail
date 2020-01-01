using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsMinDate(this DateTime value)
        {
            return value.Equals(DateTime.MinValue);
        }
    }
}

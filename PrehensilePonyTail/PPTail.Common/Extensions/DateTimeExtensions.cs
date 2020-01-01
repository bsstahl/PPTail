using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Extensions
{
    public static class DateTimeExtensions
    {
        public static Boolean IsMinDate(this DateTime value) => value.Equals(DateTime.MinValue);
    }
}

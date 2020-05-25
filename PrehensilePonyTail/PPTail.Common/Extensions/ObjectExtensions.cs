using System;
using System.Collections.Generic;
using System.Text;

namespace PPTail.Extensions
{
    public static class ObjectExtensions
    {
        public static Boolean IsNotNull(this Object obj)
        {
            return (obj is null) ? false : true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class IntegerExtensions
    {
        private const String _errorMessage = "Cannot determine the deviation of a list of fewer than 2 items.";

        public static double StdDev(this IEnumerable<int> values)
        {
            double ret = 0;
            Int32 count = values.Count();
            if (count < 2)
                throw new InvalidOperationException(_errorMessage);
            else
            {
                //Compute the Average
                double avg = values.Average();

                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}

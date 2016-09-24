using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Extensions
{
    public static class IntegerExtensions
    {
        public static double StdDev(this IEnumerable<int> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count < 2)
                throw new InvalidOperationException("Cannot determine the deviation of a list of fewer than 2 items.");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.UTIL.Helper
{
    public static class Parser
    {
        public static DateTime ParseDate(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime result))
            {
                return result;
            }

            throw new ArgumentException($"Invalid date string: {dateString}");
        }

        public static TimeSpan ParseTime(string timeString)
        {
            if (TimeSpan.TryParse(timeString, out TimeSpan result))
            {
                return result;
            }

            throw new ArgumentException($"Invalid time string: {timeString}");
        }
    }
}

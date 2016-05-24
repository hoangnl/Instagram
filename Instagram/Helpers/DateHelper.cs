using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instagram.Helpers
{
    public static class DateHelper
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double GetTicks(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler
{
    public static class Extensions
    {
        public static TimeSpan Milliseconds(this int deltaMs)
        {
            return TimeSpan.FromMilliseconds(deltaMs);
        }

        public static TimeSpan Seconds(this int deltaSeconds)
        {
            return TimeSpan.FromSeconds(deltaSeconds);
        }

        public static TimeSpan Minutes(this int deltaMinutes)
        {
            return TimeSpan.FromMinutes(deltaMinutes);
        }

        public static TimeSpan Hours(this int deltaHours)
        {
            return TimeSpan.FromHours(deltaHours);
        }
    }
}

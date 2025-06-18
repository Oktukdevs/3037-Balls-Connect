using System;
using System.Globalization;

namespace Runtime.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToInvariantShortDateString(this DateTime dateTime)
        {
            return dateTime.ToString("G", CultureInfo.InvariantCulture);
        }

        public static DateTime FromInvariantShortDateString(this string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, "G", CultureInfo.InvariantCulture);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToIsoString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        public static string ToDisplayString(this DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy");
        }
        public static DateTime ToBangladeshTime(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");            
            DateTime bangladeshTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), zoneInfo);
            return bangladeshTime;
        }

        public static DateTime BangladeshDateInUtc(this DateTime dateTime)
        {
            return dateTime.ToBangladeshTime().Date.FromBangladeshTimeToUtc();
        }

        public static DateTime FromBangladeshTimeToUtc(this DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-6);
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return dateTime;
        }

        public static (DateTime,DateTime) GetBangladeshLastMonthDateRangeInUtc(this DateTime dateTime)
        {
            var date = dateTime.ToBangladeshTime().Date;
            var fromDate = date.AddMonths(-1).AddDays(1 - date.Day).FromBangladeshTimeToUtc();
            var toDate = date.Date.AddDays(-date.Day).AddHours(23).AddMinutes(59).AddSeconds(59).FromBangladeshTimeToUtc();
            return (fromDate, toDate);
        }

        public static (DateTime, DateTime) GetBangladeshCurrentMonthDateRangeInUtc(this DateTime dateTime)
        {
            var date = dateTime.ToBangladeshTime().Date;
            var fromDate = date.AddDays(1 - date.Day).FromBangladeshTimeToUtc();
            var toDate = date.AddMonths(1).AddDays(-date.Day).AddHours(23).AddMinutes(59).AddSeconds(59).FromBangladeshTimeToUtc();
            return (fromDate, toDate);
        }
    }
}

using System;

namespace Chris.Personnel.Management.Common
{
    public static class TypeConvertExtensions
    {
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm:ss}";
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd}";
        }

        public static string ToDateTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime:yyyy-MM-dd HH:mm:ss}" : "";
        }

        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? $"{dateTime:yyyy-MM-dd}" : "";
        }

        public static string ToUpperString(this Guid id)
        {
            return id.ToString().ToUpper();
        }

        public static string ToUpperString(this Guid? id)
        {
            return id.HasValue ? id.ToString().ToUpper() : "";
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return $"{dateTime: HH:mm:ss}";
        }
    }
}

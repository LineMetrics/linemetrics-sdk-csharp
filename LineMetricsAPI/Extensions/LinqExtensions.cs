using System;
using System.Collections;
using System.Collections.Generic;

namespace LineMetrics.API.Extensions
{
    public static class LinqExtensions
    {
        public static long UnixTicks(this DateTime that)
        {
            TimeSpan span = new TimeSpan(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
            DateTime time = that.Subtract(span);
            return time.Ticks / 10000;
        }

        public static DateTime DateTimeFromUnix(this long that)
        {
            TimeSpan span = new TimeSpan(that * 10000);
            DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixTime.Add(span);
        }

        public static void ForEach<T>(this IEnumerable<T> that, Action<T> processor)
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            foreach (var t in that)
            {
                processor(t);
            }
        }

        public static bool IsNullOrWhiteSpace(this string that)
        {
            return (that == null || that.Trim().Length == 0);
        }

        public static bool IsNullOrEmptyString(this object that)
        {
            if (that == null)
            {
                return true;
            }

            if (that is string)
            {
                return ((string)that).IsNullOrWhiteSpace();
            }

            return false;
        }

        public static void AssertNotNullOrEmpty(this object that, string name)
        {
            if (that.IsNullOrEmptyString())
            {
                throw new ArgumentException(name + " must not be null or empty!");
            }
        }

        public static T FirstOrDefault<T>(this IList that)
        {
            if (that.Count > 0)
            {
                return (T)that[0];
            }

            return default(T);
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType
            && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}

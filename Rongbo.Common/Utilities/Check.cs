using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rongbo.Common.Utilities
{
    public static class Check
    {

        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                NotEmpty(parameterName, nameof(parameterName));
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, string parameterName)
        {
            NotNull(value, parameterName);
            if (value.Count == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));
                throw new ArgumentException($"{parameterName} Collection Is Empty");
            }
            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            Exception exception = null;
            if (value == null)
                exception = new ArgumentNullException(parameterName);
            else if (value.Trim().Length == 0)
                exception = new ArgumentException($"{parameterName} Argument Is Empty");
            if (exception != null)
            {
                NotEmpty(parameterName, nameof(parameterName));
                throw exception;
            }
            return value;
        }

        public static TimeSpan NotNegativeOrZero(TimeSpan argument, string argumentName)
        {
            if (argument <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(argumentName);
            return argument;
        }

        public static void NotEmpty<T>(IDictionary<string, T> argument, string argumentName)
        {
            if (argument == null || argument.Count <= 0)
                throw new ArgumentNullException(argumentName);
        }

        public static void NotEmpty<T>(IEnumerable<T> argument, string argumentName)
        {
            if (argument == null || argument.Count() <= 0)
                throw new ArgumentNullException(argumentName);
        }
    }
}

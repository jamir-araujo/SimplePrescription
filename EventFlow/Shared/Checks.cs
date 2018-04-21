using System;

namespace Prescription
{
    internal static class Checks
    {
        public static T NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotNullEmptyOrWhiteSpaces(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static decimal NotZeroOrLess(decimal value, string parameterName)
        {
            if (value <= 0)
            {
                throw new InvalidOperationException($"parameter {parameterName} cannot be zero o less");
            }

            return value;
        }
    }
}
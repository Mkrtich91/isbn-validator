using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace IsbnValidator
{
    public static class Validator
    {
        /// <summary>
        /// Returns true if the specified <paramref name="isbn"/> is valid; returns false otherwise.
        /// </summary>
        /// <param name="isbn">The string representation of 10-digit ISBN.</param>
        /// <returns>true if the specified <paramref name="isbn"/> is valid; false otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="isbn"/> is empty or has only white-space characters.</exception>
        public static bool IsIsbnValid(string isbn)
        {
            if (string.IsNullOrEmpty(isbn) || string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("IsNullOrEmpty or IsNullOrWhiteSpace");
            }

            int length = isbn.Length;
            int checksum = 0;
            int digitCount = 0;
            bool lastDigitIsX = true;
            for (int i = 0; i < length; i++)
            {
                char c = isbn[i];
                if (isbn[i] == '-' && isbn[i + 1] == '-')
                {
                    return false;
                }

                if (char.IsDigit(c))
                {
                    int digit = int.Parse(c.ToString(), CultureInfo.InvariantCulture);
                    checksum += digit * (10 - digitCount);
                    digitCount++;
                    lastDigitIsX = false;
                }
                else if (i == length - 1 && c == 'X')
                {
                    checksum += 10 * (10 - digitCount);
                    digitCount++;
                }
                else if (c != '-')
                {
                    return false;
                }

                if (isbn.Length == 14 && isbn[0] == '3' && isbn[13] == 'X')
                {
                    return false;
                }
            }

            return digitCount == 10 && checksum % 11 == 0 && !lastDigitIsX;
        }
    }
}

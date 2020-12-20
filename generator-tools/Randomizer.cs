using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace generator_tools
{
    public class Randomizer
    {
        public const string CHARS = "abcdefghijklmnopqrstuvwxyz_ABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890 ";

        public const string NUM_CHARS = "1234567890 ";

        public const string ALPHA_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string VOWEL_CHARS = "aeiou";

        public const string CONSO_CHARS = "bcdfghjklmnpqrstvwxyz";

        public static string GenerateString(int length)
        {
            return Randomizer.GenerateString(length, StringCasing.Random);
        }

        public static string GenerateString(int length, StringCasing casing = StringCasing.Random)
        {
            var rand = new Random();
            var res = string.Empty;
            for (int i = 0; i < length; i++)
            {
                res += CHARS[rand.Next(0, CHARS.Length - 1)];
            }

            res = casing switch
            {
                StringCasing.Title => res.Substring(0, 1).ToUpper() + res.Substring(1),
                StringCasing.Random => res,
                StringCasing.Upper => res.ToUpper(),
                StringCasing.Lower => res.ToLower(),
                _ => res,
            };

            return res;
        }

        public static string GenerateNaturalWord(int length, bool includeSpaces = false, StringCasing casing = StringCasing.Random)
        {
            var rand = new Random();
            var isLastVowel = GenerateBool();
            var res = string.Empty;
            for (int i = 0; i < length; i++)
            {
                if (includeSpaces && GenerateBool())
                {
                    res += ' ';
                    isLastVowel = GenerateBool();
                    continue;
                }

                res += isLastVowel
                    ? CONSO_CHARS[rand.Next(0, CONSO_CHARS.Length - 1)]
                    : VOWEL_CHARS[rand.Next(0, VOWEL_CHARS.Length - 1)];

                isLastVowel = !isLastVowel;
            }

            res = casing switch
            {
                StringCasing.Title => res.Substring(0, 1).ToUpper() + res.Substring(1),
                StringCasing.Random => res,
                StringCasing.Upper => res.ToUpper(),
                StringCasing.Lower => res.ToLower(),
                _ => res,
            };

            return res;
        }

        public static string GenerateNumberString(int length)
        {
            var rand = new Random();
            var res = string.Empty;
            for (int i = 0; i < length; i++)
            {
                res += NUM_CHARS[rand.Next(0, NUM_CHARS.Length - 1)];
            }

            return res;
        }

        public static decimal GenerateDecimal(int min = 0, int max = 100_000)
        {
            var rand = new Random();
            var res = (decimal)rand.Next(min, max);
            return res;
        }

        public static int GenerateInt(int min = 0, int max = 100_000)
        {
            var rand = new Random();
            var res = rand.Next(min, max);
            return res;
        }

        public static bool GenerateBool()
        {
            var rand = new Random();
            var res = rand.Next(0, 2) > 0;
            return res;
        }

        public static DateTime GenerateDateTime(DateTime? min, DateTime? max)
        {
            var minDate = min != null
                ? min.Value < SqlDateTime.MinValue.Value
                    ? SqlDateTime.MinValue.Value
                    : min.Value
                : SqlDateTime.MinValue.Value;
            var maxDate = max != null
                ? max.Value > SqlDateTime.MaxValue.Value
                    ? SqlDateTime.MaxValue.Value
                    : max.Value
                : SqlDateTime.MaxValue.Value;

            if (minDate > maxDate)
            {
                var tempDate = maxDate;
                minDate = maxDate;
                maxDate = tempDate;
            }

            if (minDate == maxDate)
            {
                return minDate;
            }

            var maxTimeSpan = maxDate - minDate;
            var rand = new Random();
            var randomTimeSpan = new TimeSpan(rand.Next(1, Math.Abs((int)maxTimeSpan.Ticks)));

            return minDate + randomTimeSpan;
        }

        public static DateTime GenerateDate(int years = 3, int months = 12, int days = 30)
        {
            var rand = new Random();
            return DateTime.UtcNow
                .AddYears(rand.Next(0, years))
                .AddMonths(rand.Next(0, months))
                .AddDays(rand.Next(0, days));
        }

        public static T PickFrom<T>(IEnumerable<T> array)
        {
            var rand = new Random();
            return array.ElementAt(rand.Next(array.Count() - 1));
        }
    }

    public enum StringCasing
    {
        Title,

        Random,

        Upper,

        Lower,
    }
}

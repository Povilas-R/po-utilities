using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Po.Utilities
{
    /// <summary>
    /// Contains various helpful extension methods.
    /// </summary>
    public static class UtilityExtensions
    {
        /// <summary>
        /// Returns double as string without the scientific notation.
        /// </summary>
        public static string GetString(this double value)
        {
            if (value is double.NaN)
            {
                return null;
            }
            else
            {
                return value.ToString("F99").TrimEnd('0').TrimEnd('.');
            }
        }

        /// <summary>
        /// Reads and returns all new lines from the <see cref="StreamReader"/> buffer.
        /// </summary>
        /// <param name="readFromBeginning">True resets the position within the current stream.</param>
        public static string[] ReadAllLines(this StreamReader streamReader, bool readFromBeginning = false)
        {
            if (readFromBeginning)
            {
                streamReader.DiscardBufferedData();
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            string[] lines = streamReader.ReadToEnd().Split('\n');
            if (string.IsNullOrEmpty(lines.Last().TrimEnd('\r')))
            {
                return lines.Take(lines.Length - 1).ToArray();
            }
            return lines;
        }

        /// <summary>
        /// Returns a string of fixed length.
        /// Adds leading whitespaces.
        /// </summary>
        /// <param name="fixedLength">Target length of string.</param>
        public static string GetFixedString(this int value, int fixedLength)
        {
            string result = value.ToString();
            return
                string.Concat(string.Empty, Enumerable.Repeat(" ", fixedLength - result.Length).ToArray()) //TODO: test it out
                + result;
        }
        /// <summary>
        /// Returns a string of fixed length.
        /// Adds leading whitespaces and trailing zeroes.
        /// </summary>
        /// <param name="prePointLength">Target length of string before decimal point.</param>
        /// <param name="postPointLength">Target number of digits after decimal point.</param>
        /// <returns></returns>
        public static string GetFixedString(this double value, int prePointLength, int postPointLength = -1)
        {
            if (value is double.NaN)
            {
                return string.Concat(Enumerable.Repeat(" ", Math.Max(prePointLength - 3, 0)).ToArray(), "NaN"); //TODO: test it out
            }

            string result =
                postPointLength == -1
                    ? value.GetString()
                    : Math.Round(value, postPointLength).GetString();
            string[] split = result.Split('.');
            if (split.Length == 1)
            {
                split = new string[] { split[0], "" };
            }
            string extraSpace =
                (prePointLength - split[0].Length) <= 0
                ? ""
                : string.Concat(Enumerable.Repeat(" ", prePointLength - split[0].Length).ToArray());
            string extraZeroes =
                postPointLength <= 0
                ? ""
                : string.Concat(Enumerable.Repeat("0", postPointLength - split[1].Length).ToArray());
            return extraSpace + split[0] + (postPointLength <= 0 ? "" : "." + split[1] + extraZeroes);
        }

        /// <summary>
        /// Returns the default value of the given <see cref="SettingsProperty"/> from <see cref="SettingsPropertyCollection"/> by the property name.
        /// </summary>
        public static type GetDefault<type>(this SettingsPropertyCollection properties, string property)
        {
            string str = (string)properties[property].DefaultValue;

            return (type)Convert.ChangeType(str, typeof(type));
        }
    }
}
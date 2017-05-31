/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Kuno.Serialization;

#if !core
using System.Threading;
#endif

namespace Kuno.Text
{
    /// <summary>
    /// Contains extensions for <see cref="string" /> objects.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Compresses the specified string instance.
        /// </summary>
        /// <param name="instance">The instance to compress.</param>
        /// <returns>Returns the compressed string.</returns>
        public static string Compress(this string instance)
        {
            var bytes = Encoding.UTF8.GetBytes(instance);

            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var zip = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        inStream.CopyTo(zip);
                    }

                    return Convert.ToBase64String(outStream.ToArray());
                }
            }
        }

        /// <summary>
        /// Outputs the JSON representation to a file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="path">The file path.</param>
        public static void OutputToFile(this object instance, string path)
        {
            File.WriteAllText(path, instance.ToJson());
        }

        /// <summary>
        /// Outputs the JSON representation to the console.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void OutputToJson(this object instance)
        {
            Console.WriteLine(instance.ToJson());
        }

        /// <summary>
        /// Resizes the specified string.
        /// </summary>
        /// <param name="text">The specified string.</param>
        /// <param name="count">The desired length.</param>
        /// <param name="pad">The pad character if needed.</param>
        /// <returns>A String of the specified length.</returns>
        public static string Resize(this string text, int count, char pad)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new string(text.Take(count).ToArray()).PadRight(count, pad);
        }

        /// <summary>
        /// Returns a copy of the string in camel case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in camel case.</returns>
        public static string ToCamelCase(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.Substring(0, 1).ToLowerInvariant() + instance.Substring(1);
        }

        /// <summary>
        /// Returns a copy of the string delimited with the specified delimiter.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>Returns a copy of the string with the added delimiter.</returns>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public static string ToDelimited(this string instance, string delimiter)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return string.Concat(instance.Select((x, i) => i > 0 && char.IsUpper(x) ? delimiter + x.ToString() : x.ToString())).ToLowerInvariant();
        }

        /// <summary>
        /// Returns the JSON representation.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns the JSON representation.</returns>
        public static string ToJson(this object instance)
        {
            return JsonConvert.SerializeObject(instance, DefaultSerializationSettings.Instance);
        }

        /// <summary>
        /// Returns a copy of the string in pascal case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in pascal case.</returns>
        public static string ToPascalCase(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.Substring(0, 1).ToUpperInvariant() + instance.Substring(1);
        }

        /// <summary>
        /// Returns a copy of the string as a sentence.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string as a sentence.</returns>
        public static string ToSentence(this string instance)
        {
            return instance.Replace("_", " ");
        }

        /// <summary>
        /// Returns a copy of the string in snake case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in snake case.</returns>
        public static string ToSnakeCase(this string instance)
        {
            return instance.ToDelimited("-");
        }

        /// <summary>
        /// Returns a copy of the string as a title.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string as a title.</returns>
        public static string ToTitle(this string instance)
        {
            instance = Regex.Replace(instance, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToUpper(m.Value[1])}");

#if core
            return Regex.Replace(instance, @"^\w|\b\w(?=\w{{2}})", m => m.Value.ToUpperInvariant());
#else
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(instance);
#endif
        }

        /// <summary>
        /// Uncompresses the specified string instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns an uncompressed string.</returns>
        public static string Uncompress(this string instance)
        {
            var bytes = Convert.FromBase64String(instance);
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var zip = new GZipStream(inStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outStream);
                    }

                    return Encoding.UTF8.GetString(outStream.ToArray());
                }
            }
        }
    }
}
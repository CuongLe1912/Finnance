using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace URF.Core.Helper.Extensions
{
    public static class StringExtensions
    {
        public static string TrimEx(this string source)
        {
            if (source == null) return string.Empty;
            return source.Trim();
        }
        public static bool EqualsEx(this string source, object obj)
        {
            if (source == null) return obj == null;
            return obj != null &&
                   (source.Equals(obj.ToString(), StringComparison.CurrentCultureIgnoreCase) ||
                    source.Trim().Equals(obj.ToString().Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        public static bool ContainsEx(this string source, object obj)
        {
            if (obj.IsStringNullOrEmpty()) return true;
            if (source.IsStringNullOrEmpty()) return false;
            return source.Contains(obj.ToString()) || source.ToLower().Contains(obj.ToString().ToLower());
        }
        public static bool EndsWithEx(this string source, string obj)
        {
            return source.EndsWith(obj) || source.ToLower().Trim().EndsWith(obj.ToLower().Trim());
        }
        public static bool StartsWithEx(this string source, object obj)
        {
            if (source.IsStringNullOrEmpty()) return false;
            return source.StartsWith(obj.ToString()) || source.Trim().StartsWith(obj.ToString().Trim(), true, CultureInfo.CurrentCulture);
        }
        public static string SubstringEx(this string source, int startIndex)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            if (source.Length <= startIndex)
                return string.Empty;
            return source.Substring(startIndex, 1);
        }
        public static string JoinEx(this string[] sources, string separator = default)
        {
            if (sources.IsNullOrEmpty())
                return string.Empty;
            if (separator == default) 
                separator = ", ";
            var result = string.Empty;
            foreach (var item in sources)
            {
                if (item.IsStringNullOrEmpty()) continue;
                result += result.IsStringNullOrEmpty() ? item : (separator + item);
            }
            return result;
        }
        public static decimal SumEx(this decimal?[] sources)
        {
            decimal result = 0;
            foreach (var item in sources)
            {
                result += item ?? 0;
            }
            return result;
        }
        public static bool StartsWithEx(this string source, params object[] objects)
        {
            return objects.ToList().Exists(source.StartsWithEx);
        }
        public static string ReplaceEx(this string source, string pattern, string replacement = default(string), int indexReplacement = 0)
        {
            if (source.IsStringNullOrEmpty())
                return string.Empty;
            if (pattern.IsStringNullOrEmpty())
                return source;
            if (replacement == null)
                replacement = string.Empty;

            source = indexReplacement > 0
                ? (new Regex(pattern, RegexOptions.IgnoreCase)).Replace(source, replacement, 1)
                : Regex.Replace(source, pattern, replacement);
            return source;
        }
        public static string ReplaceTags(this string source)
        {
            return source.IsStringNullOrEmpty() ? string.Empty : Regex.Replace(source, "<.*?>", string.Empty).Trim();
        }

        public static string ReplaceNewLine(this string source, string replacement = default(string))
        {
            if (source.IsStringNullOrEmpty())
                return string.Empty;

            return source.Replace("\r", replacement).Replace("\n", replacement).Replace("\t", replacement).Trim();
        }

        public static string ReplaceEx(this string source, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = source.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(source.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = source.IndexOf(oldValue, index, comparison);
            }
            sb.Append(source.Substring(previousIndex));

            return sb.ToString();
        }

        public static bool IsMatch(this string source, string pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (source.IsStringNullOrEmpty() || pattern.IsStringNullOrEmpty())
                return false;

            return (new Regex(pattern, regexOptions)).IsMatch(source);
        }
        public static bool IsMatchAll(this string source, string[] pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (pattern == null || pattern.Length == 0)
                return false;

            return pattern.All(c => (new Regex(c, regexOptions)).IsMatch(source));
        }
        public static bool IsMatchAny(this string source, string[] pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (pattern == null || pattern.Length == 0)
                return false;

            return pattern.Any(c => (new Regex(c, regexOptions)).IsMatch(source));
        }

        public static string ToJson(this object obj)
        {
            return obj == null ? string.Empty : JsonConvert.SerializeObject(obj);
        }
        public static T ToObject<T>(this string json) where T : class
        {
            try
            {
                return json.IsStringNullOrEmpty() ? default : JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
        public static string ToDomain(this string source)
        {
            source = source
                .Replace("http://m.", string.Empty)
                .Replace("https://m.", string.Empty)
                .ReplaceEx(@"www\d{1,}\.", string.Empty);
            source = source.ToLinkKey();
            var index = source.IndexOfAny(new[] { ' ', '/', '?', '#', ':' });
            if (index > 0) source = source.Substring(0, index);
            return source.Trim();
        }
        public static string ToLinkKey(this string source, string regexLinkKey = default)
        {
            if (source.IsStringNullOrEmpty())
                return string.Empty;

            if (regexLinkKey.IsStringNullOrEmpty())
            {
                return source
                    .ReplaceEx("www\\d{1,}.", "")
                    .Replace("https://", "")
                    .Replace("http://", "")
                    .Replace("&amp;", "&")
                    .Replace("www.", "")
                    .ToLowerInvariant()
                    .Trim(new[] { ' ', '\r', '\n', '\t', '-', '/', '\\' });
            }

            var linkKey = source.GetElement(regexLinkKey);
            return linkKey.IsStringNullOrEmpty()
                       ? source.ToLinkKey()
                       : linkKey;
        }

        public static string GetElement(this string source, string pattern, bool removeTags = false, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (source.IsStringNullOrEmpty() || pattern.IsStringNullOrEmpty())
                return string.Empty;

            var mt = (new Regex(pattern, regexOptions)).Match(source);
            var result = mt.Success
                       ? (pattern.Contains("(?<text>") ? mt.Groups["text"].Value : mt.Value)
                       : string.Empty;
            if (!result.IsStringNullOrEmpty() && removeTags) result = result.ReplaceTags();
            return result;
        }
        public static List<string> GetElements(this string source, string pattern, bool removeTags = false, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (source.IsStringNullOrEmpty() || pattern.IsStringNullOrEmpty())
                return null;

            source = source.ReplaceNewLine();
            var result = new List<string>();
            var items = new[] { pattern };
            return GetElements(source, items, removeTags, regexOptions);
        }
        public static List<string> GetElements(this string source, string[] patterns, bool removeTags = false, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            if (source.IsStringNullOrEmpty() || patterns == null || patterns.Length == 0)
                return null;

            source = source.ReplaceNewLine();
            var result = new List<string>();
            foreach (var item in patterns)
            {
                var mtCol = (new Regex(item, regexOptions)).Matches(source);
                var list = mtCol.Count > 0
                           ? (mtCol.Cast<Match>().Select(mt => item.Contains("(?<text>") ? mt.Groups["text"].Value : mt.Value)).ToList()
                           : null;
                if (!list.IsNullOrEmpty()) result.AddRange(list);
                if (!result.IsNullOrEmpty() && removeTags) result = result.Select(c => c.ReplaceTags()).ToList();
            }
            return result;
        }
        
        public static List<int> ToListId(this string source, bool removeNullOrEmpty = true)
        {
            if (source.IsStringNullOrEmpty()) return new List<int>();
            var list = source.ReplaceNewLine().Split(new[] { "[", ",", ";", "]", "\"", " ", "-" }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToInt32());
            return removeNullOrEmpty ? list.Where(c => !c.IsNumberNull()).ToList() : list.ToList();
        }
        public static List<double> ToListDoube(this string source, bool removeNullOrEmpty = true)
        {
            if (source.IsStringNullOrEmpty()) return new List<double>();
            var list = source.ReplaceNewLine().Split(new[] { "[", ",", ";", "]", "\"", " ", "-" }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToDouble());
            return removeNullOrEmpty ? list.Where(c => !c.IsNumberNull()).ToList() : list.ToList();
        }
        public static List<decimal> ToListDecimal(this string source, bool removeNullOrEmpty = true)
        {
            if (source.IsStringNullOrEmpty()) return new List<decimal>();
            var list = source.ReplaceNewLine().Split(new[] { "[", ",", ";", "]", "\"", " ", "-" }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToDecimal());
            return removeNullOrEmpty ? list.Where(c => !c.IsNumberNull()).ToList() : list.ToList();
        }
        public static List<bool> ToListBoolean(this string source, bool removeNullOrEmpty = true)
        {
            if (source.IsStringNullOrEmpty()) return new List<bool>();
            var list = source.ReplaceNewLine().Split(new[] { "[", ",", ";", "]", "\"", " ", "-" }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToBoolean());
            return removeNullOrEmpty ? list.Where(c => !c.IsNumberNull()).ToList() : list.ToList();
        }
        public static List<string> ToListString(this string source, bool removeNullOrEmpty = true)
        {
            if (source == null) return new List<string>();
            var list = source.Split(new[] { "[", ",", ";", "]" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim().Trim(new[] { ' ', '"' }));
            return removeNullOrEmpty ? list.Where(c => !c.IsStringNullOrEmpty()).ToList() : list.ToList();
        }


        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            var gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }


        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public static string Encrypt(this string text)
        {
            try
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
                byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Convert.ToBase64String(outputBuffer);
            }
            catch
            {
                return text;
            }
        }
        public static string Decrypt(this string text)
        {
            try
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
                byte[] inputbuffer = Convert.FromBase64String(text);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                return Encoding.Unicode.GetString(outputBuffer);
            }
            catch
            {
                return text;
            }
        }
    }
}


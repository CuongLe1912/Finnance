using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace URF.Core.Helper.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToSlug(this string value)
        {
            if (value.IsStringNullOrEmpty())
                return string.Empty;
            value = value.ToNoSign().Replace("/", "-").Replace(".", "-").Replace("#", "-").Replace("?", "-");
            value = Regex.Replace(value, "[^a-zA-Z0-9_./]+", "-", RegexOptions.Compiled);
            while (value.Contains("--")) value = value.Replace("--", "-");
            value = value.Trim(new[] { ' ', '-', '.' });
            value = value.ToLower();
            return value;
        }
        public static string ToNoSign(this string value)
        {
            string stFormD = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            var result = sb.ToString().Normalize(NormalizationForm.FormD);
            result = result
                .Replace(" - ", " ")
                .Replace(" -", " ")
                .Replace("- ", " ")
                .Replace("-", " ");
            return result;
        }
        public static byte ToByte(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            byte value;
            return byte.TryParse(source.ToString(), out value) ? value : Convert.ToByte(0);
        }
        public static int ToInt32(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;
            int value;
            return int.TryParse(source.ToString(), out value) ? value : 0;
        }
        public static long ToInt64(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            long value;
            return long.TryParse(source.ToString(), out value) ? value : 0;
        }
        public static short ToInt16(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            short value;
            return short.TryParse(source.ToString(), out value) ? value : (short)0;
        }
        public static float ToFloat(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            float value;
            return float.TryParse(source.ToString(), out value) ? value : 0;
        }
        public static bool ToBoolean(this object source)
        {
            if (source.IsStringNullOrEmpty()) return false;
            if (source.ToString() == "1") return true;

            bool value;
            return bool.TryParse(source.ToString(), out value) && value;
        }
        public static double ToDouble(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            double value;
            return double.TryParse(source.ToString(), out value) ? value : 0;
        }
        public static decimal ToDecimal(this object source)
        {
            if (source.IsStringNullOrEmpty()) return 0;

            decimal value;
            return decimal.TryParse(source.ToString(), out value) ? value : 0;
        }
        public static IDictionary<string, object> ToDictionary(this object value)
        {
            if (value == null) return null;
            var properties = TypeDescriptor.GetProperties(value.GetType());
            IDictionary<string, object> expando = new Dictionary<string, object>();
            foreach (PropertyDescriptor property in properties)
                expando.Add(property.Name, property.GetValue(value));
            return expando;
        }
        public static DateTime ToDateTime(this object source, string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (source is DateTime)
                return (DateTime)source;
            if (source.IsStringNullOrEmpty()) return default(DateTime);
            var value = source.ToString().Trim();

            DateTime result;
            if (value.Contains(" "))
            {
                var formats = new[] { format, "ddMMyyyy HH:mm:ss", "d/M/yyyy HH:mm:ss" };
                if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
            }
            else
            {
                var formats = new[] { format, "ddMMyyyy", "d/M/yyyy" };
                if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
            }
            return result;
        }

        public static bool IsList(this object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        public static bool IsNumber(this object obj)
        {
            if (obj == null) return false;

            int outObjInt;
            if (int.TryParse(obj.ToString(), out outObjInt)) return true;

            short outObjShort;
            if (short.TryParse(obj.ToString(), out outObjShort)) return true;
            
            long outObjLong;
            if (long.TryParse(obj.ToString(), out outObjLong)) return true;

            return false;
        }
        public static bool IsNumberNull(this object obj)
        {
            try
            {
                if (obj == null) return true;
                if (obj.IsNumber() && obj.ToString().Trim() == "") return true;
                if (obj.IsNumber() && obj.ToString().Trim() == "0") return true;
                if (obj.IsNumber() && obj.ToString().Trim() == "-1") return true;
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static bool IsStringNullOrEmpty(this object obj)
        {
            return obj == null ||
                   obj.ToString().Length == 0 ||
                   obj.ToString().Trim().Length == 0;
        }

        private static bool IsDateTime(this object obj)
        {
            DateTime outObj;
            return DateTime.TryParse(obj.ToString(), out outObj);
        }
        public static bool IsDateTimeNull(this object obj)
        {
            try
            {
                if (obj == null) return true;
                if (obj.IsDateTime() && obj.ToString().Trim() == "") return true;
                if (obj.IsDateTime() && (DateTime)obj == default(DateTime)) return true;
                if (obj.IsDateTime() && Convert.ToDateTime(obj) == DateTime.MinValue) return true;
                if (obj.IsDateTime() && obj.ToString().IndexOf("1/1/1900") > -1) return true;
                if (obj.IsDateTime() && obj.ToString().IndexOf("01/01/1900") > -1) return true;
                if (obj.IsDateTime() && obj.ToString().IndexOf("1/1/1753") > -1) return true;
                return false;
            }
            catch
            {
                return true;
            }
        }
       
        public static T Transform<T>(this object obj) where T : class
        {
            if (obj == null) return default(T);

            var objTarget = Activator.CreateInstance<T>();
            var objTargetProperties = typeof(T).GetProperties();
            var objFromProperties = obj.GetType().GetProperties();
            foreach (var fromProperty in objFromProperties)
            {
                foreach (var targetProperty in objTargetProperties
                    .Where(c => c.CanWrite && fromProperty.Name == c.Name))
                    targetProperty.SetValue(objTarget, fromProperty.GetValue(obj, null), null);
            }
            return objTarget;
        }
        public static T CreateObjectDefault<T>(this object obj) where T : class
        {
            if (obj == null) return default(T);

            var objTarget = Activator.CreateInstance<T>();
            var objTargetProperties = typeof(T).GetProperties();
            var objFromProperties = obj.GetType().GetProperties();
            foreach (var fromProperty in objFromProperties)
            {
                foreach (var targetProperty in objTargetProperties
                    .Where(c => c.CanWrite && fromProperty.Name == c.Name))
                    targetProperty.SetValue(objTarget, fromProperty.GetValue(obj, null), null);
            }
            return objTarget;
        }
        public static bool HasValue(this JObject jsonObj, string key1 = "data", string key2 = default)
        {
            if (jsonObj == null) return false;
            if (jsonObj.ContainsKey(key1) && !jsonObj[key1].ToString().IsStringNullOrEmpty())
            {
                if (!key2.IsStringNullOrEmpty())
                {
                    try
                    {
                        var jsonObjKey1 = (JObject)jsonObj[key1];
                        if (jsonObjKey1.ContainsKey(key2) && !jsonObjKey1[key2].ToString().IsStringNullOrEmpty())
                            return true;
                        else
                            return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static IDictionary<string, object> ToDictionary(this JObject @object)
        {
            return @object.ToObject<Dictionary<string, object>>();

        }
    }

    public static class AnonymousObjectMutator
    {
        private const BindingFlags FieldFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        private static readonly string[] BackingFieldFormats = { "<{0}>i__Field", "<{0}>" };

        public static T Set<T, TProperty>(
            this T instance,
            Expression<Func<T, TProperty>> propExpression,
            TProperty newValue) where T : class
        {
            var pi = (propExpression.Body as MemberExpression).Member;
            var backingFieldNames = BackingFieldFormats.Select(x => string.Format(x, pi.Name)).ToList();
            var fi = typeof(T)
                .GetFields(FieldFlags)
                .FirstOrDefault(f => backingFieldNames.Contains(f.Name));
            if (fi == null)
                throw new NotSupportedException(string.Format("Cannot find backing field for {0}", pi.Name));
            fi.SetValue(instance, newValue);
            return instance;
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CongLib.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, settings);
        }
        public static string ToCamelCaseName<TModel, TProp>(this Expression<Func<TModel, TProp>> property)
        {
            var pascalCaseName = ExpressionHelper.GetExpressionText(property);
            var camelCaseName = ConvertFullNameToCamelCase(pascalCaseName);
            return camelCaseName;
        }
        public static string ConvertFullNameToCamelCase(string pascalCaseName)
        {
            var parts = pascalCaseName.Split('.').Select(ConvertToCamelCase);
            return string.Join(".", parts);
        }
        public static string ConvertToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            if (!char.IsUpper(s[0])) return s;
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;
                chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);

            }
            return new string(chars);
        }
    }
}

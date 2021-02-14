using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlterDice.Net.Helpers
{
    public static class Extensions
    {
        public static Dictionary<string, object> AsDictionary(this object source,
          BindingFlags bindingAttr = BindingFlags.FlattenHierarchy |
          BindingFlags.Instance |
          BindingFlags.NonPublic |
          BindingFlags.Public |
          BindingFlags.Static)
        {
            try
            {
                var result = new Dictionary<string, object>();
                var props = source.GetType().GetProperties(bindingAttr);
                foreach (var p in props)
                {
                    if (p.IsDefined(typeof(JsonIgnoreAttribute)))
                        continue;
                    string key = p.Name;
                    if (p.IsDefined(typeof(JsonPropertyAttribute)))
                    {
                        key = p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName ?? p.Name;
                    }
                    object value = p.GetValue(source, null);

                    if (value == null)
                    {
                        continue;
                    }
                    if (value is bool)
                    {
                        value = value.ToString().ToLowerInvariant();
                    }
                   
                    if (value.GetType().IsEnum)
                    {
                        value = value?.ToString();
                    }
                    if (!result.ContainsKey(key) && !String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value.ToString()))
                    {
                        result.Add(key, value);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

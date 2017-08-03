namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    public static class ObjectExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this object obj)
        {
            var result = new RouteValueDictionary();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var enumerable = value as ICollection;
                if (enumerable != null)
                {
                    var i = 0;

                    foreach (var item in enumerable)
                    {
                        result.Add(string.Format("{0}[{1}]", p.Name, i), item.ToString());
                        i++;
                    }
                }
                else
                {
                    var date = value as DateTime?;
                    if (date != null)
                    {
                        result.Add(p.Name, date.Value.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        result.Add(p.Name, value.ToString());
                    }
                }
            }

            return result;
        }

        public static RouteValueDictionary ToRouteValueDictionary<T>(this IEnumerable<T> values, string key)
        {
            var result = new RouteValueDictionary();

            var valuesList = values.ToList();

            for (int i = 0; i < valuesList.Count; i++)
            {
                result.Add(string.Format("{0}[{1}]", key, i), valuesList[i]);
            }

            return result;
        }
    }
}
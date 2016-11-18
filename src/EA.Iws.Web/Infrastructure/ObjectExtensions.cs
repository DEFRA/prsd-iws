namespace EA.Iws.Web.Infrastructure
{
    using System.Collections;
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
                    result.Add(p.Name, value.ToString());
                }
            }

            return result;
        }
    }
}
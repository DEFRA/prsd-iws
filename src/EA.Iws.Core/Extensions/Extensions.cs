namespace EA.Iws.Core.Extensions
{
    using System;

    public static partial class Extensions
    {
        public static bool IsCustom(this Type type)
        {
            if (type.Namespace != null && type.Namespace.StartsWith("EA.Weee"))
            {
                return true;
            }

            return false;
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"A property with the name '{propertyName}' was not found", nameof(propertyName));
            }

            if (property.PropertyType != typeof(T))
            {
                throw new InvalidCastException($"The property '{propertyName}' is of type '{property.PropertyType}' not the specified '{typeof(T)}'");
            }

            return (T)property.GetValue(obj, null);
        }
    }
}

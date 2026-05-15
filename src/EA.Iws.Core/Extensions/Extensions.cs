namespace EA.Iws.Core.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static partial class Extensions
    {
        public static bool IsCustom(this Type type)
        {
            if (type.Namespace != null && type.Namespace.StartsWith("EA.Iws"))
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
                throw new ArgumentException(string.Format("A property with the name '{0}' was not found", propertyName));
            }

            if (property.PropertyType != typeof(T))
            {
                throw new InvalidCastException(string.Format("The property '{0}' is of type '{1}' not the specified '{2}'", propertyName, property.PropertyType, typeof(T)));
            }

            return (T)property.GetValue(obj, null);
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null) throw new ArgumentNullException(nameof(enumValue));

            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
                {
                    return displayAttribute.Name;
                }
            }
            return enumValue.ToString(); // Fallback to enum name
        }
    }
}

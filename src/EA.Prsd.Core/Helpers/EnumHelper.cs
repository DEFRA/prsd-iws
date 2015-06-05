namespace EA.Prsd.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class EnumHelper
    {
        public static string GetDisplayName<TEnum>(TEnum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DisplayAttribute)) as DisplayAttribute;
            if (attribute != null)
            {
                return attribute.Name;
            }
            return field.Name;
        }

        public static Dictionary<int, string> GetValues(Type enumType)
        {
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var fieldNames = new Dictionary<int, string>();

            foreach (var field in fields)
            {
                // Get the display attributes for the enum.
                var displayAttribute =
                    (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute)).SingleOrDefault();

                // Set field name to either the enum name or the display name.
                var name = (displayAttribute == null) ? field.Name : displayAttribute.Name;

                fieldNames.Add((int)field.GetValue(null), name);
            }

            return fieldNames;
        }
    }
}
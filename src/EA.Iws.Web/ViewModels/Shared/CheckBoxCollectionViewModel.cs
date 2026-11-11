namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    public class CheckBoxCollectionViewModel
    {
        public IList<SelectListItem> PossibleValues { get; set; }

        public bool ShowEnumValue { get; set; }

        /// <summary>
        /// The enum type this collection was created from. Used to resolve a checkbox's
        /// Value (the enum member's name) back to its underlying numeric value for display
        /// purposes only (e.g. when ShowEnumValue is true), without changing the Value used
        /// for form submission/model binding.
        /// </summary>
        public Type EnumType { get; set; }

        public void SetSelectedValues<TEnum>(IEnumerable<TEnum> selectedValues)
        {
            var enumerable = selectedValues as TEnum[] ?? selectedValues.ToArray();
            foreach (var item in PossibleValues)
            {
                item.Selected = enumerable.Any(p => p.ToString() == item.Value);
            }
        }

        public void SetSelectedValues(IEnumerable<int> selectedValues)
        {
            var enumerable = selectedValues as int[] ?? selectedValues.ToArray();
            foreach (var item in PossibleValues)
            {
                item.Selected = enumerable.Any(p => p.ToString() == item.Value);
            }
        }

        /// <summary>
        /// Returns the underlying numeric value for the given checkbox's Value (enum member name),
        /// for display purposes only.
        /// </summary>
        public int GetEnumValue(string value)
        {
            return Convert.ToInt32(Enum.Parse(EnumType, value));
        }

        /// <summary>
        /// Creates SelectListItem collection based on values in an enum.
        /// The SelectListItem.Value is set to the enum member's name (not its numeric value),
        /// so that it can be reliably matched against the corresponding property name on the
        /// data class used to generate the report, regardless of how the enum members are numbered.
        /// </summary>
        public static CheckBoxCollectionViewModel CreateFromEnum<T>()
        {
            if (!(typeof(Enum).IsAssignableFrom(typeof(T))))
            {
                throw new InvalidOperationException();
            }

            // Get the enum fields if present.
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);
            var fieldNames = new List<SelectListItem>();

            foreach (var field in fields)
            {
                // Get the display attributes for the enum.
                var displayAttribute = (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute)).SingleOrDefault();

                // Set field name to either the enum name or the display name.
                var name = (displayAttribute == null) ? field.Name : displayAttribute.Name;

                fieldNames.Add(new SelectListItem { Text = name, Value = field.Name });
            }

            return new CheckBoxCollectionViewModel()
            {
                PossibleValues = fieldNames,
                EnumType = typeof(T)
            };
        }
    }
}
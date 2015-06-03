namespace EA.Iws.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Prsd.Core.Domain;

    public class CheckBoxCollectionViewModel 
    {
        public IList<SelectListItem> PossibleValues { get; set; }

        public bool ShowEnumValue { get; set; }

        /// <summary>
        /// Creates SelectListItem collection based on values in an enum.
        /// </summary>
        public static CheckBoxCollectionViewModel CreateFromEnum<T>(string selectedValue = null)
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

                fieldNames.Add(new SelectListItem{Text = name, Value = ((int)field.GetValue(null)).ToString() });
            }

            return new CheckBoxCollectionViewModel()
            {
                PossibleValues = fieldNames
            };
        }
    }
}
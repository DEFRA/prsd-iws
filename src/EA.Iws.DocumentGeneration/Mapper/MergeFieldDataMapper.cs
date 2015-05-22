namespace EA.Iws.DocumentGeneration.Mapper
{
    using System;
    using System.Reflection;

    internal class MergeFieldDataMapper
    {
        private const string CheckboxChecked = "\u2610";
        private const string CheckboxUnchecked = "\u2611";

        public static void BindCorrespondingField(MergeField mergeField, object model, PropertyInfo[] properties)
        {
            mergeField.RemoveCurrentContents();

            string text;
            if (mergeField.FieldType == MergeFieldType.Checkbox)
            {
                text = GetCheckboxForMergeField(mergeField, model, properties);
            }
            else
            {
                text = GetValueForMergeField(mergeField, model, properties);
            }

            mergeField.SetText(text);
        }

        private static string GetCheckboxForMergeField(MergeField mergeField, object viewModel, PropertyInfo[] properties)
        {
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.Name.Equals(mergeField.FieldName.InnerTypeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    object value = propertyInfo.GetValue(viewModel);

                    if (value is bool)
                    {
                        return Convert.ToBoolean(value) ? CheckboxChecked : CheckboxUnchecked;
                    }
                }
            }

            return string.Empty;
        }

        private static string GetValueForMergeField(MergeField mergeField, object viewModel, PropertyInfo[] properties)
        {
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.Name.Equals(mergeField.FieldName.InnerTypeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return propertyInfo.GetValue(viewModel).ToString();
                }
            }

            return string.Empty;
        }
    }
}

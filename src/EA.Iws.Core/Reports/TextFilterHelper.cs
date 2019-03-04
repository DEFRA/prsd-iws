namespace EA.Iws.Core.Reports
{
    using System;

    public static class TextFilterHelper
    {
        public static string GetTextFilter<T>(T? textFieldType, 
            TextFieldOperator? operatorType, 
            string textSearch) where T : struct
        {
            if (!textFieldType.HasValue ||
                !operatorType.HasValue ||
                string.IsNullOrEmpty(textSearch))
            {
                return null;
            }

            string textOperator;

            switch (operatorType)
            {
                case TextFieldOperator.StartsWith:
                    textOperator = string.Format("LIKE '{0}%'", textSearch);
                    break;
                case TextFieldOperator.Contains:
                    textOperator = string.Format("LIKE '%{0}%'", textSearch);
                    break;
                case TextFieldOperator.DoesNotContain:
                    textOperator = string.Format("NOT LIKE '%{0}%'", textSearch);
                    break;
                case TextFieldOperator.EqualsTo:
                    textOperator = string.Format("= '{0}'", textSearch);
                    break;
                default:
                    throw new ArgumentException(string.Format("Invalid operator type supplied: {0}", operatorType), "operatorType type");
            }

            return string.Format("[{0}] {1}", textFieldType, textOperator);
        }
    }
}

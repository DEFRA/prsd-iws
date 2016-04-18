namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;

    public class DateTimeFormatter
    {
        public string DateTimeToDocumentFormatString(DateTime? date)
        {
            return (date.HasValue) ? date.Value.ToString("dd.MM.yy") 
                : string.Empty;
        }
    }
}

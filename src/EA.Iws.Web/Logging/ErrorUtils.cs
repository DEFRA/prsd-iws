namespace EA.Iws.Web.Logging
{
    using EA.Iws.Core.Admin;
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class ErrorUtils
    {
        public static string GetExceptionAsXml(Exception ex, string message)
        {
            var err = new ElmahErrorXmlFormat()
            {
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType().Name,
                Message = message,
                StackTrace = ex.StackTrace
            };

            var apiError = new XmlSerializer(typeof(ElmahErrorXmlFormat));

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    apiError.Serialize(writer, err);
                    return sww.ToString();
                }
            }
        }
    }
}
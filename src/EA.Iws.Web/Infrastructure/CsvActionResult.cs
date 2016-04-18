namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    public class CsvActionResult<T> : FileResult
    {
        private readonly IList<T> list;
        private readonly char separator;

        public CsvActionResult(IList<T> list,
            string fileDownloadName,
            char separator = ',')
            : base("text/csv")
        {
            this.list = list;
            FileDownloadName = fileDownloadName;
            this.separator = separator;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteList(memoryStream);
                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        private void WriteList(Stream stream)
        {
            var streamWriter = new StreamWriter(stream, Encoding.Default);

            WriteHeaderLine(streamWriter);
            streamWriter.WriteLine();
            WriteDataLines(streamWriter);

            streamWriter.Flush();
        }

        private void WriteHeaderLine(StreamWriter streamWriter)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));
                if (attr == null)
                {
                    WriteValue(streamWriter, property.Name);
                }
                else
                {
                    WriteValue(streamWriter, attr.DisplayName);
                }
            }
        }

        private void WriteDataLines(StreamWriter streamWriter)
        {
            foreach (var line in list)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    WriteValue(streamWriter, GetPropertyValue(line, property.Name));
                }
                streamWriter.WriteLine();
            }
        }

        private void WriteValue(StreamWriter writer, string value)
        {
            writer.Write("\"");
            writer.Write(value.Replace("\"", "\"\""));
            writer.Write("\"" + separator);
        }

        public static string GetPropertyValue(object src, string propName)
        {
            var obj = src.GetType().GetProperty(propName).GetValue(src, null);
            return (obj != null) ? obj.ToString() : string.Empty;
        }
    }
}
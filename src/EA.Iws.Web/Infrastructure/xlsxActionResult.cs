namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using ClosedXML.Excel;

    public class XlsxActionResult<T> : FileResult
    {
        private readonly IEnumerable<T> data;
        private XLWorkbook workBook;
        private IXLWorksheet workSheet;

        public XlsxActionResult(IEnumerable<T> data, string fileDownloadName) : base(MimeTypes.MSExcelXml)
        {
            this.data = data;
            FileDownloadName = fileDownloadName;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteSheet(memoryStream);
                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        private void WriteSheet(Stream stream)
        {
            CreateWorkBook();
            
            workSheet.Cell(2, 1).Value = data.AsEnumerable();

            AddHeaderRow();

            FormatTitles();

            workSheet.Columns().AdjustToContents();

            workBook.SaveAs(stream);
        }

        private void CreateWorkBook()
        {
            workBook = new XLWorkbook();
            workSheet = workBook.Worksheets.Add("Report");
        }

        private void AddHeaderRow()
        {
            var properties = typeof(T).GetProperties();

            for (int i = 0; i < properties.Count(); i++)
            {
                var property = properties[i];
                string columnName;

                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));

                columnName = attr == null ? SplitCamelCase(property.Name) : attr.DisplayName;

                workSheet.Cell(1, i + 1).Value = columnName;
            }
        }

        private static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
        }

        private void FormatTitles()
        {
            var numberOfProperties = typeof(T).GetProperties().Count();

            workSheet.Range(1, 1, 1, numberOfProperties).AddToNamed("Titles");

            var titlesStyle = workBook.Style;
            titlesStyle.Font.Bold = true;
            titlesStyle.Font.FontColor = XLColor.White;
            titlesStyle.Fill.BackgroundColor = XLColor.DarkGreen;

            workBook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            workSheet.SheetView.FreezeRows(1);
        }
    }
}
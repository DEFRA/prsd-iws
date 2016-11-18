namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ClosedXML.Excel;

    public class XlsxActionResult<T> : FileResult
    {
        private readonly IEnumerable<T> data;
        private const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private XLWorkbook workBook;
        private IXLWorksheet workSheet;

        public XlsxActionResult(IEnumerable<T> data, string fileDownloadName) : base(contentType)
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

            // format header row

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
                if (attr == null)
                {
                    columnName = property.Name;
                }
                else
                {
                    columnName = attr.DisplayName;
                }

                workSheet.Cell(1, i + 1).Value = columnName;
            }
        }
    }
}
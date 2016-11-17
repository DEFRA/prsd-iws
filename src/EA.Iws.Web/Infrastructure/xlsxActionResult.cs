namespace EA.Iws.Web.Infrastructure
{
    using System.Collections.Generic;
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

            // add header row

            // format header row

            workSheet.Columns().AdjustToContents();

            workBook.SaveAs(stream);
        }

        private void CreateWorkBook()
        {
            workBook = new XLWorkbook();
            workSheet = workBook.Worksheets.Add("Report");
        }
    }
}
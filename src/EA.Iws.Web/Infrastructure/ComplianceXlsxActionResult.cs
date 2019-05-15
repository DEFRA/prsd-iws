namespace EA.Iws.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using ClosedXML.Excel;
    using Core.Admin.Reports;

    public class ComplianceXlsxActionResult : XlsxActionResult<ComplianceData>
    {
        private const string ColumnsToProcess = "C,E,G,I,K";

        public ComplianceXlsxActionResult(IEnumerable<ComplianceData> data, string fileDownloadName,
            bool fixedWidthFormat = false, string columnsToHide = null)
            : base(data, fileDownloadName, fixedWidthFormat, columnsToHide)
        {
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outputStream = response.OutputStream;
            using (var memoryStream = new MemoryStream())
            {
                WriteSheet();

                FormatColumns();

                WorkBook.SaveAs(memoryStream);

                outputStream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
        }

        private void FormatColumns()
        {
            var columnsLetters = ColumnsToProcess.Split(',');

            var nonemptyDataRows = Worksheet.RowsUsed();
            foreach (var dataRow in nonemptyDataRows)
            {
                foreach (var column in columnsLetters)
                {
                    var value = dataRow.Cell(column).GetValue<string>().Trim();
                    if (value.Equals("A"))
                    {
                        dataRow.Cell(column).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffbf47");
                        dataRow.Cell(column).Value = "!";
                        dataRow.Cell(column).Style.Font.FontName = "Times New Roman";
                        dataRow.Cell(column).Style.Font.FontSize = 22;
                        dataRow.Cell(column).Style.Font.Bold = true;
                    }
                    else if (value.Equals("G"))
                    {
                        dataRow.Cell(column).Value = "ü";
                        dataRow.Cell(column).Style.Fill.BackgroundColor = XLColor.FromHtml("#85994b");
                        dataRow.Cell(column).Style.Font.FontName = "Wingdings";
                        dataRow.Cell(column).Style.Font.FontSize = 24;
                    }
                    else if (value.Equals("R"))
                    {
                        dataRow.Cell(column).Value = "û";
                        dataRow.Cell(column).Style.Fill.BackgroundColor = XLColor.FromHtml("#b10e1e");
                        dataRow.Cell(column).Style.Font.FontColor = XLColor.White;
                        dataRow.Cell(column).Style.Font.FontName = "Wingdings";
                        dataRow.Cell(column).Style.Font.FontSize = 24;
                    }
                    if (value.Equals("G") || value.Equals("A") || value.Equals("R"))
                    {
                        dataRow.Cell(column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        dataRow.Cell(column).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }
                dataRow.Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //Column F has both numbers and text stored. 
                var columnValue = dataRow.Cell("F").GetValue<string>().Trim();
                if (dataRow.RowNumber() > 1 && !dataRow.IsEmpty() && !columnValue.Equals("N/A"))
                {
                    int overLimitCount;
                    if (int.TryParse(columnValue, out overLimitCount))
                    {
                        dataRow.Cell("F").Style.NumberFormat.NumberFormatId = 3;
                        dataRow.Cell("F").Value = overLimitCount;
                    }
                }
            }
        }
    }
}
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
        private bool fixedWidthFormat;
        private const int MaxPixelColWidth = 150;
        private string columnsToHide;
        private string columnsToProcess;
        public XlsxActionResult(IEnumerable<T> data, 
            string fileDownloadName, 
            bool fixedWidthFormat = false, string columnsToHide = null, string columnsToProcess = null) : base(MimeTypes.MSExcelXml)
        {
            this.data = data;
            FileDownloadName = fileDownloadName;
            this.fixedWidthFormat = fixedWidthFormat;
            this.columnsToHide = columnsToHide;
            this.columnsToProcess = columnsToProcess;
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

            if (!string.IsNullOrEmpty(columnsToHide))
            {
                workSheet.Columns(columnsToHide).Delete();
            }

            if (!string.IsNullOrEmpty(columnsToProcess))
            {
                FormatColumns();
            }

            if (fixedWidthFormat)
            {
                workSheet.Columns().Width = PixelWidthToExcel(MaxPixelColWidth);
                workSheet.Cells().Style.Alignment.WrapText = true;
            }
            else
            {
                workSheet.Columns().AdjustToContents();
            }

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
            titlesStyle.Fill.BackgroundColor = XLColor.LimeGreen;

            workBook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            workSheet.SheetView.FreezeRows(1);
        }

        private static double PixelWidthToExcel(int pixels)
        {
            if (pixels <= 0)
            {
                return 0;
            }

            return ((pixels * 256 / 7) - (128 / 7)) / 256;
        }

        private void FormatColumns()
        {
            var columnsLetters = columnsToProcess.Split(',');
            
            var nonemptyDataRows = workSheet.RowsUsed();
            foreach (var dataRow in nonemptyDataRows)
            {
                for (int i = 0; i < columnsLetters.Length; i++)
                {
                    string column = columnsLetters[i];
                    string value = dataRow.Cell(column).GetValue<string>().Trim();
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
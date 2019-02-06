namespace EA.Iws.Web.Infrastructure
{
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using ExcelDataReader;
    using VirusScanning;
    using DataTable = System.Data.DataTable;

    internal class FileReader : IFileReader
    {
        private readonly IVirusScanner virusScanner;

        public FileReader(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        public async Task<byte[]> GetFileBytes(HttpPostedFileBase file)
        {
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                throw new VirusFoundException(string.Format("Virus found in file {0}", file.FileName));
            }

            return fileBytes;
        }

        public async Task<DataTable> GetFirstDataTable(HttpPostedFileBase file, bool isCsv, bool useHeaderRow)
        {
            DataTable result = null;

            await GetFileBytes(file); // Check for virus.

            using (var reader = isCsv
                ? ExcelReaderFactory.CreateCsvReader(file.InputStream)
                : ExcelReaderFactory.CreateReader(file.InputStream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = useHeaderRow
                    },
                    UseColumnDataType = false
                });
                
                if (dataSet.Tables.Count > 0)
                {
                    // Set the default types to string for all columns.
                    var cloned = dataSet.Tables[0].Clone();

                    foreach (DataColumn column in cloned.Columns)
                    {
                        column.DataType = typeof(string);
                    }

                    cloned.Load(dataSet.CreateDataReader());

                    result = cloned;
                }
            }

            return result;
        }
    }
}
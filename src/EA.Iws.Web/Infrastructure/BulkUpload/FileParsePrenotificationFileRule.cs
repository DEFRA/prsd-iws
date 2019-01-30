namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class FileParsePrenotificationFileRule : IBulkMovementPrenotificationFileRule
    {
        private readonly IFileReader fileReader;
        private const int MaxColumns = 6;

        public DataTable DataTable { get; set; }

        public FileParsePrenotificationFileRule(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
        {
            MessageLevel result;

            try
            {
                var isCsv = file.ContentType == MimeTypes.Csv;

                var dataTable = await fileReader.GetFirstDataTable(file, isCsv, !isCsv);

                result = IsDataTableValid(dataTable) ? MessageLevel.Success : MessageLevel.Error;

                DataTable = dataTable;
            }
            catch (Exception)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileParse, result);
        }

        private static bool IsDataTableValid(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return false;
            }
            if (dataTable.HasErrors)
            {
                return false;
            }
            if (dataTable.Columns.Count != MaxColumns)
            {
                return false;
            }

            return true;
        }
    }
}
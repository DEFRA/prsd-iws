namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class FileParsePrenotificationFileRule : IBulkMovementPrenotificationFileRule
    {
        private readonly IFileReader fileReader;
        private const int MaxColumns = 6;

        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.Prenotification,
                };

                return x;
            }
        }

        public FileParsePrenotificationFileRule(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
        {
            MessageLevel result;

            try
            {
                var extension = Path.GetExtension(file.FileName);
                var isCsv = extension == ".csv" ? true : false;

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
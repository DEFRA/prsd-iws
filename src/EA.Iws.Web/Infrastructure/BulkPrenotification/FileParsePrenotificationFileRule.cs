namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Prsd.Core.Helpers;

    public class FileParsePrenotificationFileRule : IPrenotificationFileRule
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

        public async Task<RuleResult<PrenotificationFileRules>> GetResult(HttpPostedFileBase file)
        {
            MessageLevel result;

            try
            {
                var extension = Path.GetExtension(file.FileName);
                var isCsv = extension == ".csv";

                var dataTable = await fileReader.GetFirstDataTable(file, isCsv, !isCsv);

                result = IsDataTableValid(dataTable) ? MessageLevel.Success : MessageLevel.Error;

                DataTable = dataTable;
            }
            catch (Exception)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<PrenotificationFileRules>(PrenotificationFileRules.FileParse, result);
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

            var columns = EnumHelper.GetValues(typeof(PrenotificationColumnIndex));

            var maxColumns = columns != null ? columns.Count : MaxColumns;

            return dataTable.Columns.Count == maxColumns;
        }
    }
}
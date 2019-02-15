namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkUpload;
    using Core.Rules;
    using VirusScanning;

    public class BulkFileValidator : IBulkFileValidator
    {
        private readonly IVirusScanner virusScanner;
        private readonly IFileReader fileReader;
        private const int MaxColumns = 6;

        public DataTable DataTable { get; private set; }

        public byte[] FileBytes { get; private set; }

        public BulkFileValidator(IVirusScanner virusScanner,
            IFileReader fileReader)
        {
            this.virusScanner = virusScanner;
            this.fileReader = fileReader;
        }

        public async Task<BulkFileRulesSummary> GetFileRulesSummary(HttpPostedFileBase file, BulkFileType type)
        {
            var rules = new List<RuleResult<BulkFileRules>>
            {
                GetFileTypesRule(file, type),
                GetFileSizeRule(file),
                await GetVirusScan(file)
            };

            if (type != BulkFileType.SupportingDocument)
            {
                rules.Add(await GetFileParse(file));

                if (DataTable != null && DataTable.Rows.Count == 0)
                {
                    rules.Add(new RuleResult<BulkFileRules>(BulkFileRules.EmptyData, MessageLevel.Error));
                }
            }

            return new BulkFileRulesSummary()
            {
                FileRulesResults = rules,
                DataTable = DataTable,
                FileBytes = FileBytes
            };
        }

        private static RuleResult<BulkFileRules> GetFileTypesRule(HttpPostedFileBase file, BulkFileType type)
        {
            var allowedTypes = GetAllowedFileTypes(type);

            var extension = Path.GetExtension(file.FileName);

            var result = allowedTypes.Contains(extension) ? MessageLevel.Success : MessageLevel.Error;

            var rule = type == BulkFileType.SupportingDocument
                ? BulkFileRules.SupportingDocumentFileType
                : BulkFileRules.DataFileType;

            return new RuleResult<BulkFileRules>(rule, result);
        }

        private static RuleResult<BulkFileRules> GetFileSizeRule(HttpPostedFileBase file)
        {
            var result = file.ContentLength < int.MaxValue ? MessageLevel.Success : MessageLevel.Error;
            return new RuleResult<BulkFileRules>(BulkFileRules.FileSize, result);
        }

        private async Task<RuleResult<BulkFileRules>> GetVirusScan(HttpPostedFileBase file)
        {
            var result = MessageLevel.Success;
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();

                FileBytes = fileBytes;
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<BulkFileRules>(BulkFileRules.Virus, result);
        }

        private async Task<RuleResult<BulkFileRules>> GetFileParse(HttpPostedFileBase file)
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

            return new RuleResult<BulkFileRules>(BulkFileRules.FileParse, result);
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

            return dataTable.Columns.Count == MaxColumns;
        }

        private static IEnumerable<string> GetAllowedFileTypes(BulkFileType type)
        {
            if (type == BulkFileType.SupportingDocument)
            {
                return new[]
                {
                    MimeTypes.Bitmap,
                    MimeTypes.Gif,
                    MimeTypes.Jpeg,
                    MimeTypes.MSExcel,
                    MimeTypes.MSExcelXml,
                    MimeTypes.MSPowerPoint,
                    MimeTypes.MSPowerPointXml,
                    MimeTypes.MSWord,
                    MimeTypes.MSWordXml,
                    MimeTypes.OpenOfficePresentation,
                    MimeTypes.OpenOfficeSpreadsheet,
                    MimeTypes.OpenOfficeText,
                    MimeTypes.Pdf,
                    MimeTypes.Png
                };
            }

            return new[]
            {
                ".xlsx",
                ".xls",
                ".csv"
            };
        }
    }
}
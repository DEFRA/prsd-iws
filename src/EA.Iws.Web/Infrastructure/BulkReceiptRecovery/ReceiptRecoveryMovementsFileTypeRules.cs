namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public class ReceiptRecoveryMovementsFileTypeRules : IReceiptRecoveryFileRule
    {
        private readonly string[] allowedTypes;

        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.ShipmentMovementDocuments
                };

                return x;
            }
        }

        public ReceiptRecoveryMovementsFileTypeRules()
        {
            allowedTypes = new[]
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

        public async Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var result = allowedTypes.Contains(file.ContentType) ? MessageLevel.Success : MessageLevel.Error;

                return new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.FileTypeShipmentDocuments, result);
            });
        }
    }
}
namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using VirusScanning;

    public class VirusRule : IReceiptRecoveryFileRule
    {
        private readonly IVirusScanner virusScanner;

        public VirusRule(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.ReceiptRecovery,
                    FileUploadType.ShipmentMovementDocuments
                };

                return x;
            }
        }

        public async Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file)
        {
            var result = MessageLevel.Success;
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.Virus, result);
        }
    }
}
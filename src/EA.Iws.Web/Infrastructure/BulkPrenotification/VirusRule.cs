namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using VirusScanning;

    public class VirusRule : IBulkMovementPrenotificationFileRule
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
                    FileUploadType.Prenotification,
                    FileUploadType.ShipmentMovementDocuments
                };

                return x;
            }
        }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
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

            return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.Virus, result);
        }
    }
}

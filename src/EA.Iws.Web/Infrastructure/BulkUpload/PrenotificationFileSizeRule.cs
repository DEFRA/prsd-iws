namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationFileSizeRule : IBulkMovementPrenotificationFileRule
    {   
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
            return await Task.Run(() =>
            {
                var result = file.ContentLength < int.MaxValue ? MessageLevel.Success : MessageLevel.Error;
                return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileSize, result);
            });
        }
    }
}
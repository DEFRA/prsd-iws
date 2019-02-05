namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;

    public interface IBulkMovementPrenotificationFileRule
    {
        List<FileUploadType> UploadType { get; }
        DataTable DataTable { get; set; }
        Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file);
    }
}
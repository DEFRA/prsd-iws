namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public interface IBulkMovementPrenotificationFileRule
    {
        DataTable DataTable { get; set; }
        Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file);
    }
}
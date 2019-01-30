namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationFileSizeRule : IBulkMovementPrenotificationFileRule
    {
        public DataTable DataTable { get; set; }

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
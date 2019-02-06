namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;

    public class PrenotificationFileTypeRule : IPrenotificationFileRule
    {
        private readonly string[] allowedTypes;

        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.Prenotification
                };

                return x;
            }
        }

        public PrenotificationFileTypeRule()
        {
            allowedTypes = new[] 
            {
                ".xlsx",
                ".xls",
                ".csv"
            };
        }

        public async Task<RuleResult<PrenotificationFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var extension = Path.GetExtension(file.FileName);

                var result = allowedTypes.Contains(extension) ? MessageLevel.Success : MessageLevel.Error;

                return new RuleResult<PrenotificationFileRules>(PrenotificationFileRules.FileTypePrenotification, result);
            });
        }
    }
}
namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using BulkUpload;
    using Core.Movement.BulkPrenotification;
    using Core.Movement.BulkUpload;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    public class PrenotificationValidator : IPrenotificationValidator
    {
        private readonly IMediator mediator;
        private readonly IBulkFileValidator fileValidator;

        public PrenotificationValidator(IMediator mediator,
            IBulkFileValidator fileValidator)
        {
            this.mediator = mediator;
            this.fileValidator = fileValidator;
        }

        public async Task<PrenotificationRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var fileRulesSummary = await fileValidator.GetFileRulesSummary(file, BulkFileType.Prenotification);
            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv";

            var rulesSummary = new PrenotificationRulesSummary(fileRulesSummary.FileRulesResults);

            if (rulesSummary.IsFileRulesSuccess)
            {
                rulesSummary =
                    await
                        mediator.SendAsync(new PerformPrenotificationContentValidation(rulesSummary,
                            notificationId, fileRulesSummary.DataTable, file.FileName, isCsv));
            }

            return rulesSummary;
        }

        public async Task<BulkFileRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            return await fileValidator.GetFileRulesSummary(file, BulkFileType.SupportingDocument);
        }
    }
}
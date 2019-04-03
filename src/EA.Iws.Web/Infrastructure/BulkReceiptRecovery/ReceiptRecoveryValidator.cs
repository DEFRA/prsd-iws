namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using BulkUpload;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Movement.BulkUpload;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    public class ReceiptRecoveryValidator : IReceiptRecoveryValidator
    {
        private readonly IMediator mediator;
        private readonly IBulkFileValidator fileValidator;

        public ReceiptRecoveryValidator(IMediator mediator, 
            IBulkFileValidator fileValidator)
        {
            this.mediator = mediator;
            this.fileValidator = fileValidator;
        }

        public async Task<ReceiptRecoveryRulesSummary> GetValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var fileRulesSummary = await fileValidator.GetFileRulesSummary(file, BulkFileType.ReceiptRecovery);
            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv";

            var rulesSummary = new ReceiptRecoveryRulesSummary(fileRulesSummary.FileRulesResults);

            if (rulesSummary.IsFileRulesSuccess)
            {
                rulesSummary =
                    await
                        mediator.SendAsync(new PerformReceiptRecoveryContentValidation(rulesSummary,
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
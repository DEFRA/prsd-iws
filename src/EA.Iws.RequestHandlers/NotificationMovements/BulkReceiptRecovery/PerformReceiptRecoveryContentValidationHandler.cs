namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class PerformReceiptRecoveryContentValidationHandler : IRequestHandler<PerformReceiptRecoveryContentValidation, ReceiptRecoveryRulesSummary>
    {
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;

        public PerformReceiptRecoveryContentValidationHandler(IEnumerable<IReceiptRecoveryContentRule> contentRules,
            IMap<DataTable, List<ReceiptRecoveryMovement>> mapper)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
        }

        public async Task<ReceiptRecoveryRulesSummary> HandleAsync(PerformReceiptRecoveryContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            var movements = mapper.Map(message.DataTable);

            var addFirstRowWarningRule = message.IsCsv && !CheckFirstRow(movements);

            result.ContentRulesResults = await GetOrderedContentRules(movements, message.NotificationId, addFirstRowWarningRule);

            if (result.IsContentRulesSuccess)
            {
                result.ShipmentNumbers =
                    movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value);

                //result.DraftBulkUploadId = await repository.Add(message.NotificationId, movements, message.FileName);
            }

            return result;
        }

        private async Task<List<ContentRuleResult<ReceiptRecoveryContentRules>>> GetOrderedContentRules(List<ReceiptRecoveryMovement> movements,
            Guid notificationId, bool addFirstRowWarningRule)
        {
            var rules = new List<ContentRuleResult<ReceiptRecoveryContentRules>>();

            if (addFirstRowWarningRule)
            {
                rules.Add(new ContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.HeaderDataRemoved,
                    MessageLevel.Warning,
                    Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.HeaderDataRemoved)));
            }

            var missingData = await GetMissingDataResult(movements, notificationId);

            rules.Add(missingData);

            // Only run rest of validations if there are no missing/blank data.
            if (missingData.MessageLevel == MessageLevel.Success)
            {
                foreach (var rule in contentRules)
                {
                    rules.Add(await rule.GetResult(movements, notificationId));
                }
            }

            return rules.OrderBy(r => r.Rule).ToList();
        }

        private async Task<ContentRuleResult<ReceiptRecoveryContentRules>> GetMissingDataResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (movement.ShipmentNumber.HasValue &&
                        (movement.MissingNotificationNumber ||
                        movement.MissingReceivedDate ||
                        movement.MissingQuantity ||
                        movement.MissingUnits ||
                        movement.MissingRecoveredDisposedDate))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", missingDataShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.MissingData), shipmentNumbers);

                return new ContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MissingData, missingDataResult, errorMessage);
            });
        }

        private static bool CheckFirstRow(IList<ReceiptRecoveryMovement> movements)
        {
            if (!IsValidNotificationNumber(movements[(int)ReceiptRecoveryColumnIndex.NotificationNumber].NotificationNumber))
            {
                movements.RemoveAt(0);
                return false;
            }

            return true;
        }

        private static bool IsValidNotificationNumber(string input)
        {
            var match = Regex.Match(input.Replace(" ", string.Empty), @"(GB)(\d{4})(\d{6})");

            return match.Success;
        }
    }
}
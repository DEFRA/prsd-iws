namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class PerformReceiptRecoveryContentValidationHandler : IRequestHandler<PerformReceiptRecoveryContentValidation, ReceiptRecoveryRulesSummary>
    {
        private readonly IEnumerable<IReceiptRecoveryContentRule> contentRules;
        private readonly IMap<DataTable, List<ReceiptRecoveryMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private const int MaxShipments = 50;

        public PerformReceiptRecoveryContentValidationHandler(IEnumerable<IReceiptRecoveryContentRule> contentRules,
            IMap<DataTable, List<ReceiptRecoveryMovement>> mapper,
            IDraftMovementRepository repository)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ReceiptRecoveryRulesSummary> HandleAsync(PerformReceiptRecoveryContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            var movements = mapper.Map(message.DataTable);

            result.ContentRulesResults = await GetOrderedContentRules(movements, message.NotificationId);

            result.ShipmentNumbers =
                    movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value);

            if (result.IsContentRulesSuccess)
            { 
                result.DraftBulkUploadId = await repository.AddReceiptRecovery(message.NotificationId, movements, message.FileName);
            }

            return result;
        }

        private async Task<List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>> GetOrderedContentRules(List<ReceiptRecoveryMovement> movements,
            Guid notificationId)
        {
            var rules = new List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>()
            {
                await GetMaxShipments(movements)
            };

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            rules.Add(await GetMissingNotificationNumbersOrShipmentNumbers(movements));

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            rules.Add(await GetMissingReceiptDataResult(movements, notificationId));

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(movements, notificationId));
            }

            return rules.OrderBy(r =>
            {
                var shipments = r.ErrorMessage.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                return shipments;
            }).ThenBy(r => r.Rule).ToList();
        }

        private static async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMaxShipments(
            IReadOnlyCollection<ReceiptRecoveryMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Count > MaxShipments ? MessageLevel.Error : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.MaximumShipments),
                        MaxShipments);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MaximumShipments,
                    result, errorMessage);
            });
        }

        private static async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMissingNotificationNumbersOrShipmentNumbers(
            IReadOnlyCollection<ReceiptRecoveryMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Any(m => m.MissingShipmentNumber || !m.ShipmentNumber.HasValue 
                    || m.MissingNotificationNumber || string.IsNullOrEmpty(m.NotificationNumber))
                    ? MessageLevel.Error
                    : MessageLevel.Success;

                var errorMessage = Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.InvalidNotificationOrShipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.InvalidNotificationOrShipmentNumbers,
                    result, errorMessage);
            });
        }

        private async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetMissingReceiptDataResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if record has a shipment number, otherwise record will be picked up by the GetMissingShipmentNumbers method
                    // Only report an error if record has a notification number, otherwise record will be picked up by the GetMissingNotificationNumbers method
                    if (movement.ShipmentNumber.HasValue &&
                        !string.IsNullOrEmpty(movement.NotificationNumber) &&
                        ((movement.MissingReceivedDate && movement.MissingRecoveredDisposedDate) ||
                        movement.MissingQuantity ||
                        movement.MissingUnits))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", missingDataShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.MissingReceiptData), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.MissingReceiptData, missingDataResult, errorMessage);
            });
        }
    }
}
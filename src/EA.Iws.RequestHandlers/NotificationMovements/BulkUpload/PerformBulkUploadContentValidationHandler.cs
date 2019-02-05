namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private const int MaxShipments = 50;

        public PerformBulkUploadContentValidationHandler(IEnumerable<IBulkMovementPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper,
            IDraftMovementRepository repository)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            var movements = mapper.Map(message.DataTable);

            result.ContentRulesResults = await GetOrderedContentRules(movements, message.NotificationId);

            if (result.IsContentRulesSuccess)
            {
                result.ShipmentNumbers =
                    movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value);

                result.DraftBulkUploadId = await repository.Add(message.NotificationId, movements, message.FileName);
            }

            return result;
        }

        private async Task<List<ContentRuleResult<BulkMovementContentRules>>> GetOrderedContentRules(List<PrenotificationMovement> movements, 
            Guid notificationId)
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            var maxShipments = await GetMaxShipments(movements);
            
            rules.Add(maxShipments);

            if (maxShipments.MessageLevel == MessageLevel.Success)
            {
                var missingNotificationShipment = await GetMissingNotificationShipmentNumbers(movements);

                rules.Add(missingNotificationShipment);

                if (missingNotificationShipment.MessageLevel == MessageLevel.Success)
                {
                    var missingData = await GetMissingDataResult(movements);

                    rules.Add(missingData);

                    // Only run rest of validations if there are no missing/blank data.
                    if (missingData.MessageLevel == MessageLevel.Success)
                    {
                        foreach (var rule in contentRules)
                        {
                            rules.Add(await rule.GetResult(movements, notificationId));
                        }
                    }
                }
            }

            return rules.OrderBy(r => r.Rule).ToList();
        }

        private static async Task<ContentRuleResult<BulkMovementContentRules>> GetMaxShipments(
            IReadOnlyCollection<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Count > MaxShipments ? MessageLevel.Error : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MaximumShipments),
                        MaxShipments);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MaximumShipments,
                    result, errorMessage);
            });
        }

        private static async Task<ContentRuleResult<BulkMovementContentRules>> GetMissingNotificationShipmentNumbers(
            IReadOnlyCollection<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Any(m => m.MissingNotificationNumber
                                                || m.MissingShipmentNumber
                                                || !m.ShipmentNumber.HasValue)
                    ? MessageLevel.Error
                    : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MissingShipmentNumbers),
                        movements.Count);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingShipmentNumbers,
                    result, errorMessage);
            });
        }

        private static async Task<ContentRuleResult<BulkMovementContentRules>> GetMissingDataResult(
            List<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                (m.MissingQuantity || m.MissingUnits || m.MissingPackagingTypes ||
                                 m.MissingDateOfShipment))
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage =
                    string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MissingData),
                        shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData,
                    result, errorMessage);
            });
        }

        /*private static bool CheckFirstRow(IList<PrenotificationMovement> movements)
        {
            if (!IsValidNotificationNumber(movements[(int)PrenotificationColumnIndex.NotificationNumber].NotificationNumber))
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
        }*/
    }
}

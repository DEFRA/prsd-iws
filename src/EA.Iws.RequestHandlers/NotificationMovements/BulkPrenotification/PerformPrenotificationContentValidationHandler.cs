namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class PerformPrenotificationContentValidationHandler : IRequestHandler<PerformPrenotificationContentValidation, PrenotificationRulesSummary>
    {
        private readonly IEnumerable<IPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;
        private const int MaxShipments = 50;

        public PerformPrenotificationContentValidationHandler(IEnumerable<IPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper,
            IDraftMovementRepository repository)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<PrenotificationRulesSummary> HandleAsync(PerformPrenotificationContentValidation message)
        {
            var result = message.RulesSummary;

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

        private async Task<List<PrenotificationContentRuleResult<PrenotificationContentRules>>> GetOrderedContentRules(List<PrenotificationMovement> movements, 
            Guid notificationId)
        {
            var rules = new List<PrenotificationContentRuleResult<PrenotificationContentRules>>
            {
                await GetMaxShipments(movements)
            };

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            rules.Add(await GetMissingNotificationShipmentNumbers(movements));

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            rules.Add(await GetMissingDataResult(movements));

            if (rules.Any(r => r.MessageLevel == MessageLevel.Error))
            {
                return rules;
            }

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(movements, notificationId));
            }

            return rules.OrderBy(r => r.Rule).ToList();
        }

        private static async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetMaxShipments(
            IReadOnlyCollection<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Count > MaxShipments ? MessageLevel.Error : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.MaximumShipments),
                        MaxShipments);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MaximumShipments,
                    result, errorMessage);
            });
        }

        private static async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetMissingNotificationShipmentNumbers(
            IReadOnlyCollection<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result =
                    movements.Any(m => m.MissingNotificationNumber 
                                       || string.IsNullOrWhiteSpace(m.NotificationNumber)
                                       || m.MissingShipmentNumber
                                       || !m.ShipmentNumber.HasValue)
                        ? MessageLevel.Error
                        : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.MissingShipmentNumbers),
                        movements.Count);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingShipmentNumbers,
                    result, errorMessage);
            });
       }

        private static async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetMissingDataResult(
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
                    string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.MissingData),
                        shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingData,
                    result, errorMessage);
            });
        }
    }
}

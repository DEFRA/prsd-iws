namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    internal class PerformPrenotificationContentValidationHandler : IRequestHandler<PerformPrenotificationContentValidation, PrenotificationRulesSummary>
    {
        private readonly IEnumerable<IPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository draftRepository;
        private readonly IMovementRepository movementRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        private const int MaxShipments = 50;
        private const PrenotificationContentRules LastRule = PrenotificationContentRules.ThreeWorkingDaysToShipment;

        public PerformPrenotificationContentValidationHandler(IEnumerable<IPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper,
            IDraftMovementRepository draftRepository,
            IMovementRepository movementRepository,
            IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
            this.draftRepository = draftRepository;
            this.movementRepository = movementRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
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

                result.DraftBulkUploadId = await draftRepository.AddPrenotifications(message.NotificationId, movements, message.FileName);
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

            rules.AddRange(await GetActiveLoadsRule(movements, notificationId));

            var lastRuleResult = rules.FirstOrDefault(r => r.Rule == LastRule);

            var orderedRules =
                rules.Where(r => r.Rule != LastRule).OrderBy(r => r.MinShipmentNumber).ThenBy(r =>
                {
                    var shipments = r.ErrorMessage.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    return shipments;
                }).ThenBy(r => r.Rule).ToList();

            if (lastRuleResult != null)
            {
                orderedRules.Add(lastRuleResult);
            }

            return orderedRules;
        }

        public async Task<IEnumerable<PrenotificationContentRuleResult<PrenotificationContentRules>>> GetActiveLoadsRule(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var rules = new List<PrenotificationContentRuleResult<PrenotificationContentRules>>();

            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(notificationId);
            var currentFinancialGuarantee =
                financialGuaranteeCollection.FinancialGuarantees.SingleOrDefault(
                    fg => fg.Status == FinancialGuaranteeStatus.Approved);
            var activeLoadsPermitted = currentFinancialGuarantee == null ? 0 : currentFinancialGuarantee.ActiveLoadsPermitted.GetValueOrDefault();

            var newShipmentsGroupedByDate =
                movements.Where(m => m.ActualDateOfShipment.HasValue && m.ShipmentNumber.HasValue)
                    .GroupBy(m => m.ActualDateOfShipment.Value)
                    .Where(g => g.Count() > activeLoadsPermitted)
                    .Select(
                        m =>
                            new
                            {
                                Date = m.Key,
                                ShipmentCount = m.Select(mm => mm.ShipmentNumber).Count(),
                                MinShipmentNumber = m.Min(mm => mm.ShipmentNumber),
                                Shipments = string.Join(", ", m.OrderBy(mm => mm.ShipmentNumber).Select(mm => mm.ShipmentNumber))
                            })
                    .ToList();

            if (newShipmentsGroupedByDate.Any())
            {
                newShipmentsGroupedByDate.ForEach(dateOfShipment =>
                {
                    var validationMessage =
                        string.Format(EnumHelper.GetDisplayName(PrenotificationContentRules.ActiveLoadsDataShipments),
                            dateOfShipment.Shipments, dateOfShipment.ShipmentCount, activeLoadsPermitted);
                    var rule =
                        new PrenotificationContentRuleResult<PrenotificationContentRules>(
                            PrenotificationContentRules.ActiveLoadsDataShipments, MessageLevel.Error, validationMessage,
                            dateOfShipment.MinShipmentNumber.Value);

                    rules.Add(rule);
                });

                return rules;
            }

            var existingMovements = await movementRepository.GetFutureActiveMovements(notificationId);

            var newAndExistingGroupedByDate =
                movements.Where(m => m.ActualDateOfShipment.HasValue && m.ShipmentNumber.HasValue)
                    .GroupBy(m => m.ActualDateOfShipment.Value)
                    .Where(m => m.Count() + existingMovements.Count(mm => mm.Date.Date == m.Key) > activeLoadsPermitted)
                    .Select(
                        m =>
                            new
                            {
                                Date = m.Key,
                                ExistingShipmentCount = existingMovements.Count(mm => mm.Date.Date == m.Key),
                                NewShipmentsCount = m.Select(mm => mm.ShipmentNumber).Count(),
                                MinNewShipmentNumber = m.Min(mm => mm.ShipmentNumber),
                                NewShipments =
                                string.Join(", ", m.OrderBy(mm => mm.ShipmentNumber).Select(mm => mm.ShipmentNumber))
                            })
                    .ToList();

            newAndExistingGroupedByDate.ForEach(dateOfShipment =>
            {
                var validationMessage =
                    string.Format(EnumHelper.GetDisplayName(PrenotificationContentRules.ActiveLoadsWithExistingShipments),
                        dateOfShipment.NewShipments, dateOfShipment.ExistingShipmentCount, dateOfShipment.NewShipmentsCount, activeLoadsPermitted);
                var rule =
                    new PrenotificationContentRuleResult<PrenotificationContentRules>(
                        PrenotificationContentRules.ActiveLoadsWithExistingShipments, MessageLevel.Error, validationMessage,
                        dateOfShipment.MinNewShipmentNumber.Value);

                rules.Add(rule);
            });

            return rules;
        }

        private static async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetMaxShipments(
            IReadOnlyCollection<PrenotificationMovement> movements)
        {
            return await Task.Run(() =>
            {
                var result = movements.Count > MaxShipments ? MessageLevel.Error : MessageLevel.Success;

                var errorMessage =
                    string.Format(
                        EnumHelper.GetDisplayName(PrenotificationContentRules.MaximumShipments),
                        MaxShipments);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MaximumShipments,
                    result, errorMessage, 0);
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
                        EnumHelper.GetDisplayName(PrenotificationContentRules.MissingShipmentNumbers),
                        movements.Count);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingShipmentNumbers,
                    result, errorMessage, 0);
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
                        .OrderBy(m => m.ShipmentNumber)
                        .Select(m => m.ShipmentNumber)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage =
                    string.Format(EnumHelper.GetDisplayName(PrenotificationContentRules.MissingData),
                        shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.MissingData,
                    result, errorMessage, minShipment);
            });
        }
    }
}

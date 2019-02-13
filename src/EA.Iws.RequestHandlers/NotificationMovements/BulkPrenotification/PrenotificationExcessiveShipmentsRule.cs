namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.FinancialGuarantee;
    using Domain.Movement;

    public class PrenotificationExcessiveShipmentsRule : IPrenotificationContentRule
    {
        private readonly IMovementRepository movementRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public PrenotificationExcessiveShipmentsRule(IMovementRepository movementRepository,
            IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.movementRepository = movementRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(notificationId);
            var currentFinancialGuarantee =
                financialGuaranteeCollection.FinancialGuarantees.SingleOrDefault(
                    fg => fg.Status == FinancialGuaranteeStatus.Approved);
            var activeLoadsPermitted = currentFinancialGuarantee == null ? 0 : currentFinancialGuarantee.ActiveLoadsPermitted.GetValueOrDefault();

            var groupedByDateOfShipment =
                movements.Where(m => m.ActualDateOfShipment.HasValue)
                    .GroupBy(m => m.ActualDateOfShipment.Value)
                    .Where(g => g.Count() > activeLoadsPermitted)
                    .Select(m => m.Key);

            var result = groupedByDateOfShipment.Any() ? MessageLevel.Error : MessageLevel.Success;

            var errorMessage =
                string.Format(
                    Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.ActiveLoadsGrouped),
                    movements.Count, activeLoadsPermitted);

            if (result == MessageLevel.Error)
            {
                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.ActiveLoadsGrouped, result, errorMessage);
            }

            var currentActiveLoads = (await movementRepository.GetActiveMovements(notificationId)).Count();
            var remainingShipments = activeLoadsPermitted - currentActiveLoads;

            result = remainingShipments < movements.Count ? MessageLevel.Error : MessageLevel.Success;
            errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.ExcessiveShipments), movements.Count, remainingShipments);

            return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.ExcessiveShipments, result, errorMessage);
        }
    }
}
namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentOnlyNewShipmentsRule : IBulkMovementPrenotificationContentRule
    {
        private readonly IMovementRepository repo;

        public PrenotificationContentOnlyNewShipmentsRule(IMovementRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var existingMovements = await this.repo.GetAllMovements(notificationId);

            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;

                int firstNumber = movements.OrderBy(p => p.ShipmentNumber).First().ShipmentNumber.GetValueOrDefault();

                if (firstNumber < existingMovements.Count())
                {
                    missingDataResult = MessageLevel.Error;
                }

                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.OnlyNewShipments), firstNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.OnlyNewShipments, missingDataResult, errorMessage);
            });
        }
    }
}
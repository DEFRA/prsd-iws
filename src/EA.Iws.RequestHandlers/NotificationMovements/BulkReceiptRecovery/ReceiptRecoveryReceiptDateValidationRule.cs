namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;

    public class ReceiptRecoveryReceiptDateValidationRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository repository;

        public ReceiptRecoveryReceiptDateValidationRule(IMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments = new List<int>();
                var existingMovements = repository.GetAllMovements(notificationId).Result;

                foreach (ReceiptRecoveryMovement movement in movements)
                {
                    if (movement.ShipmentNumber.HasValue && movement.ReceivedDate.HasValue)
                    {
                        try
                        {
                            var existingMovement = existingMovements.Where(m => m.Number == movement.ShipmentNumber).Single();
                            if (movement.ReceivedDate > DateTime.UtcNow || movement.ReceivedDate < existingMovement.Date)
                            {
                                shipments.Add(movement.ShipmentNumber.Value);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            // if there is no existing movement catch the error and ignore
                        }
                    }
                }

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceiptDateValidation), shipmentNumbers);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceiptDateValidation, result, errorMessage);
            });
        }
    }
}

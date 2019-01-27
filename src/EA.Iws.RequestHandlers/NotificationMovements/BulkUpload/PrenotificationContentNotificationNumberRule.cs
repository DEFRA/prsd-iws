namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class PrenotificationContentNotificationNumberRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationContentNotificationNumberRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
            return await Task.Run(() =>
            {
                var notificationNumberResult = MessageLevel.Success;
                var notificationNumberShipmentNumbers = new List<string>();
                
                foreach (PrenotificationMovement shipment in shipments)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (shipment.HasShipmentNumber)
                    {
                        if (!shipment.NotificationNumber.Trim().ToLower().Equals(notificationNumber))
                        {
                                notificationNumberResult = MessageLevel.Error;
                                notificationNumberShipmentNumbers.Add(shipment.ShipmentNumber);
                        }
                    }
                }

                var errorMessage = GenerateErrorMessage(notificationNumberShipmentNumbers, notificationNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.WrongNotificationNumber, notificationNumberResult, errorMessage);
            });
        }

        private string GenerateErrorMessage(List<string> shipmentNumbers, string notificationNumber)
        {
            var errorArg1 = string.Join(", ", shipmentNumbers);
                        
            return string.Format("Shipment number/s {0}: data must only be for notification number {1}", errorArg1, notificationNumber);
        }
    }
}

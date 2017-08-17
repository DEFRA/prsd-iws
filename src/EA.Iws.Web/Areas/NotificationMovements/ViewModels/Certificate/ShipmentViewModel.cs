namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Certificate
{
    using Core.Movement;
    using Core.MovementOperation;
    using Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    public class ShipmentViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public ShipmentViewModel()
        {
        }

        public NotificationType NotificationType { get; set; }

        public CertificateType Certificate { get; set; }

        public IList<SelectShipmentViewModel> ReceiveShipments { get; set; }

        public IList<SelectShipmentViewModel> RecoveryShipments { get; set; }
        public ShipmentViewModel(Guid id, NotificationType notificationType, CertificateType certificate, IEnumerable<MovementData> receiveModel)
        {
            ReceiveShipments = receiveModel
               .OrderBy(d => d.Number)
               .Select(d => new SelectShipmentViewModel
               {
                   DisplayName = "Shipment " + d.Number,
                   Id = d.Id,
                   IsSelected = false
               }).ToArray();
            NotificationId = id;
            NotificationType = notificationType;
            Certificate = certificate;
        }

        public ShipmentViewModel(Guid id, NotificationType notificationType, CertificateType certificate, MovementOperationData recoveryModel)
        {
            var list = recoveryModel.MovementDatas;
            RecoveryShipments = list
               .OrderBy(d => d.Number)
               .Select(d => new SelectShipmentViewModel
               {
                   DisplayName = "Shipment " + d.Number,
                   Id = d.Id,
                   IsSelected = false
               }).ToArray();
            NotificationId = id;
            NotificationType = notificationType;
            Certificate = certificate;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((ReceiveShipments != null && !ReceiveShipments.Any(s => s.IsSelected)) || (RecoveryShipments != null && !RecoveryShipments.Any(s => s.IsSelected)))
            {
                yield return new ValidationResult(ShipmentViewModelResources.ShipmentRequired);
            }
        }

        public class SelectShipmentViewModel
        {
            public string DisplayName { get; set; }

            public Guid Id { get; set; }

            public bool IsSelected { get; set; }
        }
    }   
}
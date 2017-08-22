namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Certificate
{
    using Core.Movement;
    using Core.MovementOperation;
    using Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Web.ViewModels.Shared;

    public class ShipmentViewModel 
    {
        public Guid NotificationId { get; set; }

        public ShipmentViewModel()
        {
        }

        public NotificationType NotificationType { get; set; }

        public CertificateType Certificate { get; set; }

        public StringGuidRadioButtons ReceiveShipments { get; set; }

        public StringGuidRadioButtons RecoveryShipments { get; set; }

        public ShipmentViewModel(Guid id, NotificationType notificationType, CertificateType certificate, IEnumerable<MovementData> receiveModel)
        {
            ReceiveShipments = new StringGuidRadioButtons(receiveModel
               .OrderBy(d => d.Number)
               .Select(d => new KeyValuePair<string, Guid>("Shipment " + d.Number, d.Id)));
           
            NotificationId = id;
            NotificationType = notificationType;
            Certificate = certificate;
        }

        public ShipmentViewModel(Guid id, NotificationType notificationType, CertificateType certificate, MovementOperationData recoveryModel)
        {
            var list = recoveryModel.MovementDatas;
            RecoveryShipments = new StringGuidRadioButtons(list
               .OrderBy(d => d.Number)
               .Select(d => new KeyValuePair<string, Guid>("Shipment " + d.Number, d.Id)));
           
            NotificationId = id;
            NotificationType = notificationType;
            Certificate = certificate;
        }
    }   
}
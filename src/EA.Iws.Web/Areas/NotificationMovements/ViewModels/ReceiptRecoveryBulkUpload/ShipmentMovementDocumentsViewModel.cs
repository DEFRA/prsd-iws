namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.ReceiptRecoveryBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Core.Shared;
    using Views.ReceiptRecoveryBulkUpload;

    public class ShipmentMovementDocumentsViewModel : IValidatableObject
    {
        public ShipmentMovementDocumentsViewModel()
        {
            this.Shipments = new List<int>();
        }

        public ShipmentMovementDocumentsViewModel(Guid notificationId, IEnumerable<int> shipments, string receiptRecoveryFileName, NotificationType type)
        {
            this.NotificationId = notificationId;
            this.Shipments = shipments;
            this.ReceiptRecoveryFileName = receiptRecoveryFileName;
            this.NotificationType = type;
        }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public IEnumerable<int> Shipments { get; set; }

        public string ReceiptRecoveryFileName { get; set; }

        public string ShipmentMovementFileName { get; set; }

        public string FileSuccessMessage
        {
            get
            {
                return ReceiptRecoveryBulkUploadResources.ShipmentMovementsSuccessText.Replace("{filename}",
                    this.ReceiptRecoveryFileName);
            }
        }

        [Display(Name = "Upload the file containing your shipment movement documents")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Upload the file containing your shipment movement documents", new[] { "File" });
            }
        }

        public string GetShipments
        {
            get
            {
                var returnString = string.Empty;
                if (Shipments != null)
                {
                    for (var i = 0; i < Shipments.Count(); i++)
                    {
                        returnString += Shipments.ElementAt(i).ToString() + ", ";
                        if (i == Shipments.Count() - 2)
                        {
                            if (Shipments.Count() == 2)
                            {
                                returnString = returnString.Trim().TrimEnd(',');
                            }
                            returnString = string.Concat(returnString.Trim(), " and ");
                        }
                    }
                }

                return returnString.Trim().TrimEnd(',');
            }
        }
    }
}
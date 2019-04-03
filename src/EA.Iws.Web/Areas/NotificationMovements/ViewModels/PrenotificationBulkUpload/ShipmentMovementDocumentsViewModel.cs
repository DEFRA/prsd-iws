namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.PrenotificationBulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Views.PrenotificationBulkUpload;

    public class ShipmentMovementDocumentsViewModel : IValidatableObject
    {
        public ShipmentMovementDocumentsViewModel()
        {
            this.Shipments = new List<int>();
        }

        public ShipmentMovementDocumentsViewModel(Guid notificationId, IEnumerable<int> shipments, string preNotificationFileName)
        {
            this.NotificationId = notificationId;
            this.Shipments = shipments;
            this.PreNotificationFileName = preNotificationFileName;
        }

        public Guid NotificationId { get; set; }

        public IEnumerable<int> Shipments { get; set; }

        public string PreNotificationFileName { get; set; }

        public string ShipmentMovementFileName { get; set; }

        public string FileSuccessMessage
        {
            get
            {
                return PrenotificationBulkUploadResources.ShipmentMovementsSuccessText.Replace("{filename}",
                    PreNotificationFileName);
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
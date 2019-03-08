namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Core.Admin;
    using Prsd.Core.Helpers;

    public class AddCommentsViewModel : IValidatableObject
    {
        public AddCommentsViewModel()
        {
            CommentTypes = new SelectList(EnumHelper.GetValues(typeof(NotificationShipmentsCommentsType)), dataTextField: "Value", dataValueField: "Key");
            this.ModelIsValid = true;
        }

        public Guid NotificationId { get; set; }

        [DisplayName("Type of comment")]
        public NotificationShipmentsCommentsType? SelectedType { get; set; }

        public SelectList CommentTypes { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Shipment number")]
        public string ShipmentNumberStr { get; set; }

        public int? ShipmentNumber
        {
            get
            {
                if (this.SelectedType == NotificationShipmentsCommentsType.Shipments)
                {
                    int number;
                    int.TryParse(this.ShipmentNumberStr, out number);
                    return number;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool ModelIsValid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.SelectedType == null)
            {
                yield return new ValidationResult("Select a category from the list", new[] { "SelectedType" });
            }
            else
            {
                if (this.SelectedType == NotificationShipmentsCommentsType.Shipments)
                {
                    bool shipmentNumberIsValid = this.ShipmentNumber == null || this.ShipmentNumber < 1 || this.ShipmentNumber.ToString().Length > 6 ? false : true;
                    if (!shipmentNumberIsValid)
                    {
                        yield return new ValidationResult("Enter a valid shipment number", new[] { "ShipmentNumberStr" });
                    }
                }

                if (this.Comment == null)
                {
                    yield return new ValidationResult("Enter a comment", new[] { "Comment" });
                }
            }
        }
    }
}
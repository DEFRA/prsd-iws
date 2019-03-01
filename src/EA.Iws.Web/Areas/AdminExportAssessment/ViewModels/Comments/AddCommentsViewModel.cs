namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
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

        [DisplayName("Shipment Type")]
        public int? ShipmentNumber { get; set; }

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
                    bool shipmentNumberIsValid = this.ShipmentNumber == null || this.ShipmentNumber > 999999 ? false : true;
                    if (!shipmentNumberIsValid)
                    {
                        yield return new ValidationResult("Enter a valid shipment number", new[] { "ShipmentNumber" });
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
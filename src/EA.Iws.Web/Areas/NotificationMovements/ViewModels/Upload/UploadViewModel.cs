namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Upload
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Core.Movement;
    using Infrastructure.Validation;

    public class UploadViewModel : IValidatableObject
    {
        public UploadViewModel()
        {
        }

        public UploadViewModel(IEnumerable<MovementInfo> movements)
        {
            var movementInfos = movements as MovementInfo[] ?? movements.ToArray();

            MovementIds = movementInfos.Select(m => m.Id);
            ShipmentNumbers = string.Join(", ", movementInfos.Select(m => m.ShipmentNumber));
        }

        public IEnumerable<Guid> MovementIds { get; set; }

        public string ShipmentNumbers { get; set; }

        [Display(Name = "Upload the signed copy of the prenotification document")]
        [RestrictToAllowedUploadTypes]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (File == null || File.InputStream.Length == 0)
            {
                yield return new ValidationResult("Please upload the signed copy of the prenotification document", new[] { "File" });
            }
        }
    }
}
namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.UploadChoice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Movement;

    public class UploadChoiceViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public IList<SelectShipmentViewModel> Shipments { get; set; }

        public UploadChoiceViewModel()
        {
        }

        public UploadChoiceViewModel(Guid id, IEnumerable<MovementData> model)
        {
            Shipments = model
                .OrderBy(d => d.Number)
                .Select(d => new SelectShipmentViewModel
                {
                    DisplayName = "Shipment " + d.Number,
                    Id = d.Id,
                    IsSelected = false
                }).ToArray();
            NotificationId = id;
        }

        public bool NoMovementsToList
        {
            get { return Shipments == null || !Shipments.Any(); }
        }

        public class SelectShipmentViewModel
        {
            public string DisplayName { get; set; }

            public Guid Id { get; set; }

            public bool IsSelected { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Shipments.Any(s => s.IsSelected))
            {
                yield return new ValidationResult(UploadChoiceViewModelResources.ShipmentRequired, new[] { "Shipments" });
            }
        }
    }
}
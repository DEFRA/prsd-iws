namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportMovement;

    public class CancellableMovementsViewModel : IValidatableObject
    {
        public CancellableMovementsViewModel()
        {
        }

        public CancellableMovementsViewModel(IEnumerable<ImportCancellableMovement> cancellableMovements)
        {
            CancellableMovements = new List<CancellableMovementViewModel>(
                cancellableMovements.Select(x => new CancellableMovementViewModel
                {
                    MovementId = x.MovementId,
                    Number = x.Number,
                    ActualShipmentDate = x.ActualShipmentDate,
                    PrenotificationDate = x.PrenotificationDate
                }));
        }

        public IList<CancellableMovementViewModel> CancellableMovements { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public class CancellableMovementViewModel
        {
            public Guid MovementId { get; set; }

            public int Number { get; set; }

            public bool IsSelected { get; set; }

            public DateTime ActualShipmentDate { get; set; }

            public DateTime? PrenotificationDate { get; set; }
        }
    }
}
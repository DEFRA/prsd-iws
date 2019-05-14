namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Cancel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportMovement;
    using Core.Movement;

    public class CancellableMovementsViewModel : IValidatableObject
    {
        public CancellableMovementsViewModel()
        {
            CancellableMovements = new List<CancellableMovementViewModel>();
            AddedMovements = new List<AddedCancellableMovement>();
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
                }).OrderBy(x => x.Number));
        }

        public IList<CancellableMovementViewModel> CancellableMovements { get; set; }

        public IList<AddedCancellableMovement> AddedMovements { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!CancellableMovements.Any(m => m.IsSelected))
            {
                yield return new ValidationResult(CancelResources.SelectPrenotificationsToCancel, new[] { "CancellableMovements" });
            }
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
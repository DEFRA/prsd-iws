namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Views.Shared;

    public class EnterWasteCodesViewModel : IValidatableObject
    {
        public IList<WasteCodeViewModel> WasteCodes { get; set; }

        public Guid? SelectedCode { get; set; }

        public List<Guid> SelectedWasteCodes { get; set; }

        [Display(Name = "Not applicable")]
        public bool IsNotApplicable { get; set; }

        public SelectList Codes
        {
            get
            {
                return new SelectList(WasteCodes, "Id", "Name", SelectedCode);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedCode.HasValue && SelectedWasteCodes == null || !SelectedWasteCodes.Any() && !IsNotApplicable)
            {
                yield return new ValidationResult("Please enter a code or select not applicable", new[] { "SelectedCode" });
            }
        }
    }
}
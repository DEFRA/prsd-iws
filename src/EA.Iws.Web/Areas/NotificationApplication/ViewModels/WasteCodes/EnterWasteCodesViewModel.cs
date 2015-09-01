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
        private string validationMessage = "Please enter a code or select not applicable";
        public virtual string ValidationMessage
        {
            get { return validationMessage; }
            set { validationMessage = value; }
        }

        public IList<WasteCodeViewModel> WasteCodes { get; set; }

        public Guid? SelectedCode { get; set; }

        public List<Guid> SelectedWasteCodes { get; set; }

        [Display(Name = "Not applicable")]
        public virtual bool IsNotApplicable { get; set; }

        public SelectList Codes
        {
            get
            {
                return new SelectList(WasteCodes.OrderBy(wc => wc.Name), "Id", "Name", SelectedCode);
            }
        }

        public EnterWasteCodesViewModel()
        {
            SelectedWasteCodes = new List<Guid>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SelectedCode.HasValue && SelectedWasteCodes.Count == 0 && !IsNotApplicable)
            {
                yield return new ValidationResult(this.ValidationMessage, new[] { "SelectedCode" });
            }

            if (IsNotApplicable && SelectedWasteCodes != null && SelectedWasteCodes.Count > 0 && !SelectedCode.HasValue)
            {
                yield return new ValidationResult("Do not select not applicable where you have also selected codes", new[] { "IsNotApplicable" });
            }

            if (IsNotApplicable && SelectedCode.HasValue)
            {
                yield return new ValidationResult("Do not select both not applicable and a code", new[] { "IsNotApplicable" });
            }
        }
    }
}
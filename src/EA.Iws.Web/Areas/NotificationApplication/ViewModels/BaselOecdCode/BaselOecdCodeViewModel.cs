namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.BaselOecdCode
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Views.Shared;

    public class BaselOecdCodeViewModel : IValidatableObject
    {
        public Guid? SelectedCode { get; set; }

        public IList<WasteCodeViewModel> WasteCodes { get; set; }

        public SelectList Codes
        {
            get
            {
                return new SelectList(WasteCodes, "Id", "Name", SelectedCode);
            }
        }

        [Display(Name = "Not listed")]
        public bool NotListed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NotListed && !SelectedCode.HasValue)
            {
                yield return new ValidationResult("You must select either not listed or choose a code", new[] { "NotListed" });
            }

            if (NotListed && SelectedCode.HasValue)
            {
                yield return new ValidationResult("Do not select Not listed where you have also entered a code", new[] { "NotListed" });
            }
        }
    }
}
namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.BaselOecdCode
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Views.BaselOecdCode;
    using Views.Shared;

    public class BaselOecdCodeViewModel : IValidatableObject
    {
        [Display(Name = "HeaderDescription", ResourceType = typeof(BaselOecdCodeResources))]
        public Guid? SelectedCode { get; set; }

        public IList<WasteCodeViewModel> WasteCodes { get; set; }

        public SelectList Codes
        {
            get
            {
                return new SelectList(WasteCodes, "Id", "Name", SelectedCode);
            }
        }

        [Display(Name = "NotListed", ResourceType = typeof(BaselOecdCodeResources))]
        public bool NotListed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NotListed && !SelectedCode.HasValue)
            {
                yield return new ValidationResult(BaselOecdCodeResources.CodeRequired, new[] { "SelectedCode" });
            }
        }

        public Guid? GetCode()
        {
            return NotListed ? null : SelectedCode;
        }
    }
}
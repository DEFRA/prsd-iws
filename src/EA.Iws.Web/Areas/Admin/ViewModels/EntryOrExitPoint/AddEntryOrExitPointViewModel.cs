namespace EA.Iws.Web.Areas.Admin.ViewModels.EntryOrExitPoint
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Views.EntryOrExitPoint;

    public class AddEntryOrExitPointViewModel : IValidatableObject
    {
        [Display(ResourceType = typeof(AddEntryOrExitPointViewModelResources), Name = "NameLabel")]
        [Required(ErrorMessageResourceType = typeof(AddEntryOrExitPointViewModelResources), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }
        
        public bool IsWarningAccepted { get; set; }

        public IList<CountryData> Countries { get; set; }

        public SelectList CountryList
        {
            get
            {
                return new SelectList(Countries.Select(c => new KeyValuePair<string, Guid>(c.Name, c.Id)), "Value", "Key");
            }
        }

        [Display(ResourceType = typeof(AddEntryOrExitPointViewModelResources), Name = "CountryLabel")]
        [Required(ErrorMessageResourceType = typeof(AddEntryOrExitPointViewModelResources), ErrorMessageResourceName = "CountryRequired")]
        public Guid? CountryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsWarningAccepted)
            {
                yield return new ValidationResult(AddEntryOrExitPointViewModelResources.WarningAcceptanceRequired, new[] { "IsWarningAccepted" });
            }
        }
    }
}
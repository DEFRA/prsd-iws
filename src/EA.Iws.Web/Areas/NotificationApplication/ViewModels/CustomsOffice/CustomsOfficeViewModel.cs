namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.CustomsOffice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Views.EntryCustomsOffice;

    public class CustomsOfficeViewModel : IValidatableObject
    {
        public CustomsOfficeViewModel()
        {
            this.CustomsOfficeRequired = null;
        }

        //[Required]
        [StringLength(1024)]
        public string Name { get; set; }

        //[Required]
        [StringLength(4000)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public SelectList Countries { get; set; }

        //[Required]
        [Display(Name = "Country", ResourceType = typeof(EntryCustomsOfficeResources))]
        public Guid? SelectedCountry { get; set; }

        public int Steps { get; set; }

        public bool? CustomsOfficeRequired { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomsOfficeRequired.GetValueOrDefault())
            {
                if (SelectedCountry == null)
                {
                    yield return new ValidationResult("The Country field is required.", new[] { "SelectedCountry" });
                }
                if (Address == null)
                {
                    yield return new ValidationResult("The Address field is required.", new[] { "Address" });
                }
                if (Name == null)
                {
                    yield return new ValidationResult("The Name field is required.", new[] { "Name" });
                }
            }
        }
    }
}
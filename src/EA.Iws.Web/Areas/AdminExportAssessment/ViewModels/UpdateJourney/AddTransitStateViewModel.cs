namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;

    public class AddTransitStateViewModel
    {
        [Display(Name = "Country", ResourceType = typeof(AddTransitStateViewModelResources))]
        [Required(ErrorMessageResourceName = "TransitCountryRequired", ErrorMessageResourceType = typeof(AddTransitStateViewModelResources))]
        public Guid? CountryId { get; set; }

        public SelectList Countries { get; set; }

        public SelectList EntryOrExitPoints { get; set; }

        [Display(Name = "EntryPoint", ResourceType = typeof(AddTransitStateViewModelResources))]
        [Required(ErrorMessageResourceName = "EntryPointRequired", ErrorMessageResourceType = typeof(AddTransitStateViewModelResources))]
        public Guid? EntryPointId { get; set; }

        [Display(Name = "ExitPoint", ResourceType = typeof(AddTransitStateViewModelResources))]
        [Required(ErrorMessageResourceName = "ExitPointRequired", ErrorMessageResourceType = typeof(AddTransitStateViewModelResources))]
        public Guid? ExitPointId { get; set; }

        [Display(Name = "CA", ResourceType = typeof(AddTransitStateViewModelResources))]
        [Required(ErrorMessageResourceName = "CARequired", ErrorMessageResourceType = typeof(AddTransitStateViewModelResources))]
        public StringGuidRadioButtons CompetentAuthorities { get; set; }
    }
}
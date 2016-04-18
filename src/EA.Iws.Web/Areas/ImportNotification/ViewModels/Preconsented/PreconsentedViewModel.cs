namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Preconsented
{
    using Web.ViewModels.Shared;

    public class PreconsentedViewModel
    {
        public bool? SelectedValue
        {
            get
            {
                if (PreconsentedFacilityExists.SelectedValue == null)
                {
                    return null;
                }

                return PreconsentedFacilityExists.SelectedValue == PreconsentedViewModelResources.YesOption;
            }
        }

        public RadioButtonStringCollectionOptionalViewModel PreconsentedFacilityExists { get; set; }

        public PreconsentedViewModel() : this(null)
        {
        }

        public PreconsentedViewModel(bool? facilityExists)
        {
            PreconsentedFacilityExists = 
                new RadioButtonStringCollectionOptionalViewModel(new[] 
                {
                    PreconsentedViewModelResources.YesOption,
                    PreconsentedViewModelResources.NoOption
                });

            if (facilityExists.HasValue)
            {
                PreconsentedFacilityExists.SelectedValue = facilityExists.Value ?
                    PreconsentedViewModelResources.YesOption : PreconsentedViewModelResources.NoOption;
            }
        }
    }
}
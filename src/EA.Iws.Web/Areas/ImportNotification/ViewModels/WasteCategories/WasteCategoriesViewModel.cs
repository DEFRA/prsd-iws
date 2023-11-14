namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteCategories
{
    using EA.Iws.Core.WasteType;
    using EA.Iws.Web.ViewModels.Shared;
    using Prsd.Core.Helpers;

    public class WasteCategoriesViewModel
    {
        public WasteCategoriesViewModel()
        {
            WasteCategories = RadioButtonStringCollectionViewModel.CreateFromEnum<WasteCategoryType>();
        }

        public RadioButtonStringCollectionViewModel WasteCategories { get; set; }

        public WasteCategoryType GetSelectedWasteCategoryType()
        {
            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Metals))
            {
                return WasteCategoryType.Metals;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Catalysts))
            {
                return WasteCategoryType.Catalysts;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.WEEE))
            {
                return WasteCategoryType.WEEE;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Plastics))
            {
                return WasteCategoryType.Plastics;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Batteries))
            {
                return WasteCategoryType.Batteries;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Clinical))
            {
                return WasteCategoryType.Clinical;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Pharmaceuticals))
            {
                return WasteCategoryType.Pharmaceuticals;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Ragsabsorbents))
            {
                return WasteCategoryType.Ragsabsorbents;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Oils))
            {
                return WasteCategoryType.Oils;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Solventsdyes))
            {
                return WasteCategoryType.Solventsdyes;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Singleship))
            {
                return WasteCategoryType.Singleship;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Platformrig))
            {
                return WasteCategoryType.Platformrig;
            }

            if (WasteCategories.SelectedValue == EnumHelper.GetDisplayName(WasteCategoryType.Notapplicable))
            {
                return WasteCategoryType.Notapplicable;
            }

            return default(WasteCategoryType);
        }
    }
}
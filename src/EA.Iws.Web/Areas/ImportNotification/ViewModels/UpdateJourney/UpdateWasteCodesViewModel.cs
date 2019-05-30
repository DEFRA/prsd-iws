namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportNotification.Update;
    using Core.WasteCodes;
    using Newtonsoft.Json;
    using Shared;

    public class UpdateWasteCodesViewModel : IValidatableObject
    {
        public UpdateWasteCodesViewModel()
        {
        }

        public UpdateWasteCodesViewModel(WasteTypes data)
        {
            Name = data.Name;
            BaselCodeNotListed = data.BaselCodeNotListed;
            SelectedBaselCode = data.SelectedBaselCode;

            YCodeNotApplicable = data.YCodeNotApplicable;
            HCodeNotApplicable = data.HCodeNotApplicable;
            UnClassNotApplicable = data.UnClassNotApplicable;

            SelectedEwcCodesDisplay = new List<WasteCodeViewModel>();
            SelectedYCodesDisplay = new List<WasteCodeViewModel>();
            SelectedHCodesDisplay = new List<WasteCodeViewModel>();
            SelectedUnClassesDisplay = new List<WasteCodeViewModel>();
        }

        public Guid ImportNotificationId { get; set; }

        [Display(Name = "BaselCodeNotListed", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public bool BaselCodeNotListed { get; set; }

        [Display(Name = "YCodeNotApplicable", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public bool YCodeNotApplicable { get; set; }

        [Display(Name = "HCodeNotApplicable", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public bool HCodeNotApplicable { get; set; }

        [Display(Name = "UnClassNotApplicable", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public bool UnClassNotApplicable { get; set; }

        [Display(Name = "WasteTypeName", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public string Name { get; set; }

        public IList<WasteCodeViewModel> AllCodes { get; set; }

        [Display(Name = "BaselCode", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public Guid? SelectedBaselCode { get; set; }

        [Display(Name = "EwcCode", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public Guid? SelectedEwcCode { get; set; }

        [Display(Name = "YCode", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public Guid? SelectedYCode { get; set; }

        [Display(Name = "HCode", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public Guid? SelectedHCode { get; set; }

        [Display(Name = "UnClass", ResourceType = typeof(UpdateWasteCodesViewModelResources))]
        public Guid? SelectedUnClass { get; set; }

        public string SelectedEwcCodesJson { get; set; }
        public string SelectedYCodesJson { get; set; }
        public string SelectedHCodesJson { get; set; }
        public string SelectedUnClassesJson { get; set; }

        public IList<WasteCodeViewModel> SelectedEwcCodesDisplay { get; set; }
        public IList<WasteCodeViewModel> SelectedYCodesDisplay { get; set; }
        public IList<WasteCodeViewModel> SelectedHCodesDisplay { get; set; }
        public IList<WasteCodeViewModel> SelectedUnClassesDisplay { get; set; }

        public SelectList BaselCodes
        {
            get
            {
                return new SelectList(
                    AllCodes.Where(c => c.CodeType == CodeType.Basel || c.CodeType == CodeType.Oecd),
                    "Id",
                    "Name",
                    SelectedBaselCode);
            }
        }

        public SelectList EwcCodes
        {
            get
            {
                return new SelectList(
                    AllCodes.Where(c => c.CodeType == CodeType.Ewc),
                    "Id",
                    "Name",
                    SelectedEwcCode);
            }
        }

        public SelectList YCodes
        {
            get
            {
                return new SelectList(
                    AllCodes.Where(c => c.CodeType == CodeType.Y),
                    "Id",
                    "Name",
                    SelectedYCode);
            }
        }

        public SelectList HCodes
        {
            get
            {
                return new SelectList(
                    AllCodes.Where(c => c.CodeType == CodeType.H),
                    "Id",
                    "Name",
                    SelectedHCode);
            }
        }

        public SelectList UnClasses
        {
            get
            {
                return new SelectList(
                    AllCodes.Where(c => c.CodeType == CodeType.Un),
                    "Id",
                    "Name",
                    SelectedUnClass);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.WasteNameRequired, new[] { "Name" });
            }

            if (!BaselCodeNotListed && !SelectedBaselCode.HasValue)
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.BaselCodeRequired, new[] { "SelectedBaselCode" });
            }

            var selectedEwcCodes = JsonConvert.DeserializeObject<List<Guid>>(SelectedEwcCodesJson);
            var selectedHCodes = JsonConvert.DeserializeObject<List<Guid>>(SelectedHCodesJson);
            var selectedYCodes = JsonConvert.DeserializeObject<List<Guid>>(SelectedYCodesJson);
            var selectedUnCodes = JsonConvert.DeserializeObject<List<Guid>>(SelectedUnClassesJson);

            if (!selectedEwcCodes.Any())
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.EwcCodeRequired, new[] { "SelectedEwcCode" });
            }

            if (!HCodeNotApplicable && !selectedHCodes.Any())
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.HCodeRequired, new[] { "SelectedHCode" });
            }

            if (!YCodeNotApplicable && !selectedYCodes.Any())
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.YCodeRequired, new[] { "SelectedYCode" });
            }

            if (!UnClassNotApplicable && !selectedUnCodes.Any())
            {
                yield return new ValidationResult(UpdateWasteCodesViewModelResources.UnClassRequired, new[] { "SelectedUnClass" });
            }
        }
    }
}
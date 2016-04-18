namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Core.WasteCodes;
    using Shared;

    public class WasteTypeViewModel
    {
        public WasteTypeViewModel()
        {
        }

        public WasteTypeViewModel(WasteType data)
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

        [Display(Name = "BaselCodeNotListed", ResourceType = typeof(WasteTypeViewModelResources))]
        public bool BaselCodeNotListed { get; set; }

        [Display(Name = "YCodeNotApplicable", ResourceType = typeof(WasteTypeViewModelResources))]
        public bool YCodeNotApplicable { get; set; }

        [Display(Name = "HCodeNotApplicable", ResourceType = typeof(WasteTypeViewModelResources))]
        public bool HCodeNotApplicable { get; set; }

        [Display(Name = "UnClassNotApplicable", ResourceType = typeof(WasteTypeViewModelResources))]
        public bool UnClassNotApplicable { get; set; }

        [Display(Name = "WasteTypeName", ResourceType = typeof(WasteTypeViewModelResources))]
        public string Name { get; set; }

        public IList<WasteCodeViewModel> AllCodes { get; set; }

        [Display(Name = "BaselCode", ResourceType = typeof(WasteTypeViewModelResources))]
        public Guid? SelectedBaselCode { get; set; }

        [Display(Name = "EwcCode", ResourceType = typeof(WasteTypeViewModelResources))]
        public Guid? SelectedEwcCode { get; set; }

        [Display(Name = "YCode", ResourceType = typeof(WasteTypeViewModelResources))]
        public Guid? SelectedYCode { get; set; }

        [Display(Name = "HCode", ResourceType = typeof(WasteTypeViewModelResources))]
        public Guid? SelectedHCode { get; set; }

        [Display(Name = "UnClass", ResourceType = typeof(WasteTypeViewModelResources))]
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
    }
}
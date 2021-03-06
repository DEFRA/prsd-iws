﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.NotificationAssessment;
    using Core.WasteCodes;

    public class WasteCodeOverviewViewModel
    {
        public Guid NotificationId { get; set; }
        public WasteCodeData[] BaselOecdCode { get; set; }
        public WasteCodeData[] EwcCodes { get; set; }
        public WasteCodeData[] NationExportCode { get; set; }
        public WasteCodeData[] NationImportCode { get; set; }
        public WasteCodeData[] OtherCodes { get; set; }
        public WasteCodeData[] YCodes { get; set; }
        public WasteCodeData[] HCodes { get; set; }
        public WasteCodeData[] UnClass { get; set; }
        public WasteCodeData[] UnNumber { get; set; }
        public WasteCodeData[] CustomCodes { get; set; }
        public bool IsBaselOecdCodeCompleted { get; set; }
        public bool AreEwcCodesCompleted { get; set; }
        public bool AreYCodesCompleted { get; set; }
        public bool AreHCodesCompleted { get; set; }
        public bool AreUnClassesCompleted { get; set; }
        public bool AreUnNumbersCompleted { get; set; }
        public bool AreOtherCodesCompleted { get; set; }
        public bool CanEditEWCCodes { get; set; }
        public bool CanEditYCodes { get; set; }     
        public bool CanEditHCodes { get; set; }

        public bool IsBaselOecdCodeNotApplicable
        {
            get { return IsCodeTypeNotApplicable(IsBaselOecdCodeCompleted, BaselOecdCode); }
        }

        public bool AreEwcCodesNotApplicable
        {
            get { return IsCodeTypeNotApplicable(AreEwcCodesCompleted, EwcCodes); }
        }

        public bool AreYCodesNotApplicable
        {
            get { return IsCodeTypeNotApplicable(AreYCodesCompleted, YCodes); }
        }

        public bool AreHCodesNotApplicable
        {
            get { return IsCodeTypeNotApplicable(AreHCodesCompleted, HCodes); }
        }

        public bool AreUnNumbersNotApplicable
        {
            get { return IsCodeTypeNotApplicable(AreUnNumbersCompleted, UnNumber); }
        }

        public bool AreUnClassesNotApplicable
        {
            get { return IsCodeTypeNotApplicable(AreUnClassesCompleted, UnClass); }
        }

        private bool IsCodeTypeNotApplicable(bool isCodeTypeCompleted, WasteCodeData[] codesOfType)
        {
            return isCodeTypeCompleted
                       && codesOfType != null
                       && codesOfType.Any()
                       && codesOfType.First().IsNotApplicable;
        }

        public WasteCodeOverviewViewModel(WasteCodesOverviewInfo classifyYourWasteInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = classifyYourWasteInfo.NotificationId;
            BaselOecdCode = classifyYourWasteInfo.BaselOecdCode;
            EwcCodes = classifyYourWasteInfo.EwcCodes.OrderBy(w => w.Code).ToArray();
            NationExportCode = classifyYourWasteInfo.NationExportCode;
            NationImportCode = classifyYourWasteInfo.NationImportCode;
            OtherCodes = classifyYourWasteInfo.OtherCodes;
            YCodes = classifyYourWasteInfo.YCodes.OrderBy(w => Regex.Match(!string.IsNullOrEmpty(w.Code) ? w.Code : string.Empty, @"(\D+)").Value).ThenBy(w =>
            {
                double val;
                double.TryParse(Regex.Match(!string.IsNullOrEmpty(w.Code) ? w.Code : string.Empty, @"(\d+(\.\d*)?|\.\d+)").Value, out val);
                return val;
            }).ToArray();
            HCodes = classifyYourWasteInfo.HCodes.OrderBy(w => Regex.Match(!string.IsNullOrEmpty(w.Code) ? w.Code : string.Empty, @"(\D+)").Value).ThenBy(w =>
            {
                double val;
                double.TryParse(Regex.Match(!string.IsNullOrEmpty(w.Code) ? w.Code : string.Empty, @"(\d+(\.\d*)?|\.\d+)").Value, out val);
                return val;
            }).ToArray();
            UnClass = classifyYourWasteInfo.UnClass.OrderBy(w => w.Code).ToArray();
            UnNumber = classifyYourWasteInfo.UnNumber.OrderBy(w => w.Code).ToArray();
            CustomCodes = classifyYourWasteInfo.CustomCodes;
            IsBaselOecdCodeCompleted = progress.HasBaselOecdCode;
            AreEwcCodesCompleted = progress.HasEwcCodes;
            AreYCodesCompleted = progress.HasYCodes;
            AreHCodesCompleted = progress.HasHCodes;
            AreUnClassesCompleted = progress.HasUnClasses;
            AreUnNumbersCompleted = progress.HasUnNumbers;
            AreOtherCodesCompleted = progress.HasOtherCodes;
        }
    }
}
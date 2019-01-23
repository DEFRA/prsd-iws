namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Home
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Core.ImportNotification.Summary;
    using Core.ImportNotificationAssessment;
    using Core.Shared;

    public class SummaryTableContainerViewModel
    {
        public ImportNotificationSummary Details { get; set; }

        public bool ShowChangeLinks { get; set; }

        public bool ShowPreconsentedLinks
        {
            get { return Details.Type == NotificationType.Recovery; }
        }

        public bool ShowChangeNumberOfShipmentsLink { get; set; }

        public bool ShowChangeEntryExitPointLink { get; set; }

        public SummaryTableContainerViewModel(ImportNotificationSummary details, bool canChangeNumberOfShipments,
            bool canChangeEntryExitPoint)
        {
            Details = details;
            ShowChangeLinks = details.Status == ImportNotificationStatus.NotificationReceived;
            ShowChangeNumberOfShipmentsLink = canChangeNumberOfShipments &&
                                              details.Status == ImportNotificationStatus.Consented;
            ShowChangeEntryExitPointLink = canChangeEntryExitPoint && details.Status != ImportNotificationStatus.New &&
                                           details.Status != ImportNotificationStatus.NotificationReceived;

            var ewcCodesOrdered = details.WasteType.EwcCodes.WasteCodes.OrderBy(w => w.Name).ToList();
            var ycodesOrdered = details.WasteType.YCodes.WasteCodes.OrderBy(w => Regex.Match(!string.IsNullOrEmpty(w.Name) ? w.Name : string.Empty, @"(\D+)").Value).ThenBy(w =>
            {
                double val;
                double.TryParse(Regex.Match(!string.IsNullOrEmpty(w.Name) ? w.Name : string.Empty, @"(\d+(\.\d*)?|\.\d+)").Value, out val);
                return val;
            }).ToList();
            var hcodesOrdered = details.WasteType.HCodes.WasteCodes.OrderBy(w => Regex.Match(!string.IsNullOrEmpty(w.Name) ? w.Name : string.Empty, @"(\D+)").Value).ThenBy(w =>
            {
                double val;
                double.TryParse(Regex.Match(!string.IsNullOrEmpty(w.Name) ? w.Name : string.Empty, @"(\d+(\.\d*)?|\.\d+)").Value, out val);
                return val;
            }).ToList();
            var unclassesOrdered = details.WasteType.UnClasses.WasteCodes.OrderBy(w => w.Name).ToList();

            Details.WasteType.EwcCodes.WasteCodes = ewcCodesOrdered;
            Details.WasteType.YCodes.WasteCodes = ycodesOrdered;
            Details.WasteType.HCodes.WasteCodes = hcodesOrdered;
            Details.WasteType.UnClasses.WasteCodes = unclassesOrdered;
        }
    }
}
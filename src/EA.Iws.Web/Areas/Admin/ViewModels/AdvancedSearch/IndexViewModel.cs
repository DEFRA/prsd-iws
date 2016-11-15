namespace EA.Iws.Web.Areas.Admin.ViewModels.AdvancedSearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using Core.OperationCodes;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class IndexViewModel : IValidatableObject
    {
        private const int StatusIdOffset = 500;

        public IndexViewModel()
        {
            ConsentValidFromStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidFromEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidToStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            ConsentValidToEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            NotificationReceivedStart = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            NotificationReceivedEnd = new OptionalDateInputViewModel(allowPastDates: true, showLabels: false);
            NotificationTypes = new SelectList(EnumHelper.GetValues(typeof(NotificationType)), dataTextField: "Value", dataValueField: "Key");
            TradeDirections = new SelectList(EnumHelper.GetValues(typeof(TradeDirection)), dataTextField: "Value", dataValueField: "Key");
            InterimStatus = new SelectList(new[]
            {
                new SelectListItem
                {
                    Text = "Interim",
                    Value = "true"
                },
                new SelectListItem
                {
                    Text = "Non-interim",
                    Value = "false"
                }
            }, dataTextField: "Text", dataValueField: "Value");
            OperationCodes = new MultiSelectList(EnumHelper.GetValues(typeof(OperationCode)), dataTextField: "Value", dataValueField: "Key");

            NotificationStatuses = new SelectList(GetCombinedNotificationStatuses(), dataTextField: "Name", dataValueField: "StatusId", dataGroupField: "TradeDirection", selectedValue: null);

            SelectedOperationCodes = new OperationCode[] { };
        }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "EwcCode")]
        public string EwcCode { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ProducerName")]
        public string ProducerName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ImporterName")]
        public string ImporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ImportCountryName")]
        public string ImportCountryName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExportCountryName")]
        public string ExportCountryName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "LocalArea")]
        public Guid? LocalAreaId { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ConsentValidFrom")]
        public OptionalDateInputViewModel ConsentValidFromStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel ConsentValidFromEnd { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ConsentValidTo")]
        public OptionalDateInputViewModel ConsentValidToStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel ConsentValidToEnd { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExporterName")]
        public string ExporterName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "BaselOecdCode")]
        public string BaselOecdCode { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "FacilityName")]
        public string FacilityName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "ExitPointName")]
        public string ExitPointName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "EntryPointName")]
        public string EntryPointName { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "NotificationReceived")]
        public OptionalDateInputViewModel NotificationReceivedStart { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "To")]
        public OptionalDateInputViewModel NotificationReceivedEnd { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "NotificationType")]
        public NotificationType? SelectedNotificationType { get; set; }

        public SelectList NotificationTypes { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "TradeDirection")]
        public TradeDirection? SelectedTradeDirection { get; set; }

        public SelectList TradeDirections { get; set; }

        public SelectList Areas { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "IsInterim")]
        public bool? IsInterim { get; set; }

        public SelectList InterimStatus { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "OperationCodes")]
        public OperationCode[] SelectedOperationCodes { get; set; }

        public MultiSelectList OperationCodes { get; set; }

        public SelectList NotificationStatuses { get; set; }

        [Display(ResourceType = typeof(IndexViewModelResources), Name = "NotificationStatus")]
        public int? SelectedNotificationStatusId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EwcCode) && string.IsNullOrWhiteSpace(ProducerName)
                && string.IsNullOrWhiteSpace(ImporterName) && string.IsNullOrWhiteSpace(ImportCountryName)
                && !LocalAreaId.HasValue && !(ConsentValidToStart.IsCompleted && ConsentValidToEnd.IsCompleted)
                && string.IsNullOrWhiteSpace(ExporterName) && string.IsNullOrWhiteSpace(BaselOecdCode)
                && string.IsNullOrWhiteSpace(FacilityName) && string.IsNullOrWhiteSpace(ExitPointName)
                && string.IsNullOrWhiteSpace(EntryPointName) && !(NotificationReceivedStart.IsCompleted && NotificationReceivedEnd.IsCompleted)
                && !SelectedNotificationStatusId.HasValue && !SelectedTradeDirection.HasValue && !SelectedNotificationType.HasValue
                && !SelectedOperationCodes.Any() && !IsInterim.HasValue && string.IsNullOrWhiteSpace(ExportCountryName)
                && !(ConsentValidFromStart.IsCompleted && ConsentValidFromEnd.IsCompleted))
            {
                yield return new ValidationResult(IndexViewModelResources.NoSearchCriteriaCompleted);
            }

            if (ConsentValidToStart.IsCompleted && !ConsentValidToEnd.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterEndDate, new[] { "ConsentValidToEnd" });
            }

            if (ConsentValidToEnd.IsCompleted && !ConsentValidToStart.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterStartDate, new[] { "ConsentValidToStart" });
            }

            if (ConsentValidFromStart.IsCompleted && !ConsentValidFromEnd.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterEndDate, new[] { "ConsentValidFromEnd" });
            }

            if (ConsentValidFromEnd.IsCompleted && !ConsentValidFromStart.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterStartDate, new[] { "ConsentValidFromStart" });
            }

            if (NotificationReceivedStart.IsCompleted && !NotificationReceivedEnd.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterEndDate, new[] { "NotificationReceivedEnd" });
            }

            if (NotificationReceivedEnd.IsCompleted && !NotificationReceivedStart.IsCompleted)
            {
                yield return new ValidationResult(IndexViewModelResources.PleaseEnterStartDate, new[] { "NotificationReceivedStart" });
            }
        }

        public NotificationStatus? GetExportNotificationStatus()
        {
            if (!SelectedNotificationStatusId.HasValue || SelectedNotificationStatusId.Value > StatusIdOffset)
            {
                return null;
            }

            NotificationStatus status;

            if (Enum.TryParse(SelectedNotificationStatusId.ToString(), out status))
            {
                return status;
            }

            return null;
        }

        public ImportNotificationStatus? GetImportNotificationStatus()
        {
            if (!SelectedNotificationStatusId.HasValue || SelectedNotificationStatusId.Value < StatusIdOffset)
            {
                return null;
            }

            ImportNotificationStatus status;

            if (Enum.TryParse((SelectedNotificationStatusId.Value - StatusIdOffset).ToString(), out status))
            {
                return status;
            }

            return null;
        }

        private static IEnumerable<NotificationStatusViewModel> GetCombinedNotificationStatuses()
        {
            foreach (var status in EnumHelper.GetValues(typeof(NotificationStatus))
                .Where(x => x.Key != (int)NotificationStatus.InDetermination
                    || x.Key != (int)NotificationStatus.NotSubmitted))
            {
                yield return new NotificationStatusViewModel
                {
                    Name = status.Value,
                    StatusId = status.Key,
                    TradeDirection = TradeDirection.Export
                };
            }

            foreach (var status in EnumHelper.GetValues(typeof(ImportNotificationStatus))
                .Where(x => x.Key != (int)ImportNotificationStatus.New))
            {
                yield return new NotificationStatusViewModel
                {
                    Name = status.Value,
                    StatusId = status.Key + StatusIdOffset,
                    TradeDirection = TradeDirection.Import
                };
            }
        } 

        public class NotificationStatusViewModel
        {
            public TradeDirection TradeDirection { get; set; }

            public int StatusId { get; set; }

            public string Name { get; set; }
        }
    }
}
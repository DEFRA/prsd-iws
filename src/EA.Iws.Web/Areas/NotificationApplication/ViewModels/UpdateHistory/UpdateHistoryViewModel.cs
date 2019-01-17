namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.UpdateHistory
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Notification.Audit;
    using Views.UpdateHistory;

    public class UpdateHistoryViewModel : IValidatableObject
    {
        public UpdateHistoryViewModel()
        {
        }
        
        public UpdateHistoryViewModel(IEnumerable<NotificationAuditForDisplay> notificationUpdateHistory, IEnumerable<NotificationAuditScreen> screens)
        {
            this.Screens = screens.ToList();

            UpdateHistoryItems = new List<NotificationAuditForDisplay>();
            foreach (NotificationAuditForDisplay updateHistory in notificationUpdateHistory)
            {
                UpdateHistoryItems.Add(updateHistory);
            }
        }

        public Guid NotificationId { get; set; }

        public List<NotificationAuditForDisplay> UpdateHistoryItems { get; set; }

        public List<NotificationAuditScreen> Screens { get; set; }

        public bool HasHistoryItems { get; set; }

        public SelectList FilterTerms
        {
            get
            {
                var filterTerms = this.Screens.Select(p => new SelectListItem
                {
                    Text = p.ScreenName,
                    Value = p.Id.ToString()
                }).ToList();

                filterTerms.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });
                filterTerms.Insert(1, new SelectListItem { Text = "Date", Value = "date" });

                return new SelectList(filterTerms, "Value", "Text", SelectedScreen);
            }
        }

        public string SelectedScreen { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Day", ResourceType = typeof(IndexResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? StartDay { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Month", ResourceType = typeof(IndexResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? StartMonth { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Year", ResourceType = typeof(IndexResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? StartYear { get; set; }

        [Required(ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Day", ResourceType = typeof(IndexResources))]
        [Range(1, 31, ErrorMessageResourceName = "DayError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? EndDay { get; set; }

        [Required(ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Month", ResourceType = typeof(IndexResources))]
        [Range(1, 12, ErrorMessageResourceName = "MonthError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? EndMonth { get; set; }

        [Required(ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "Year", ResourceType = typeof(IndexResources))]
        [Range(2015, 3000, ErrorMessageResourceName = "YearError", ErrorMessageResourceType = typeof(IndexResources))]
        public int? EndYear { get; set; }
    }
}
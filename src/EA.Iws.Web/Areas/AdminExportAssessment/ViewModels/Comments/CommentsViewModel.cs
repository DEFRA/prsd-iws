namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.InternalComments;
    using EA.Prsd.Core;
    using Views.Comments;

    public class CommentsViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }
        public NotificationShipmentsCommentsType Type { get; set; }

        public List<InternalComment> Comments { get; set; }

        public SelectList FilterTerms
        {
            get
            {
                var filterTerms = new List<SelectListItem>();

                filterTerms.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });
                filterTerms.Insert(1, new SelectListItem { Text = "Date", Value = "date" });
                if (this.Type == NotificationShipmentsCommentsType.Shipments)
                {
                    filterTerms.Insert(2, new SelectListItem { Text = "Shipment Number", Value = "shipment" });
                }

                return new SelectList(filterTerms, "Value", "Text", "View all");
            }
        }

        public bool HasComments
        {
            get
            {
                return this.TotalNumberOfComments > 0;
            }
        }

        public string SelectedFilter { get; set; }

        public int? ShipmentNumber { get; set; }

        public int? StartDay { get; set; }

        public int? StartMonth { get; set; }

        public int? StartYear { get; set; }

        public int? EndDay { get; set; }

        public int? EndMonth { get; set; }

        public int? EndYear { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        public int TotalNumberOfComments { get; set; }
        public int TotalNumberOfFilteredComments { get; set; }

        public CommentsViewModel()
        {
            this.Comments = new List<InternalComment>();
            this.Type = NotificationShipmentsCommentsType.Notification;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.SelectedFilter == "shipment")
            {
                if (this.ShipmentNumber == null || this.ShipmentNumber < 1 || this.ShipmentNumber > 999999)
                {
                    yield return new ValidationResult(IndexResources.ShipmentNumberError, new[] { "ShipmentNumber" });
                }
            }
            else
            {
                bool allDateFieldsValid = true;
                if (StartDay == null || StartDay < 1 || StartDay > 31)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.DayError, new[] { "StartDay" });
                }
                if (StartMonth == null || StartMonth < 1 || StartMonth > 12)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.MonthError, new[] { "StartMonth" });
                }
                if (StartYear == null || StartYear < 2013 || StartYear > 3000)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.YearError, new[] { "StartYear" });
                }

                if (EndDay == null || EndDay < 1 || EndDay > 31)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.DayError, new[] { "EndDay" });
                }
                if (EndMonth == null || EndMonth < 1 || EndMonth > 12)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.MonthError, new[] { "EndMonth" });
                }
                if (EndYear == null || EndYear < 2013 || EndYear > 3000)
                {
                    allDateFieldsValid = false;
                    yield return new ValidationResult(IndexResources.YearError, new[] { "EndYear" });
                }

                if (!allDateFieldsValid)
                {
                    // Stop further validation if any of the date fields are empty
                    yield break;
                }

                DateTime startDate;
                bool isValidStartDate = SystemTime.TryParse(StartYear.GetValueOrDefault(), StartMonth.GetValueOrDefault(), StartDay.GetValueOrDefault(), out startDate);
                if (!isValidStartDate)
                {
                    yield return new ValidationResult(IndexResources.FromValid, new[] { "StartDay" });
                }

                DateTime endDate;
                bool isValidEndDate = SystemTime.TryParse(EndYear.GetValueOrDefault(), EndMonth.GetValueOrDefault(), EndDay.GetValueOrDefault(), out endDate);
                if (!isValidEndDate)
                {
                    yield return new ValidationResult(IndexResources.ToValid, new[] { "EndDay" });
                }

                if (!(isValidStartDate && isValidEndDate))
                {
                    // Stop further validation if either date is not a valid date
                    yield break;
                }

                if (startDate > endDate)
                {
                    yield return new ValidationResult(IndexResources.FromDateAfterToDate, new[] { "StartYear" });
                }
            }
        }

        public void SetDates(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null || startDate == DateTime.MinValue || endDate == DateTime.MaxValue)
            {
                return;
            }
            this.StartDay = startDate.GetValueOrDefault().Day;
            this.StartMonth = startDate.GetValueOrDefault().Month;
            this.StartYear = startDate.GetValueOrDefault().Year;

            this.EndDay = endDate.GetValueOrDefault().Day;
            this.EndMonth = endDate.GetValueOrDefault().Month;
            this.EndYear = endDate.GetValueOrDefault().Year;
        }
    }
}
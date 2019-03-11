namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.InternalComments;
    using EA.Iws.Web.ViewModels.Shared;
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
        private string shipmentNumberStr;
        public string ShipmentNumberStr
        {
            get
            {
                if (shipmentNumberStr == null || shipmentNumberStr == "0")
                {
                    return string.Empty;
                }
                return shipmentNumberStr;
            }
            set
            {
                shipmentNumberStr = value;
            }
        }

        public int? ShipmentNumber
        {
            get
            {
                int number;
                int.TryParse(this.ShipmentNumberStr, out number);
                return number;
            }
        }

        public DateEntryViewModel From { get; set; }

        public DateEntryViewModel To { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        public int TotalNumberOfComments { get; set; }
        public int TotalNumberOfFilteredComments { get; set; }

        public CommentsViewModel()
        {
            this.Comments = new List<InternalComment>();
            this.Type = NotificationShipmentsCommentsType.Notification;

            this.From = new DateEntryViewModel(showLabels: false);
            this.To = new DateEntryViewModel(showLabels: false);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.SelectedFilter == "shipment")
            {
                if (this.ShipmentNumber == null || this.ShipmentNumber < 1 || this.ShipmentNumber.ToString().Length > 6)
                {
                    yield return new ValidationResult(IndexResources.ShipmentNumberError, new[] { "ShipmentNumber" });
                }
            }
            else if (this.SelectedFilter == "date")
            {
                DateTime startDate;
                bool isValidstartDate = SystemTime.TryParse(this.From.Year.GetValueOrDefault(), this.From.Month.GetValueOrDefault(), this.From.Day.GetValueOrDefault(), out startDate);
                if (!isValidstartDate)
                {
                    yield return new ValidationResult("Please enter a valid start date", new[] { "StartDay" });
                }

                if (!(isValidstartDate))
                {
                    // Stop further validation if either date is not a valid date
                    yield break;
                }

                DateTime endDate;
                bool isValidEndDate = SystemTime.TryParse(this.To.Year.GetValueOrDefault(), this.To.Month.GetValueOrDefault(), this.To.Day.GetValueOrDefault(), out endDate);
                if (!isValidEndDate)
                {
                    yield return new ValidationResult("Please enter a valid end date", new[] { "EndDay" });
                }

                if (!(isValidEndDate))
                {
                    // Stop further validation if either date is not a valid date
                    yield break;
                }

                if (startDate > endDate)
                {
                    yield return new ValidationResult("The from date must be before the to date", new[] { "StartYear" });
                }
            }
        }

        public void SetDates(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null || startDate == DateTime.MinValue || endDate == DateTime.MaxValue)
            {
                return;
            }
            this.From.Day = startDate.GetValueOrDefault().Day;
            this.From.Month = startDate.GetValueOrDefault().Month;
            this.From.Year = startDate.GetValueOrDefault().Year;

            this.To.Day = endDate.GetValueOrDefault().Day;
            this.To.Month = endDate.GetValueOrDefault().Month;
            this.To.Year = endDate.GetValueOrDefault().Year;
        }
    }
}
namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.ShipmentAudit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels;
    using Web.ViewModels.Shared;

    public class ShipmentAuditViewModel : IValidatableObject
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public ShipmentAuditTrailViewModel ShipmentAuditModel { get; set; }

        public int NumberOfShipmentAudits { get; set; }

        public bool DisplayFilter
        {
            get { return (SelectedFilter.HasValue || ShipmentAuditModel.NumberOfShipmentAudits > 0); }
        }

        public ShipmentAuditFilterType? SelectedFilter { get; set; }

        [Display(Name = "Shipment Number")]
        public string ShipmentNumberSearch { get; set; }

        public int ShipmentNumber
        {
            get
            {
                int result;
                int.TryParse(ShipmentNumberSearch, out result);
                return result;
            }
        }

        public SelectList FilterTerms
        {
            get
            {
                var filters =
                    Enum.GetValues(typeof(ShipmentAuditFilterType))
                        .Cast<ShipmentAuditFilterType>()
                        .Select(
                            s => new SelectListItem
                            {
                                Text = EnumHelper.GetDisplayName(s),
                                Value = ((int)s).ToString()
                            })
                        .ToList();

                filters.Insert(0, new SelectListItem { Text = "View all", Value = "-1" });

                return new SelectList(filters, "Value", "Text", SelectedFilter);
            }
        }

        public ShipmentAuditViewModel()
        {
            ShipmentAuditModel = new ShipmentAuditTrailViewModel();
        }

        public ShipmentAuditViewModel(ShipmentAuditData data)
        {
            ShipmentAuditModel = new ShipmentAuditTrailViewModel(data);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedFilter == ShipmentAuditFilterType.ShipmentNumber)
            {
                if (string.IsNullOrEmpty(ShipmentNumberSearch) || ShipmentNumberSearch.Length > 6 || ShipmentNumber < 1)
                {
                    yield return new ValidationResult("Enter a valid shipment number", new[] { "ShipmentNumberSearch" });
                }
            }
        }
    }
}
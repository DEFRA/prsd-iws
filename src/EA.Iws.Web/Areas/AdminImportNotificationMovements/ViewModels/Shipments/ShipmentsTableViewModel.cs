namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Shipments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using Prsd.Core.Helpers;

    public class ShipmentsTableViewModel
    {
        public ShipmentsTableViewModel(MovementsSummary data)
        {
            ImportNotificationId = data.ImportNotificationId;
            NotificationType = data.NotificationType;
            TableData = data.TableData.Select(d => new TableDataViewModel(d)).ToList();
        }

        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<TableDataViewModel> TableData { get; set; }

        public ImportMovementStatus? SelectedMovementStatus { get; set; }

        public SelectList MovementStatuses
        {
            get
            {
                var units = Enum.GetValues(typeof(ImportMovementStatus))
                    .Cast<ImportMovementStatus>()
                    .Select(s => new SelectListItem
                    {
                        Text = EnumHelper.GetDisplayName(s),
                        Value = ((int)s).ToString()
                    }).ToList();

                units.Insert(0, new SelectListItem { Text = "View all", Value = string.Empty });

                return new SelectList(units, "Value", "Text", SelectedMovementStatus);
            }
        }

        public bool ShowShipments()
        {
            return true;
        }
    }
}
namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;
    using Infrastructure;
    using Prsd.Core.Helpers;

    public class SummaryViewModel
    {
        public SummaryViewModel()
        {
        }

        public SummaryViewModel(IEnumerable<MovementInfo> movements)
        {
            var movementInfos = movements as MovementInfo[] ?? movements.ToArray();

            MovementIds = movementInfos.Select(m => m.Id);
            NumberOfShipments = movementInfos.Count();
            ShipmentNumbers = movementInfos.Select(m => m.ShipmentNumber).ToRangeString();
            Quantity = string.Format("{0} {1}", movementInfos.First().ActualQuantity.ToString("G29"),
                EnumHelper.GetShortName(movementInfos.First().Unit));
            PackagingTypes = string.Join(", ", movementInfos.First().PackagingTypes.Select(EnumHelper.GetDisplayName));
           
            TotalQuantity = string.Format("{0} {1}", movementInfos.Sum(m => m.ActualQuantity).ToString("G29"),
                EnumHelper.GetDisplayName(movementInfos.First().Unit));
        }

        public IEnumerable<Guid> MovementIds { get; set; }

        public string ShipmentNumbers { get; set; }

        public string Quantity { get; set; }

        public string PackagingTypes { get; set; }

        public int NumberOfShipments { get; set; }

        public string TotalQuantity { get; set; }
    }
}
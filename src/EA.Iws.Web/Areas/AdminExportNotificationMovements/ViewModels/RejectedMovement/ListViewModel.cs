namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.RejectedMovement
{
    using System.Collections.Generic;
    using Core.Movement;

    public class ListViewModel
    {
        public IList<RejectedMovementListData> Movements { get; set; }
    }
}
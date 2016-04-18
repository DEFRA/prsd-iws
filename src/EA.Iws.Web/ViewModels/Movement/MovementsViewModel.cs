namespace EA.Iws.Web.ViewModels.Movement
{
    using System;
    using System.Collections.Generic;

    public class MovementsViewModel
    {
        public Guid NotificationId { get; set; }
        public Dictionary<int, Guid> Movements { get; set; }
    }
}
namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using TransportRoute;

    public partial class NotificationApplication
    {
        public void AddStateOfExportToNotification(StateOfExport stateOfExport)
        {
            Guard.ArgumentNotNull(stateOfExport);
            
            if (this.StateOfExport != null)
            {
                throw new InvalidOperationException("Cannot add a State of Export to Notification " + this.Id + ". This Notification already has a State of Export " + this.StateOfExport.Id + ".");
            }

            this.StateOfExport = stateOfExport;
        }
    }
}

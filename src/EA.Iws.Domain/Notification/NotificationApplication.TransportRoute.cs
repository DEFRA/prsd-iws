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
                throw new InvalidOperationException("Cannot add a State of Export to Notification " 
                    + this.Id 
                    + ". This Notification already has a State of Export " 
                    + this.StateOfExport.Id 
                    + ".");
            }

            this.StateOfExport = stateOfExport;
        }

        public void AddStateOfImportToNotification(StateOfImport stateOfImport)
        {
            Guard.ArgumentNotNull(stateOfImport);

            if (this.StateOfImport != null)
            {
                throw new InvalidOperationException("Cannot add a State of Import to Notification " 
                    + this.Id 
                    + ". This Notification already has a State of Import " 
                    + this.StateOfImport.Id 
                    + ".");
            }

            if (this.StateOfExport != null && this.StateOfExport.Country.Id == stateOfImport.Country.Id)
            {
                throw new InvalidOperationException(string.Format("Cannot add a State of Import in the same country as the State of Export for Notification {0}. Country: {1}", 
                    this.Id, 
                    this.StateOfExport.Country.Name));
            }

            this.StateOfImport = stateOfImport;
        }
    }
}

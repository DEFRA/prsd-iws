namespace EA.Iws.Domain.NotificationApplication.WasteRecovery
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;
    
    public class WasteDisposal : Entity
    {
        public Guid NotificationId { get; private set; }

        public string Method { get; private set; }

        public DisposalCost Cost { get; private set; }
        
        protected WasteDisposal()
        {
        }

        public WasteDisposal(Guid notificationId, string method, DisposalCost cost)
        {
            Guard.ArgumentNotNullOrEmpty(() => method, method);

            NotificationId = notificationId;
            Method = method;
            Cost = cost;
        }

        public void UpdateWasteDisposal(string method, DisposalCost cost)
        {
            Guard.ArgumentNotNullOrEmpty(() => method, method);

            Method = method;
            Cost = cost;
        }
    }
}

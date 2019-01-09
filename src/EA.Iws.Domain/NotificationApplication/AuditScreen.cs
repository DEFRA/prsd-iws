namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class AuditScreen : Entity
    {
        public string ScreenName { get; private set; }

        protected AuditScreen()
        {
        }

        public AuditScreen(string screenName)
        {
            this.ScreenName = screenName;
        }
    }
}

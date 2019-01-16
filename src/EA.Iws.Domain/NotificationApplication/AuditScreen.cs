namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core.Domain;
    using System;

    public class AuditScreen
    {
        public int Id { get; private set; }
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

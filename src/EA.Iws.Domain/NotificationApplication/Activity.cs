namespace EA.Iws.Domain.Notification
{
    using System;
    using Core.Shared;

    public class Activity
    {
        protected Activity()
        {
        }

        public Guid Id { get; protected set; }

        public TradeDirection TradeDirection { get; protected set; }

        public NotificationType NotificationType { get; protected set; }

        public bool IsInterim { get; protected set; }
    }
}
namespace EA.Iws.DataAccess.Draft
{
    using System;

    internal class Import
    {
        public Import(Guid importNotificationId, string type, string value)
        {
            ImportNotificationId = importNotificationId;
            Type = type;
            Value = value;
        }

        protected Import()
        {
        }

        public int Id { get; protected set; }

        public Guid ImportNotificationId { get; private set; }

        public string Type { get; private set; }

        public string Value { get; set; }
    }
}
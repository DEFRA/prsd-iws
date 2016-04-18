namespace EA.Iws.Core.OperationCodes
{
    using System;
    using Shared;

    [AttributeUsage(AttributeTargets.Field)]
    public class OperationCodeTypeAttribute : Attribute
    {
        public NotificationType OperationType { get; private set; }

        public OperationCodeTypeAttribute(NotificationType operationType)
        {
            OperationType = operationType;
        }
    }
}

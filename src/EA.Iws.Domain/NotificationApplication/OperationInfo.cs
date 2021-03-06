﻿namespace EA.Iws.Domain.NotificationApplication
{
    using Core.OperationCodes;
    using Prsd.Core.Domain;

    public class OperationInfo : Entity
    {
        public OperationCode OperationCode { get; private set; }

        protected OperationInfo()
        {
        }

        internal OperationInfo(OperationCode operationCode)
        {
            OperationCode = operationCode;
        }
    }
}

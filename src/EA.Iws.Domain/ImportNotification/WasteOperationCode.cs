namespace EA.Iws.Domain.ImportNotification
{
    using Prsd.Core.Domain;

    public class WasteOperationCode : Entity
    {
        protected WasteOperationCode()
        {    
        }

        public WasteOperationCode(OperationCode operationCode)
        {
            OperationCode = operationCode;
        }

        public OperationCode OperationCode { get; private set; }
    }
}
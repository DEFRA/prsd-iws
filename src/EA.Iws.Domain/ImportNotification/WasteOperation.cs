namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class WasteOperation : Entity
    {
        protected WasteOperation()
        {
        }

        public WasteOperation(Guid importNotificationId, OperationCodesList operationCodes)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNull(() => operationCodes, operationCodes);

            ImportNotificationId = importNotificationId;
            OperationCodesCollection = new List<WasteOperationCode>(operationCodes);
        }

        public Guid ImportNotificationId { get; private set; }

        protected virtual ICollection<WasteOperationCode> OperationCodesCollection { get; set; }

        public string TechnologyEmployed { get; private set; }

        public IEnumerable<WasteOperationCode> Codes
        {
            get { return OperationCodesCollection.ToSafeIEnumerable(); }
        }

        public void SetTechnologyEmployed(string technologyEmployed)
        {
            TechnologyEmployed = technologyEmployed;
        }

        public void SetOperationCodes(List<WasteOperationCode> operationCodes)
        {
            Guard.ArgumentNotNull(() => operationCodes, operationCodes);

            OperationCodesCollection.Clear();

            OperationCodesCollection = operationCodes;
        }
    }
}
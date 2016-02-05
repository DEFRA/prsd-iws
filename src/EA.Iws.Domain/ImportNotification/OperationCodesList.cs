namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Core.OperationCodes;

    public class OperationCodesList : IEnumerable<WasteOperationCode>
    {
        private readonly List<WasteOperationCode> operationCodes; 

        private OperationCodesList(IEnumerable<OperationCode> operationCodes)
        {
            this.operationCodes = new List<WasteOperationCode>(operationCodes.Select(c => new WasteOperationCode(c)));
        }

        public static OperationCodesList CreateForNotification(ImportNotification notification,
            IEnumerable<OperationCode> operationCodes)
        {
            var codes = operationCodes as OperationCode[] ?? operationCodes.ToArray();

            if (!codes.Any())
            {
                throw new ArgumentException("Operation codes can't be empty", "operationCodes");
            }

            if (!codes.IsUnique())
            {
                throw new ArgumentException("Operation codes must be unique", "operationCodes");
            }

            if (codes.Any(p => !OperationCodeMetadata.IsCodeOfOperationType(p, notification.NotificationType)))
            {
                throw new ArgumentException(
                    string.Format("This notification {0} can only have {1} operation codes.",
                        notification.Id, notification.NotificationType), "operationCodes");
            }

            return new OperationCodesList(codes);
        }

        public IEnumerator<WasteOperationCode> GetEnumerator()
        {
            return operationCodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return operationCodes.GetEnumerator();
        }
    }
}
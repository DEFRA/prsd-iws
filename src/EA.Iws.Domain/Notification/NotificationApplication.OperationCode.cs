namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class NotificationApplication
    {
        public void SetOperationCodes(IEnumerable<OperationCode> operationCodes)
        {
            var operationCodesList = operationCodes as IList<OperationCode> ?? operationCodes.ToList();
            if (operationCodesList.Any(p => p.NotificationType != NotificationType))
            {
                throw new InvalidOperationException(string.Format("This notification {0} can only have {1} operation codes.", Id, NotificationType));
            }

            OperationInfosCollection.Clear();

            foreach (var operationCode in operationCodesList)
            {
                OperationInfosCollection.Add(new OperationInfo(operationCode));
            }
        }
    }
}
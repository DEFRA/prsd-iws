namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;

    public class FinancialGuaranteeFactory
    {
        public static FinancialGuarantee Create(Guid id, Guid notificationId, FinancialGuaranteeStatus status)
        {
            var financialGuarantee = ObjectInstantiator<FinancialGuarantee>.CreateNew();
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Id, id, financialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.NotificationApplicationId, notificationId, financialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, status, financialGuarantee);

            return financialGuarantee;
        }
    }
}

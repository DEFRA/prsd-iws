namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.FinancialGuarantee;
    using FinancialGuarantee;

    [AutoRegister]
    public class NumberOfActiveLoads
    {
        private readonly IMovementRepository movementRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        public NumberOfActiveLoads(IMovementRepository movementRepository, IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.movementRepository = movementRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<bool> HasMaximum(Guid notificationId, DateTime actualMovementDate)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(notificationId);

            var currentFinancialGuarantee =
                financialGuaranteeCollection.FinancialGuarantees.SingleOrDefault(
                    fg => fg.Status == FinancialGuaranteeStatus.Approved);

            var activeShipmentsByDate =
                (await movementRepository.GetAllActiveMovements(notificationId)).Count(
                    m => m.Date.Date == actualMovementDate.Date);

            var activeLoadsPermitted = currentFinancialGuarantee == null ? 0 : currentFinancialGuarantee.ActiveLoadsPermitted.GetValueOrDefault();

            return activeShipmentsByDate >= activeLoadsPermitted;
        }
    }
}

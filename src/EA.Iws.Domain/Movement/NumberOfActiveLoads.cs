namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
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

        public async Task<bool> HasMaximum(Guid notificationId)
        {
            var currentActiveLoads = (await movementRepository.GetActiveMovements(notificationId)).Count();
            var activeLoadsPermitted = (await financialGuaranteeRepository.GetByNotificationId(notificationId)).ActiveLoadsPermitted.GetValueOrDefault();

            return currentActiveLoads >= activeLoadsPermitted;
        }
    }
}

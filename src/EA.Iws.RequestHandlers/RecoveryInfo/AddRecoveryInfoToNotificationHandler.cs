namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;

    public class AddRecoveryInfoToNotificationHandler : IRequestHandler<AddRecoveryInfoToNotification, Guid>
    {
        private readonly IwsContext context;

        public AddRecoveryInfoToNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(AddRecoveryInfoToNotification command)
        {
            var recoveryInfo = await context.GetRecoveryInfoAsync(command.NotificationId);

            var estimatedValue = new EstimatedValue(command.EstimatedUnit, command.EstimatedAmount);
            var recoveryCost = new RecoveryCost(command.CostUnit, command.CostAmount);
            var disposalCost = new DisposalCost(command.DisposalUnit, command.CostAmount);
            if (recoveryInfo == null)
            {
                recoveryInfo = new RecoveryInfo(
                    command.NotificationId,
                    estimatedValue, 
                    recoveryCost,
                    disposalCost);

                context.RecoveryInfos.Add(recoveryInfo);
            }
            else
            {
                recoveryInfo.UpdateEstimatedValue(estimatedValue);
                recoveryInfo.UpdateRecoveryCost(recoveryCost);
                recoveryInfo.UpdateDisposalCost(disposalCost);
            }

            await context.SaveChangesAsync();

            return recoveryInfo.Id;
        }
    }
}

namespace EA.Iws.RequestHandlers.RecoveryInfo
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
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
            var notification = await context.GetNotificationApplication(command.NotificationId);
            RecoveryInfo recoveryInfo;
            if (command.IsDisposal)
            {
                recoveryInfo = notification.SetRecoveryInfoValues(command.EstimatedUnit, command.EstimatedAmount,
                                    command.CostUnit, command.CostAmount, command.DisposalUnit, command.DisposalAmount);
            }
            else
            {
                recoveryInfo = notification.SetRecoveryInfoValues(command.EstimatedUnit, command.EstimatedAmount,
                    command.CostUnit, command.CostAmount, null, null);
            }
            await context.SaveChangesAsync();

            return recoveryInfo.Id;
        }
    }
}

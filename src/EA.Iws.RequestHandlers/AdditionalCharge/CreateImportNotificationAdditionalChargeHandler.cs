namespace EA.Iws.RequestHandlers.AdditionalCharge
{
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Prsd.Core.Mediator;    
    using System;
    using System.Threading.Tasks;

    internal class CreateImportNotificationAdditionalChargeHandler : IRequestHandler<CreateImportNotificationAdditionalCharge, bool>
    {
        private readonly IImportNotificationAdditionalChargeRepository repository;

        public CreateImportNotificationAdditionalChargeHandler(IImportNotificationAdditionalChargeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateImportNotificationAdditionalCharge message)
        {
            await repository.AddImportNotificationAdditionalCharge(this.Map(message));

            return true;
        }

        private AdditionalCharge Map(CreateImportNotificationAdditionalCharge additionalCharge)
        {
            return new AdditionalCharge(additionalCharge.NotificationId,
                                        DateTime.UtcNow,
                                        additionalCharge.ChargeAmount,
                                        (int)additionalCharge.ChangeDetailType,
                                        additionalCharge.Comments);
        }
    }
}

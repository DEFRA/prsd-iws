namespace EA.Iws.RequestHandlers.AdditionalCharge
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;

    public class CreateAdditionalChargeHandler : IRequestHandler<CreateAdditionalCharge, bool>
    {
        private readonly INotificationAdditionalChargeRepository repository;

        public CreateAdditionalChargeHandler(INotificationAdditionalChargeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateAdditionalCharge message)
        {
            await repository.AddAdditionalCharge(this.Map(message));

            return true;
        }

        private AdditionalCharge Map(CreateAdditionalCharge additionalCharge)
        {
            return new AdditionalCharge(additionalCharge.NotificationId,
                                        DateTime.UtcNow,
                                        additionalCharge.ChargeAmount,
                                        (int)additionalCharge.ChangeDetailType,
                                        additionalCharge.Comments);
        }
    }
}

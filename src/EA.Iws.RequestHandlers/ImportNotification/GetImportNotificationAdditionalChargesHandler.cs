namespace EA.Iws.RequestHandlers.ImportNotification
{
    using EA.Iws.Core.ImportNotification.AdditionalCharge;
    using EA.Iws.Domain.ImportNotification;
    using EA.Iws.Requests.ImportNotification;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetImportNotificationAdditionalChargesHandler : IRequestHandler<GetImportNotificationAdditionalCharges, IList<AdditionalChargeForDisplay>>
    {
        private readonly IImportNotificationAdditionalChargeRepository repository;

        public GetImportNotificationAdditionalChargesHandler(IImportNotificationAdditionalChargeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<AdditionalChargeForDisplay>> HandleAsync(GetImportNotificationAdditionalCharges message)
        {
            IEnumerable<AdditionalCharge> additionalCharges = await repository.GetImportNotificationAdditionalChargesById(message.NotificationId);

            var sdditionChargesListData = new List<AdditionalChargeForDisplay>();
            if (additionalCharges != null && additionalCharges.Count() > 0)
            {
                foreach (var additionalCharge in additionalCharges)
                {
                    var charge = new AdditionalChargeForDisplay()
                    {
                        ChangeDetailType = additionalCharge.ChangeDetailType,
                        ChargeAmount = additionalCharge.ChargeAmount,
                        ChargeDate = additionalCharge.ChargeDate,
                        Comments = additionalCharge.Comments
                    };
                    sdditionChargesListData.Add(charge);
                }
            }

            return sdditionChargesListData;
        }
    }
}

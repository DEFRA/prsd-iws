namespace EA.Iws.Web.Infrastructure.AdditionalCharge
{
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    public class AdditionalChargeService : IAdditionalChargeService
    {
        public async Task AddAdditionalCharge(IMediator mediator, CreateAdditionalCharge additionalChargeData)
        {
            await mediator.SendAsync(additionalChargeData);
        }
    }
}
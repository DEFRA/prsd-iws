namespace EA.Iws.Web.Infrastructure.AdditionalCharge
{
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    public interface IAdditionalChargeService
    {
        Task AddAdditionalCharge(IMediator mediator, CreateAdditionalCharge additionalChargeData);

        Task AddImportAdditionalCharge(IMediator mediator, CreateImportNotificationAdditionalCharge importAdditionalChargeData);
    }
}
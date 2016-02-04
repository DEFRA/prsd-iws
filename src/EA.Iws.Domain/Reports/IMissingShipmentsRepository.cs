namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IMissingShipmentsRepository
    {
        Task<IEnumerable<MissingShipment>> Get(int year, UKCompetentAuthority competentAuthority);
    }
}

namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface IMissingShipmentsRepository
    {
        Task<IEnumerable<MissingShipment>> Get(int year, CompetentAuthorityEnum competentAuthority);
    }
}

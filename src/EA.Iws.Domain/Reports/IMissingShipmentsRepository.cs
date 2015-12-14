namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMissingShipmentsRepository
    {
        Task<IEnumerable<MissingShipment>> Get(int year, UKCompetentAuthority competentAuthority);
    }
}

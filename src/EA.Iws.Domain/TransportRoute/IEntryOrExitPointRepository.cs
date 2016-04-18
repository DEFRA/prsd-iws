namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEntryOrExitPointRepository
    {
        Task<EntryOrExitPoint> GetById(Guid id);

        Task<IEnumerable<EntryOrExitPoint>> GetForCountry(Country country);

        Task<IEnumerable<EntryOrExitPoint>> GetForCountry(Guid countryId);

        Task<IEnumerable<EntryOrExitPoint>> GetAll();

        Task Add(EntryOrExitPoint entryOrExitPoint);

        Task<IEnumerable<EntryOrExitPoint>> GetByIds(IEnumerable<Guid> ids);
    }
}

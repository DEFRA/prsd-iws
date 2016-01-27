namespace EA.Iws.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAll();

        Task<Country> GetById(Guid id);

        Task<Country> GetByName(string name);

        Task<Guid> GetUnitedKingdomId();

        Task<IEnumerable<Country>> GetAllHavingCompetentAuthorities();
    }
}

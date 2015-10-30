namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface ICountryRepository
    {
        Task<Country> GetById(Guid id);

        Task<Country> GetByName(string name);
    }
}

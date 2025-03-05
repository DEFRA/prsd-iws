namespace EA.Iws.Api.Client.CompaniesHouseAPI
{
    using EA.Iws.Api.Client.Models;
    using System;
    using System.Threading.Tasks;

    public interface ICompaniesHouseClient : IDisposable
    {
        Task<DefraCompaniesHouseApiModel> GetCompanyDetailsAsync(string endpoint, string companyReference);
    }
}

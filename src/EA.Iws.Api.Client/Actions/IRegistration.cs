namespace EA.Iws.Api.Client.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<HttpResponseMessage> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData);
        Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData);
        Task<OrganisationData[]> SearchOrganisationAsync(string accessToken, string organisationName);
        Task<CountryData[]> GetCountriesAsync();
        Task<HttpResponseMessage> LinkUserToOrganisationAsync(string accessToken, Guid organisationId);
    }
}
namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<HttpResponseMessage> RegisterApplicantAsync(ApplicantRegistrationData applicatRegistrationData);
        Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData);
        Task<OrganisationData[]> SearchOrganisationAsync(string organisationName);
    }
}
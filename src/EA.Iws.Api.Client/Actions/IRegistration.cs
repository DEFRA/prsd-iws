namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;

    public interface IRegistration
    {
        Task<HttpResponseMessage> RegisterApplicantAsync(string accessToken, ApplicantRegistrationData applicatRegistrationData);
        Task<HttpResponseMessage> RegisterOrganisationAsync(string accessToken, OrganisationRegistrationData organisationRegistrationData);
    }
}
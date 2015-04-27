namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;

    public interface IApplicantRegistration
    {
        Task<HttpResponseMessage> RegisterApplicantAsync(string accessToken, ApplicantRegistrationData applicatRegistrationData);
    }
}
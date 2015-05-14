namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;
    using Prsd.Core.Web.ApiClient;

    public interface IRegistration
    {
        Task<ApiResponse<string>> RegisterApplicantAsync(ApplicantRegistrationData applicantRegistrationData);
    }
}
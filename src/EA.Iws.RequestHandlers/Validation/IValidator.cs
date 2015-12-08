namespace EA.Iws.RequestHandlers.Validation
{
    using System.Threading.Tasks;
    using Requests.ImportNotification.Validate;

    internal interface IValidator
    {
        Task<ValidationResults> ValidateAsync<TInstace>(TInstace instance);
    }
}
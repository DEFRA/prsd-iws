namespace EA.Iws.Api.Client
{
    using System.Threading.Tasks;
    using Actions;
    using Prsd.Core.Mediator;

    public interface IIwsClient
    {
        IRegistration Registration { get; }

        Task<TResult> SendAsync<TResult>(IRequest<TResult> request);
        Task<TResult> SendAsync<TResult>(string accessToken, IRequest<TResult> request);
    }
}
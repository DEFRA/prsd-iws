namespace EA.Iws.Api.Client
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Actions;
    using Prsd.Core.Mediator;

    public interface IIwsClient
    {
        IRegistration Registration { get; }

        IErrorLog ErrorLog { get; }

        Task<TResult> SendAsync<TResult>(string accessToken, IRequest<TResult> request);
    }
}
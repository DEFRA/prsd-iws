namespace EA.Iws.Api.Client
{
    using System;
    using System.Threading.Tasks;
    using Actions;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;

    public interface IIwsClient : IDisposable
    {
        IRegistration Registration { get; }
        Task<ApiResponse<TResult>> SendAsync<TResult>(IRequest<TResult> request);
        Task<ApiResponse<TResult>> SendAsync<TResult>(string accessToken, IRequest<TResult> request);
    }
}
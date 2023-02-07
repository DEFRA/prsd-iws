﻿namespace EA.Iws.Api.Client
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Actions;
    using Newtonsoft.Json;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Converters;
    using Prsd.Core.Web.Extensions;

    public class IwsClient : IIwsClient
    {
        private readonly HttpClient httpClient;
        private IRegistration registration;
        private IErrorLog errorLog;

        public IwsClient(string baseUrl, TimeSpan timeout)
        {
            var baseUri = new Uri(baseUrl.EnsureTrailingSlash());

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri, "api/"),
                Timeout = timeout
            };
        }

        public IRegistration Registration
        {
            get { return registration ?? (registration = new Registration(httpClient)); }
        }

        public IErrorLog ErrorLog
        {
            get { return errorLog ?? (errorLog = new ErrorLog(httpClient)); }
        }

        public async Task<TResult> SendAsync<TResult>(string accessToken, IRequest<TResult> request)
        {
            httpClient.SetBearerToken(accessToken);
            return await InternalSendAsync(request).ConfigureAwait(false);
        }

        private async Task<TResult> InternalSendAsync<TResult>(IRequest<TResult> request)
        {
            var apiRequest = new ApiRequest
            {
                RequestJson = JsonConvert.SerializeObject(request),
                TypeName = request.GetType().AssemblyQualifiedName
            };

            var response = await httpClient.PostAsJsonAsync("Send", apiRequest).ConfigureAwait(false);

            var result = await response.CreateResponseAsync<TResult>(new EnumerationConverter()).ConfigureAwait(false);

            return result;
        }
    }
}
namespace EA.Iws.Api.Client.GovUkPay
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using CuttingEdge.Conditions;
    using EA.Iws.Api.Client.HttpClients;
    using EA.Iws.Api.Client.Polly;
    using EA.Iws.Api.Client.Serlializer;
    using Serilog;

    public class PayClient : IPayClient
    {
        private readonly IHttpClientWrapper httpClient;
        private readonly IRetryPolicyWrapper retryPolicy;
        private readonly IJsonSerializer jsonSerializer;
        private readonly ILogger logger;
        private bool disposed;

        public PayClient(string baseUrl,
            string apiKey,
            IHttpClientWrapperFactory httpClientFactory,
            IRetryPolicyWrapper retryPolicy,
            IJsonSerializer jsonSerializer,
            HttpClientHandlerConfig config,
            ILogger logger)
        {
            Condition.Requires(baseUrl).IsNotNullOrWhiteSpace();
            Condition.Requires(apiKey).IsNotNullOrWhiteSpace();
            Condition.Requires(httpClientFactory).IsNotNull();
            Condition.Requires(retryPolicy).IsNotNull();
            Condition.Requires(jsonSerializer).IsNotNull();
            Condition.Requires(config).IsNotNull();
            Condition.Requires(logger).IsNotNull();

            this.httpClient = httpClientFactory.CreateHttpClientWithAuthorization(baseUrl, config, logger, "Bearer", apiKey);
            this.retryPolicy = retryPolicy;
            this.jsonSerializer = jsonSerializer;
            this.logger = logger;
        }

        public async Task<CreatePaymentResult> CreatePayment(CreateCardPaymentRequest request)
        {
            Condition.Requires(request).IsNotNull();

            var idempotencyKey = Guid.NewGuid().ToString();

            try
            {
                var body = jsonSerializer.Serialize(request);

                var response = await retryPolicy.ExecuteAsync(() =>
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, "v1/payments")
                    {
                        Content = new StringContent(body, Encoding.UTF8, "application/json")
                    };
                    httpRequest.Headers.Add("Idempotency-Key", idempotencyKey);

                    return httpClient.SendAsync(httpRequest);
                }).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var payment = jsonSerializer.Deserialize<PaymentWithAllLinks>(responseContent);

                return new CreatePaymentResult
                {
                    PaymentId = payment.PaymentId,
                    NextUrl = payment.Links?.NextUrl?.Href,
                    State = payment.State
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to create GOV.UK Pay payment for reference {Reference}", request.Reference);
                throw;
            }
        }

        public async Task<PaymentWithAllLinks> GetPayment(string paymentId)
        {
            Condition.Requires(paymentId).IsNotNullOrWhiteSpace();

            try
            {
                var response = await retryPolicy.ExecuteAsync(() =>
                    httpClient.GetAsync($"v1/payments/{paymentId}")).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return jsonSerializer.Deserialize<PaymentWithAllLinks>(content);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to retrieve GOV.UK Pay payment {PaymentId}", paymentId);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                (httpClient as IDisposable)?.Dispose();
            }
            disposed = true;
        }
    }
}
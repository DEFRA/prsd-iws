namespace EA.Iws.Api.Client.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json.Linq;

    internal static class HttpResponseMessageExtensions
    {
        public static async Task<Response<T>> CreateResponseAsync<T>(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = new Response<T>(await httpResponseMessage.Content.ReadAsAsync<T>());
                return result;
            }
            else
            {
                var errorResponse = await httpResponseMessage.Content.ReadAsAsync<HttpError>();
                if (errorResponse != null)
                {
                    if (errorResponse.ContainsKey("ModelState"))
                    {
                        var errors = errorResponse["ModelState"] as JObject;
                        if (errors != null)
                        {
                            var listOfErrors = new List<string>();
                            foreach (var error in errors)
                            {
                                foreach (var message in error.Value.Values<string>())
                                {
                                    listOfErrors.Add(message);
                                }
                            }
                            return new Response<T>(listOfErrors.ToArray());
                        }
                    }
                }
                return new Response<T>("Unknown error!");
            }
        }

        public static async Task<Response<byte[]>> CreateResponseByteArrayAsync(
            this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                return new Response<byte[]>(bytes);
            }
            else
            {
                return new Response<byte[]>("Failed to generate the requested document.");
            }
        }
    }
}
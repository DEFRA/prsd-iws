namespace EA.Iws.Api.Client.Actions
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Entities;
    using Prsd.Core.Web.Extensions;

    internal class ErrorLog : IErrorLog
    {
        private const string Controller = "ErrorLog/";
        private readonly HttpClient httpClient;

        public ErrorLog(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Create(ErrorData errorData)
        {
            var response = await httpClient.PostAsJsonAsync(Controller + "Create", errorData);
            return response.IsSuccessStatusCode;
        }

        public async Task<ErrorData> Get(string id)
        {
            var response = await httpClient.GetAsync(Controller + id);
            return await response.CreateResponseAsync<ErrorData>();
        }

        public async Task<PagedErrorDataList> GetList(int pageIndex, int pageSize)
        {
            var response = await httpClient.GetAsync(Controller + "list" + string.Format("?pageIndex={0}&pageSize={1}", pageIndex, pageSize));
            return await response.CreateResponseAsync<PagedErrorDataList>();
        }
    }
}
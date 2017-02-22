namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IErrorLog
    {
        Task<bool> Create(ErrorData errorData);

        Task<ErrorData> Get(string accessToken, string id, string applicationName = "");

        Task<PagedErrorDataList> GetList(string accessToken, int pageIndex, int pageSize, string applicationName = "");
    }
}
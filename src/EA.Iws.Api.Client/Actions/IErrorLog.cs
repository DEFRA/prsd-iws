namespace EA.Iws.Api.Client.Actions
{
    using System.Threading.Tasks;
    using Entities;

    public interface IErrorLog
    {
        Task<bool> Create(ErrorData errorData);

        Task<ErrorData> Get(string id, string applicationName = "");

        Task<PagedErrorDataList> GetList(int pageIndex, int pageSize, string applicationName = "");
    }
}
namespace EA.IWS.Api.Infrastructure.Infrastructure
{
    using EA.Iws.Api.Client.Entities;
    using System;
    using System.Threading.Tasks;

    public interface IElmahSqlLogger
    {
        Task Log(ErrorData errorData);

        Task<ErrorData> GetError(Guid id, string applicationName);

        Task<PagedErrorDataList> GetPagedErrorList(int pageIndex, int pageSize, string applicationName);
    }
}
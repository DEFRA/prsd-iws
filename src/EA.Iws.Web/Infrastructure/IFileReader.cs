namespace EA.Iws.Web.Infrastructure
{
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;

    public interface IFileReader
    {
        Task<byte[]> GetFileBytes(HttpPostedFileBase file, string token);

        Task<DataTable> GetFirstDataTable(HttpPostedFileBase file, bool isCsv, bool useHeaderRow, string token);
    }
}
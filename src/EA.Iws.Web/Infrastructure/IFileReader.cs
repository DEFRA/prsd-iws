namespace EA.Iws.Web.Infrastructure
{
    using System.Threading.Tasks;
    using System.Web;

    public interface IFileReader
    {
        Task<byte[]> GetFileBytes(HttpPostedFileBase file);
    }
}
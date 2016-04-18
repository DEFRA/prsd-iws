namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System.Threading.Tasks;

    public interface IVirusScanner
    {
        ScanResult ScanFile(byte[] fileData);

        Task<ScanResult> ScanFileAsync(byte[] fileData);
    }
}
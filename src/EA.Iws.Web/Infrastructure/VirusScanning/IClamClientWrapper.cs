namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System.Threading.Tasks;
    using nClam;

    public interface IClamClientWrapper
    {
        Task<ClamScanResult> ScanFileAsync(byte[] fileData);
    }
}
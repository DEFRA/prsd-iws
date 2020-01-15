namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;
    using nClam;

    public interface IClamClientWrapper
    {
        Task<ClamScanResult> ScanFileAsync(byte[] fileData);
    }
}
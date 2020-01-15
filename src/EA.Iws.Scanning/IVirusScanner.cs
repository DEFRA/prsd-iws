namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;

    public interface IVirusScanner
    {
        ScanResult ScanFile(byte[] fileData);

        Task<ScanResult> ScanFileAsync(byte[] fileData);
    }
}
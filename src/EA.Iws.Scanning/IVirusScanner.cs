namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;

    public interface IVirusScanner
    {
        Task<ScanResult> ScanFileAsync(byte[] fileData, string token);
    }
}
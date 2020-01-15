namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;

    public interface IWriteFileVirusWrapper
    {
        Task<ScanResult> ScanFile(byte[] fileData, string token);
    }
}
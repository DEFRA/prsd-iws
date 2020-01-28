namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;
    using Api.Client.Actions;

    public interface IIwsScanClient
    {
        Task<ScanResult> ScanAsync(string token, byte[] file);
    }
}
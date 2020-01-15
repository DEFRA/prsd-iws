namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;
    using Api.Client.Actions;

    public interface IIwsScanClient
    {
        IErrorLog ErrorLog { get; }

        Task<ScanResult> ScanAsync(string token, byte[] file);
    }
}
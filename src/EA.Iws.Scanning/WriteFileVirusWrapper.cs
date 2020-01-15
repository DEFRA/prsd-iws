namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;

    public class WriteFileVirusWrapper : IWriteFileVirusWrapper
    {
        private readonly IVirusScanner virusScanner;
        private readonly IIwsScanClient scanClient;
        private readonly bool useLocal;

        public WriteFileVirusWrapper(IVirusScanner virusScanner, IIwsScanClient scanClient, bool useLocal)
        {
            this.virusScanner = virusScanner;
            this.scanClient = scanClient;
            this.useLocal = useLocal;
        }

        public async Task<ScanResult> ScanFile(byte[] fileData, string token)
        {
            if (useLocal)
            {
                return await virusScanner.ScanFileAsync(fileData);
            }

            return await scanClient.ScanAsync(token, fileData);
        }
    }
}

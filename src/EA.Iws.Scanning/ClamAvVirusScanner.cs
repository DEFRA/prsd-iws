namespace EA.Iws.Scanning
{
    using System;
    using System.Threading.Tasks;
    using nClam;

    public class ClamAvVirusScanner : IVirusScanner
    {
        private readonly IClamClientWrapper claimClientWrapper;

        public ClamAvVirusScanner(IClamClientWrapper claimClientWrapper)
        {
            this.claimClientWrapper = claimClientWrapper;
        }

        public ScanResult ScanFile(byte[] fileData)
        {
            throw new NotImplementedException();
        }

        public async Task<ScanResult> ScanFileAsync(byte[] fileData)
        {
            var scan = await claimClientWrapper.ScanFileAsync(fileData);

            switch (scan.Result)
            {
                case ClamScanResults.Clean:
                    return ScanResult.Clean;
                case ClamScanResults.VirusDetected:
                    return ScanResult.Virus;
                case ClamScanResults.Error:
                    return ScanResult.Error;
                case ClamScanResults.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ScanResult.Unknown;
        }
    }
}
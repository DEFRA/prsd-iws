namespace EA.Iws.Scanning
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using EA.Prsd.Core;
    using Elmah;
    using nClam;

    public class ClamClientWrapper : IClamClientWrapper
    {
        private readonly ClamClient client;

        public ClamClientWrapper(string host, int port)
        {
            Guard.ArgumentNotNullOrEmpty(() => host, host);
            
            client = new ClamClient(host, port);
        }

        public async Task<ClamScanResult> ScanFileAsync(byte[] fileData)
        {
            try
            {
                return await client.SendAndScanFileAsync(fileData);
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));

                throw;
            }
        }
    }
}
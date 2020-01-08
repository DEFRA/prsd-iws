namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Elmah;
    using nClam;
    using Prsd.Core;
    using Services;

    public class ClamClientWrapper : IClamClientWrapper
    {
        private readonly ClamClient client;

        public ClamClientWrapper(AppConfiguration config)
        {
            Guard.ArgumentNotNullOrEmpty(() => config.ClamAvHost, config.ClamAvHost);
            
            client = new ClamClient(config.ClamAvHost);
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
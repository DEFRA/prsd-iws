namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Services;

    /// <summary>
    ///     This implementation assumes that writing a file to disk will result in a virus
    ///     scan by the system. So write a file and attempt to read it back. If it still
    ///     exists then the file has not been quarantined, so was not a virus.
    /// </summary>
    public class WriteFileVirusScanner : IVirusScanner
    {
        private readonly AppConfiguration config;
        private readonly IFileAccess fileAccess;
        private readonly TimeSpan fileWriteTimeout;

        public WriteFileVirusScanner(AppConfiguration config, IFileAccess fileAccess)
        {
            this.config = config;
            this.fileAccess = fileAccess;
            fileWriteTimeout = TimeSpan.FromMilliseconds(config.FileSafeTimerMilliseconds);
        }

        public ScanResult ScanFile(byte[] fileData)
        {
            var fileName = GetTempFileName();

            try
            {
                fileAccess.WriteFile(fileName, fileData);
            }
            catch (IOException ex)
            {
                return ScanResult.Error;
            }

            Thread.Sleep(fileWriteTimeout);

            return GetScanResult(fileName);
        }

        public async Task<ScanResult> ScanFileAsync(byte[] fileData)
        {
            var fileName = GetTempFileName();

            try
            {
                await fileAccess.WriteFileAsync(fileName, fileData);
            }
            catch (IOException ex)
            {
                return ScanResult.Error;
            }

            await Task.Delay(fileWriteTimeout);

            return GetScanResult(fileName);
        }

        private string GetTempFileName()
        {
            return Path.Combine(fileAccess.MapPath(config.FileUploadTempPath), Guid.NewGuid().ToString());
        }

        private ScanResult GetScanResult(string fileName)
        {
            if (fileAccess.FileExists(fileName))
            {
                try
                {
                    fileAccess.DeleteFile(fileName);
                }
                catch (IOException ex)
                {
                    // If we can't delete the file just leave it
                }

                return ScanResult.Clean;
            }
            return ScanResult.Virus;
        }
    }
}
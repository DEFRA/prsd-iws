namespace EA.Iws.Scanning
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     This implementation assumes that writing a file to disk will result in a virus
    ///     scan by the system. So write a file and attempt to read it back. If it still
    ///     exists then the file has not been quarantined, so was not a virus.
    /// </summary>
    public class WriteFileVirusScanner : IVirusScanner
    {
        private readonly IFileAccess fileAccess;
        private readonly TimeSpan fileWriteTimeout;
        private readonly string tempPath;

        public WriteFileVirusScanner(Double timeout, IFileAccess fileAccess, string tempPath)
        {
            this.fileAccess = fileAccess;
            this.tempPath = tempPath;
            fileWriteTimeout = TimeSpan.FromMilliseconds(timeout);
        }

        public ScanResult ScanFile(byte[] fileData)
        {
            var fileName = GetTempFileName();

            try
            {
                fileAccess.WriteFile(fileName, fileData);
            }
            catch (IOException)
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
            catch (IOException)
            {
                return ScanResult.Error;
            }

            await Task.Delay(fileWriteTimeout);

            return GetScanResult(fileName);
        }

        private string GetTempFileName()
        {
            return Path.Combine(fileAccess.MapPath(tempPath), Guid.NewGuid().ToString());
        }

        private ScanResult GetScanResult(string fileName)
        {
            if (fileAccess.FileExists(fileName))
            {
                try
                {
                    fileAccess.DeleteFile(fileName);
                }
                catch (IOException)
                {
                    // If we can't delete the file just leave it
                }

                return ScanResult.Clean;
            }
            return ScanResult.Virus;
        }
    }
}
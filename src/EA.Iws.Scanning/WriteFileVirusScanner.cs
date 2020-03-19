namespace EA.Iws.Scanning
{
    using System;
    using System.IO;
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

        public WriteFileVirusScanner(double timeout, IFileAccess fileAccess, string tempPath)
        {
            this.fileAccess = fileAccess;
            this.tempPath = tempPath;
            fileWriteTimeout = TimeSpan.FromMilliseconds(timeout);
        }

        public async Task<ScanResult> ScanFileAsync(byte[] fileData, string accessToken)
        {
            var fileName = fileAccess.GetTemporaryFileName(tempPath);

            try
            {
                await fileAccess.WriteFileAsync(fileName, fileData);
            }
            catch
            {
                throw new IOException("Virus scan could not write to file system");
            }

            await Task.Delay(fileWriteTimeout);

            return GetScanResult(fileName);
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
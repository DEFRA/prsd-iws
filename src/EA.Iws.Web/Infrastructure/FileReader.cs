namespace EA.Iws.Web.Infrastructure
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using VirusScanning;

    internal class FileReader : IFileReader
    {
        private readonly IVirusScanner virusScanner;

        public FileReader(IVirusScanner virusScanner)
        {
            this.virusScanner = virusScanner;
        }

        public async Task<byte[]> GetFileBytes(HttpPostedFileBase file)
        {
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                throw new VirusFoundException(string.Format("Virus found in file {0}", file.FileName));
            }

            return fileBytes;
        }
    }
}
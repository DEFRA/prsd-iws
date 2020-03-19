namespace EA.Iws.Scanning
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Hosting;

    public class FileAccess : IFileAccess
    {
        public async Task WriteFileAsync(string fileName, byte[] fileBytes)
        {
            EnsureDirectoryExists(fileName);

            using (var fs = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write, FileShare.Read))
            {
                await fs.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
        }

        public void WriteFile(string fileName, byte[] fileBytes)
        {
            EnsureDirectoryExists(fileName);

            using (var fs = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write, FileShare.Read))
            {
                fs.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }

        public string GetTemporaryFileName(string tempPath)
        {
            return Path.Combine(MapPath(tempPath), Guid.NewGuid().ToString());
        }

        private static void EnsureDirectoryExists(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
    }
}
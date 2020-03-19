namespace EA.Iws.Scanning
{
    using System.Threading.Tasks;

    public interface IFileAccess
    {
        Task WriteFileAsync(string fileName, byte[] fileBytes);

        void WriteFile(string fileName, byte[] fileBytes);

        bool FileExists(string filePath);

        void DeleteFile(string filePath);

        string MapPath(string virtualPath);

        string GetTemporaryFileName(string tempPath);
    }
}
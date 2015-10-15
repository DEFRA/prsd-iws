namespace EA.Iws.Core.Documents
{
    public class FileData
    {
        public FileData(string fileName, FileType fileType, byte[] content)
        {
            FileName = fileName;
            FileType = fileType;
            Content = content;
        }

        public string FileName { get; private set; }

        public FileType FileType { get; private set; }

        public byte[] Content { get; private set; }

        public string FileNameWithExtension
        {
            get { return string.Format("{0}.{1}", FileName, FileType.ToString().ToLowerInvariant()); }
        }
    }
}
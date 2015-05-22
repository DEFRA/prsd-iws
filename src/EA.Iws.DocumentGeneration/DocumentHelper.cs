namespace EA.Iws.DocumentGeneration
{
    using System;
    using System.IO;

    internal static class DocumentHelper
    {
        public static MemoryStream ReadDocumentStreamShared(string fileName)
        {
            var fullPath = Path.Combine(GetDocumentDirectory(), fileName);

            var inMemoryCopy = new MemoryStream();

            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.CopyTo(inMemoryCopy);
            }

            return inMemoryCopy;
        }

        /// <summary>
        /// Gets the location of the document templates assuming they have been compiled to the bin.
        /// </summary>
        /// <returns>The full path name of the document directory ending in "\"</returns>
        private static string GetDocumentDirectory()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(root, "Documents");
        }
    }
}
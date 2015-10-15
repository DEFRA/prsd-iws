namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;

    // Simplified from https://github.com/khellang/MimeTypes to only include file extensions
    // we are using.
    public static class MimeTypeHelper
    {
        private static readonly Dictionary<string, string> TypeMap;

        static MimeTypeHelper()
        {
            FallbackMimeType = Prsd.Core.Web.Constants.ByteArrayResponseContentType;

            TypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "docx", Prsd.Core.Web.Constants.MicrosoftWordContentType },
                { "pdf", "application/pdf" }
            };
        }

        public static string FallbackMimeType { get; set; }

        public static string GetMimeType(string fileName)
        {
            var dotIndex = fileName.LastIndexOf('.');
            if (dotIndex != -1 && fileName.Length > dotIndex + 1)
            {
                string result;
                if (TypeMap.TryGetValue(fileName.Substring(dotIndex + 1), out result))
                {
                    return result;
                }
            }
            return FallbackMimeType;
        }
    }
}
﻿namespace EA.Iws.DocumentGeneration
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

        /// <summary>
        /// Formats Telephone and Fax number for merge document.
        /// Ensures non empty numbers starts with + sign.
        /// Country code and number is separated by space.
        /// </summary>
        /// <param name="contactNumber">Telephone or Fax number (Country code and number separated by - sign).</param>
        /// <returns>Formatted contact number. For null, empty or whitespace only, it will return string.empty.</returns>
        public static string ToFormattedContact(this string contactNumber)
        {
            const string countryCodePrefix = "+";
            const string countryCodeSeparator = "-";

            if (string.IsNullOrWhiteSpace(contactNumber))
            {
                return string.Empty;
            }

            if (!contactNumber.StartsWith(countryCodePrefix))
            {
                contactNumber = countryCodePrefix + contactNumber;
            }

            if (contactNumber.Contains(countryCodeSeparator))
            {
                return contactNumber.Replace(countryCodeSeparator, " ");
            }
            return contactNumber;
        }
    }
}
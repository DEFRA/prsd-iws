namespace EA.Iws.DocumentGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    internal static class RunLocator
    {
        private static readonly Func<WordprocessingDocument, string, IEnumerable<Run>> FindRunsCaseInsensitive =
            (document, content) => document.MainDocumentPart.Document.Descendants<Run>()
                .Where(r => r.InnerText.Equals(content, StringComparison.OrdinalIgnoreCase));

        public static Run FirstRunByContent(WordprocessingDocument document, string content)
        {
            return FindRunsCaseInsensitive(document, content).First();
        }

        public static Run SingleRunByContent(WordprocessingDocument document, string content)
        {
            return FindRunsCaseInsensitive(document, content).Single();
        }
    }
}

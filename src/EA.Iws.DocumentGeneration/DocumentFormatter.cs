namespace EA.Iws.DocumentGeneration
{
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain;

    internal static class DocumentFormatter
    {
        public static void ApplyFormatting(WordprocessingDocument document, ShipmentQuantityUnits shipmentQuantityUnits)
        {
            // Gets each element in the document and applies formatting (e.g strikethrough) if required
            foreach (var run in document.MainDocumentPart.Document.Descendants<Run>())
            {
                if (run.InnerText.Contains("TonneFieldName"))
                {
                    if (shipmentQuantityUnits != ShipmentQuantityUnits.Tonnes)
                    {
                        run.AppendChild(new RunProperties(new Strike() { Val = true }));
                    }
                    var t = new Text(ReplaceText(run, "Tonnes (Mg):"));
                    run.RemoveAllChildren<Text>();
                    run.AppendChild(t);
                }

                if (run.InnerText.Contains("cubfn"))
                {
                    if (shipmentQuantityUnits != ShipmentQuantityUnits.CubicMetres)
                    {
                        run.AppendChild(new RunProperties(new Strike() { Val = true }));
                    }
                    var t = new Text(ReplaceText(run, "m³:"));
                    run.RemoveAllChildren<Text>();
                    run.AppendChild(t);
                }
            }
        }

        private static string ReplaceText(Run run, string text)
        {
            var innerText = run.InnerText;
            return run.InnerText.Replace(innerText, text);
        }
    }
}

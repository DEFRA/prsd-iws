namespace EA.Iws.DocumentGeneration.Formatters
{
    using Core.Shared;

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain.NotificationApplication;

    public class ShipmentQuantityUnitFormatter
    {
        private const string TonnesFieldName = "TonneFieldName";
        private const string CubicMetresFieldName = "cubfn";

        private const string TonnesFieldText = "Tonnes (Mg):";
        private const string CubicMetresFieldText = "m\u00B3";

        public static void ApplyStrikethroughFormattingToUnits(WordprocessingDocument document, ShipmentInfo shipmentInfo)
        {
            if (shipmentInfo == null)
            {
                return;
            }

            ApplyStrikethroughFormattingToUnits(document, shipmentInfo.Units);
        }

        public static void ApplyStrikethroughFormattingToUnits(WordprocessingDocument document,
            ShipmentQuantityUnits unitsToDisplay)
        {
            var tonnesField = RunLocator.SingleRunByContent(document, TonnesFieldName);
            var cubicMetresField = RunLocator.SingleRunByContent(document, CubicMetresFieldName);

            if (unitsToDisplay != ShipmentQuantityUnits.Tonnes)
            {
                tonnesField.AppendChild(GetStrikethroughRunProperties());
            }

            if (unitsToDisplay != ShipmentQuantityUnits.CubicMetres)
            {
                cubicMetresField.AppendChild(GetStrikethroughRunProperties());
            }

            ReplacePlaceholderText(tonnesField, TonnesFieldText);
            ReplacePlaceholderText(cubicMetresField, CubicMetresFieldText);
        }

        private static RunProperties GetStrikethroughRunProperties()
        {
            return new RunProperties(new Strike { Val = true });
        }

        private static void ReplacePlaceholderText(Run run, string text)
        {
            var t = new Text(ReplaceText(run, text));
            run.RemoveAllChildren<Text>();
            run.AppendChild(t);
        }

        private static string ReplaceText(Run run, string text)
        {
            var innerText = run.InnerText;
            return run.InnerText.Replace(innerText, text);
        }
    }
}

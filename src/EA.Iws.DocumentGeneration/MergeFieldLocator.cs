namespace EA.Iws.DocumentGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    internal class MergeFieldLocator
    {
        public static IList<MergeField> GetMergeRuns(WordprocessingDocument document)
        {
            IList<MergeField> matchingRuns = new List<MergeField>();

            // Gets each element in the document with text representing a merge field.
            foreach (var run in document.MainDocumentPart.Document.Descendants<Run>())
            {
                if (run.InnerText.StartsWith(MergeField.StartMergeField.ToString())
                    && run.InnerText.EndsWith(MergeField.EndMergeField.ToString()))
                {
                    matchingRuns.Add(ConvertRunToMergeField(run));
                }
            }

            return matchingRuns;
        }

        public static IEnumerable<MergeField> GetMergeRuns(OpenXmlElement element)
        {
            // Gets each element under the element with text representing a merge field.
            foreach (var run in element.Descendants<Run>())
            {
                if (run.InnerText.StartsWith(MergeField.StartMergeField.ToString())
                    && run.InnerText.EndsWith(MergeField.EndMergeField.ToString()))
                {
                    yield return ConvertRunToMergeField(run);
                }
            }
        }

        public static MergeField ConvertRunToMergeField(Run run)
        {
            return new MergeField(run, run.InnerText.TrimStart(MergeField.StartMergeField).TrimEnd(MergeField.EndMergeField));
        }

        public static void RemoveDataSourceSettingFromMergedDocument(WordprocessingDocument document)
        {
            // If the Document has settings remove them so the end user doesn't get prompted to use the data source
            DocumentSettingsPart settingsPart = document.MainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();

            var oxeSettings = settingsPart.Settings.FirstOrDefault(a => a.LocalName == "mailMerge");

            if (oxeSettings != null)
            {
                settingsPart.Settings.RemoveChild(oxeSettings);

                settingsPart.Settings.Save();
            }
        }

        public static void MergeNamedField(string namedField, string valueToMerge, WordprocessingDocument document)
        {
            var matchingFields = document.MainDocumentPart.Document
                .Descendants<Run>()
                .Where(r => r.InnerText.StartsWith(MergeField.StartMergeField.ToString())
                            && r.InnerText.EndsWith(MergeField.EndMergeField.ToString())
                            && r.InnerText.Contains(namedField))
                .Select(ConvertRunToMergeField);

            foreach (var mergeField in matchingFields)
            {
                mergeField.RemoveCurrentContents();
                mergeField.SetText(valueToMerge);
            }
        }

        public static IList<MergeField> GetCorrespondingFieldsForBlock(IList<MergeField> allMergeFields, string blockName)
        {
            return allMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                && mf.FieldName.OuterTypeName.Equals(blockName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public static List<MergeField> GetAnnexMergeFields(IList<MergeField> allMergeFields, string blockName)
        {
            // Gets annex fields and changes them to act like normal merge fields where the field name no longer has "Annex" in.
            var annexFields = allMergeFields.Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                && mf.FieldName.OuterTypeName.Equals("Annex", StringComparison.InvariantCultureIgnoreCase))
                .Select(ConvertAnnexMergeFieldToRegularMergeField)
                .Where(mf => !string.IsNullOrWhiteSpace(mf.FieldName.OuterTypeName)
                && mf.FieldName.OuterTypeName.Equals(blockName, StringComparison.InvariantCultureIgnoreCase));

            return annexFields.ToList();
        }

        public static MergeField ConvertAnnexMergeFieldToRegularMergeField(MergeField annexMergeField)
        {
            return new MergeField(annexMergeField.Run, annexMergeField.FieldName.InnerTypeName);
        }
    }
}

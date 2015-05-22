namespace EA.Iws.DocumentGeneration
{
    using System;
    using DocumentFormat.OpenXml.Wordprocessing;

    public class MergeField
    {
        public static readonly char StartMergeField = '«';
        public static readonly char EndMergeField = '»';

        public MergeField(Run run, string fieldName)
        {
            Run = run;
            FieldName = new MergeFieldName(fieldName);

            if (FieldName.InnerTypeName.StartsWith("Is"))
            {
                FieldType = MergeFieldType.Checkbox;
            }
            else
            {
                FieldType = MergeFieldType.Text;
            }
        }

        public Run Run { get; private set; }

        public MergeFieldName FieldName { get; private set; }

        public MergeFieldType FieldType { get; private set; }

        public void SetText(string text, int numberOfLineBreaks = 1)
        {
            string[] lines = { text };

            if (text.Contains(Environment.NewLine))
            {
                lines = text.Split(new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
            }

            for (var i = 0; i < lines.Length; i++)
            {
                Run.AppendChild(new Text(lines[i]));

                if (i < lines.Length - 1)
                {
                    for (var j = 0; j < numberOfLineBreaks; j++)
                    {
                        Run.Append(new Break());
                    }
                }
            }
        }

        public void RemoveCurrentContents()
        {
            Run.RemoveAllChildren<Text>();
            Run.RemoveAllChildren<MailMerge>();
        }
    }
}
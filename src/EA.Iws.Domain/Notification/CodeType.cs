namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class CodeType : Enumeration
    {
        public static readonly CodeType Basel = new CodeType(1, "Basel");
        public static readonly CodeType Oecd = new CodeType(2, "OECD");
        public static readonly CodeType Ewc = new CodeType(3, "EWC");
        public static readonly CodeType ExportCode = new CodeType(7, "Export Code");
        public static readonly CodeType ImportCode = new CodeType(8, "Import Code");
        public static readonly CodeType OtherCode = new CodeType(9, "Other Code");
        public static readonly CodeType CustomCode = new CodeType(10, "Custom Code");
        public static readonly CodeType UnCode = new CodeType(11, "UN Number");

        protected CodeType()
        {
        }

        private CodeType(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
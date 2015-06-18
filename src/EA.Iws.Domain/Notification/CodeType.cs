namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class CodeType : Enumeration
    {
        public static readonly CodeType Basel = new CodeType(1, "Basel");
        public static readonly CodeType Oecd = new CodeType(2, "OECD");
        public static readonly CodeType Ewc = new CodeType(3, "EWC");

        protected CodeType()
        {
        }

        private CodeType(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
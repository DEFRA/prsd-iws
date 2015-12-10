namespace EA.Iws.Domain
{
    using Core.Shared;
    using Prsd.Core.Domain;

    public class OperationCode : Enumeration
    {
        private readonly NotificationType notificationType;

        public NotificationType NotificationType
        {
            get { return notificationType; }
        }

        public static readonly OperationCode R1 = new OperationCode(1, "R1", NotificationType.Recovery);
        public static readonly OperationCode R2 = new OperationCode(2, "R2", NotificationType.Recovery);
        public static readonly OperationCode R3 = new OperationCode(3, "R3", NotificationType.Recovery);
        public static readonly OperationCode R4 = new OperationCode(4, "R4", NotificationType.Recovery);
        public static readonly OperationCode R5 = new OperationCode(5, "R5", NotificationType.Recovery);
        public static readonly OperationCode R6 = new OperationCode(6, "R6", NotificationType.Recovery);
        public static readonly OperationCode R7 = new OperationCode(7, "R7", NotificationType.Recovery);
        public static readonly OperationCode R8 = new OperationCode(8, "R8", NotificationType.Recovery);
        public static readonly OperationCode R9 = new OperationCode(9, "R9", NotificationType.Recovery);
        public static readonly OperationCode R10 = new OperationCode(10, "R10", NotificationType.Recovery);
        public static readonly OperationCode R11 = new OperationCode(11, "R11", NotificationType.Recovery);
        public static readonly OperationCode R12 = new OperationCode(12, "R12", NotificationType.Recovery);
        public static readonly OperationCode R13 = new OperationCode(13, "R13", NotificationType.Recovery);

        public static readonly OperationCode D1 = new OperationCode(14, "D1", NotificationType.Disposal);
        public static readonly OperationCode D2 = new OperationCode(15, "D2", NotificationType.Disposal);
        public static readonly OperationCode D3 = new OperationCode(16, "D3", NotificationType.Disposal);
        public static readonly OperationCode D4 = new OperationCode(17, "D4", NotificationType.Disposal);
        public static readonly OperationCode D5 = new OperationCode(18, "D5", NotificationType.Disposal);
        public static readonly OperationCode D6 = new OperationCode(19, "D6", NotificationType.Disposal);
        public static readonly OperationCode D7 = new OperationCode(20, "D7", NotificationType.Disposal);
        public static readonly OperationCode D8 = new OperationCode(21, "D8", NotificationType.Disposal);
        public static readonly OperationCode D9 = new OperationCode(22, "D9", NotificationType.Disposal);
        public static readonly OperationCode D10 = new OperationCode(23, "D10", NotificationType.Disposal);
        public static readonly OperationCode D11 = new OperationCode(24, "D11", NotificationType.Disposal);
        public static readonly OperationCode D12 = new OperationCode(25, "D12", NotificationType.Disposal);
        public static readonly OperationCode D13 = new OperationCode(26, "D13", NotificationType.Disposal);
        public static readonly OperationCode D14 = new OperationCode(27, "D14", NotificationType.Disposal);
        public static readonly OperationCode D15 = new OperationCode(28, "D15", NotificationType.Disposal);

        protected OperationCode()
        {
        }

        private OperationCode(int value, string displayname, NotificationType notificationType)
            : base(value, displayname)
        {
            this.notificationType = notificationType;
        }
    }
}

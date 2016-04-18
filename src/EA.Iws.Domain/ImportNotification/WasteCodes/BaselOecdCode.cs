namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using NotificationApplication;
    using Prsd.Core;

    public class BaselOecdCode
    {
        public bool NotListed { get; private set; }

        public WasteTypeWasteCode Code { get; private set; }

        private BaselOecdCode()
        {
        }

        public BaselOecdCode(WasteCode code)
        {
            if (code.CodeType != CodeType.Basel && code.CodeType != CodeType.Oecd)
            {
                throw new ArgumentException(string.Format(
                    "Supplied code type {0} is not basel or oecd",
                    code.CodeType));
            }

            Code = new WasteTypeWasteCode(code.Id);
        }

        public static BaselOecdCode CreateNotListed()
        {
            return new BaselOecdCode
            {
                NotListed = true
            };
        }

        public static BaselOecdCode CreateFor(WasteCode code)
        {
            Guard.ArgumentNotNull(() => code, code);

            return new BaselOecdCode(code);
        }
    }
}
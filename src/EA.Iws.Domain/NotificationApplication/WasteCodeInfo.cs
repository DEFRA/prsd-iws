namespace EA.Iws.Domain.Notification
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class WasteCodeInfo : Entity
    {
        protected WasteCodeInfo()
        {
        }

        private WasteCodeInfo(WasteCode wasteCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);
            WasteCode = wasteCode;
        }

        private WasteCodeInfo(WasteCode wasteCode, string customCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);
            Guard.ArgumentNotNullOrEmpty(() => customCode, customCode);

            if (!CanHaveCustomCode(wasteCode.CodeType))
            {
                throw new InvalidOperationException(string.Format("Cannot set optional values for non optional code type for notification {0}", Id));
            }

            WasteCode = wasteCode;
            CustomCode = customCode;
        }

        public static WasteCodeInfo CreateCustomWasteCodeInfo(WasteCode wasteCode, string customCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);
            Guard.ArgumentNotNullOrEmpty(() => customCode, customCode);

            return new WasteCodeInfo(wasteCode, customCode);
        }

        public static WasteCodeInfo CreateWasteCodeInfo(WasteCode wasteCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);

            return new WasteCodeInfo(wasteCode);
        }

        public virtual WasteCode WasteCode { get; private set; }

        public string CustomCode { get; private set; }

        private static bool CanHaveCustomCode(CodeType codeType)
        {
            return codeType == CodeType.CustomsCode || codeType == CodeType.ExportCode || codeType == CodeType.ImportCode || codeType == CodeType.OtherCode;
        }
    }
}
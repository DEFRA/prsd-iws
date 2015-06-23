namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core.Domain;

    public class WasteCodeInfo : Entity
    {
        protected WasteCodeInfo()
        {
        }

        private WasteCodeInfo(WasteCode wasteCode)
        {
            WasteCode = wasteCode;
        }

        private WasteCodeInfo(WasteCode wasteCode, string optionalCode, string optionalDescription)
        {
            if (!IsOptional(wasteCode.CodeType))
            {
                throw new InvalidOperationException(string.Format("Cannot set optional values for non optional code type for notification {0}", Id));
            }

            WasteCode = wasteCode;
            OptionalDescription = optionalCode;
            OptionalDescription = optionalDescription;
        }

        public static WasteCodeInfo CreateOptionalWasteCodeInfo(WasteCode wasteCode, string optionalCode, string optionalDescription)
        {
            return new WasteCodeInfo(wasteCode, optionalCode, optionalDescription);
        }

        public static WasteCodeInfo CreateWasteCodeInfo(WasteCode wasteCode)
        {
            return new WasteCodeInfo(wasteCode);
        }

        public virtual WasteCode WasteCode { get; internal set; }

        public string OptionalDescription { get; internal set; }

        public string OptionalCode { get; internal set; }

        public bool IsOptional(CodeType codeType)
        {
            return codeType == CodeType.CustomCode || codeType == CodeType.ExportCode || codeType == CodeType.ImportCode || codeType == CodeType.OtherCode;
        }
    }
}
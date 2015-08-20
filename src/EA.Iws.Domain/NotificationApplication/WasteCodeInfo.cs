namespace EA.Iws.Domain.NotificationApplication
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
            CodeType = wasteCode.CodeType;
        }

        private WasteCodeInfo(CodeType codeType, string customCode)
        {
            Guard.ArgumentNotNullOrEmpty(() => customCode, customCode);

            CodeType = codeType;

            if (!CanHaveCustomCode(codeType))
            {
                throw new InvalidOperationException(
                    string.Format("Cannot set optional values for non optional code type for notification {0}", Id));
            }

            CustomCode = customCode;
        }

        public virtual WasteCode WasteCode { get; protected set; }

        public string CustomCode { get; protected set; }

        public bool IsNotApplicable { get; protected set; }

        public CodeType CodeType { get; protected set; }

        public static WasteCodeInfo CreateCustomWasteCodeInfo(CodeType codeType, string customCode)
        {
            Guard.ArgumentNotNullOrEmpty(() => customCode, customCode);

            return new WasteCodeInfo(codeType, customCode);
        }

        public static WasteCodeInfo CreateWasteCodeInfo(WasteCode wasteCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);

            return new WasteCodeInfo(wasteCode);
        }

        public static WasteCodeInfo CreateNotApplicableCodeInfo(CodeType codeType)
        {
            return new WasteCodeInfo
            {
                CodeType = codeType,
                IsNotApplicable = true,
            };
        }

        private static bool CanHaveCustomCode(CodeType codeType)
        {
            return codeType == CodeType.CustomsCode 
                || codeType == CodeType.ExportCode 
                || codeType == CodeType.ImportCode 
                || codeType == CodeType.OtherCode;
        }
    }
}
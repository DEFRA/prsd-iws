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

        private WasteCodeInfo(WasteCode wasteCode, string customCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);

            CodeType = wasteCode.CodeType;

            if (string.IsNullOrEmpty(customCode))
            {
                IsNotApplicable = true;
            }

            if (!CanHaveCustomCode(wasteCode.CodeType))
            {
                throw new InvalidOperationException(
                    string.Format("Cannot set optional values for non optional code type for notification {0}", Id));
            }

            WasteCode = wasteCode;
            CustomCode = customCode;
        }

        public virtual WasteCode WasteCode { get; protected set; }

        public string CustomCode { get; protected set; }

        public bool IsNotApplicable { get; protected set; }

        public CodeType CodeType { get; protected set; }

        public static WasteCodeInfo CreateCustomWasteCodeInfo(WasteCode wasteCode, string customCode)
        {
            Guard.ArgumentNotNull(() => wasteCode, wasteCode);

            return new WasteCodeInfo(wasteCode, customCode);
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
            return codeType == CodeType.CustomsCode || codeType == CodeType.ExportCode ||
                   codeType == CodeType.ImportCode || codeType == CodeType.OtherCode;
        }
    }
}
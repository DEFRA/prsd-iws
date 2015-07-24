namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.WasteCodes;

    public partial class NotificationApplication
    {
        private IEnumerable<WasteCodeInfo> GetWasteCodes(CodeType codeType)
        {
            return WasteCodeInfoCollection.Where(p => p.WasteCode.CodeType == codeType);
        }

        private void SetCodes(IEnumerable<WasteCodeInfo> codes, CodeType codeType)
        {
            var newCodes = codes as WasteCodeInfo[] ?? codes.ToArray();

            if (codeType != CodeType.CustomsCode && newCodes.Select(p => p.WasteCode.Id).Distinct().Count() != newCodes.Count())
            {
                throw new InvalidOperationException(
                    string.Format("The same code cannot be entered twice for notification {0}", Id));
            }

            if (newCodes.Any(p => p.WasteCode.CodeType != codeType))
            {
                throw new InvalidOperationException(string.Format("All codes must be of type {0} for notification {1}", codeType, Id));
            }

            var existingCodes = GetWasteCodes(codeType).ToArray();
            foreach (var code in existingCodes)
            {
                WasteCodeInfoCollection.Remove(code);
            }

            foreach (var code in newCodes)
            {
                WasteCodeInfoCollection.Add(code);
            }
        }

        private void SetCode(WasteCodeInfo code, CodeType codeType)
        {
            if (code.WasteCode.CodeType != codeType)
            {
                throw new InvalidOperationException(string.Format("This method can only set {0} code for this notification {1}", codeType, Id));
            }

            var existingCode = GetWasteCodes(codeType).SingleOrDefault();
            if (existingCode != null)
            {
                WasteCodeInfoCollection.Remove(existingCode);
            }

            WasteCodeInfoCollection.Add(code);
        }

        public void SetBaselOecdCode(WasteCodeInfo wasteCodeInfo)
        {
            if (wasteCodeInfo.WasteCode.CodeType != CodeType.Basel && wasteCodeInfo.WasteCode.CodeType != CodeType.Oecd)
            {
                throw new InvalidOperationException(string.Format("This method can only set Basel or OECD codes for this notification {0}", Id));
            }

            var baselOecdCode = BaselOecdCode;
            if (baselOecdCode != null)
            {
                WasteCodeInfoCollection.Remove(baselOecdCode);
            }

            WasteCodeInfoCollection.Add(wasteCodeInfo);
        }

        public void SetExportCode(WasteCodeInfo wasteCodeInfo)
        {
            SetCode(wasteCodeInfo, CodeType.ExportCode);
        }

        public void SetImportCode(WasteCodeInfo wasteCodeInfo)
        {
            SetCode(wasteCodeInfo, CodeType.ImportCode);
        }

        public void SetOtherCode(WasteCodeInfo wasteCodeInfo)
        {
            SetCode(wasteCodeInfo, CodeType.OtherCode);
        }

        public void RemoveOtherCode()
        {
            if (OtherCode != null)
            {
                WasteCodeInfoCollection.Remove(OtherCode);
            }
        }

        public void SetEwcCodes(IEnumerable<WasteCodeInfo> ewcCodes)
        {
            SetCodes(ewcCodes, CodeType.Ewc);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variable relates to Y-Codes")]
        public void SetYCodes(IEnumerable<WasteCodeInfo> yCodes)
        {
            SetCodes(yCodes, CodeType.Y);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variable relates to H-Codes")]
        public void SetHCodes(IEnumerable<WasteCodeInfo> yCodes)
        {
            SetCodes(yCodes, CodeType.H);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variable relates to UN class")]
        public void SetUnClasses(IEnumerable<WasteCodeInfo> unClasses)
        {
            SetCodes(unClasses, CodeType.Un);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variable relates to UN number")]
        public void SetUnNumbers(IEnumerable<WasteCodeInfo> unNumbers)
        {
            SetCodes(unNumbers, CodeType.UnNumber);
        }

        public void SetCustomsCodes(IEnumerable<WasteCodeInfo> customsCodes)
        {
            SetCodes(customsCodes, CodeType.CustomsCode);
        }

        public WasteCodeInfo BaselOecdCode
        {
            get
            {
                return
                    WasteCodeInfoCollection.SingleOrDefault(
                        p => p.WasteCode.CodeType == CodeType.Basel || p.WasteCode.CodeType == CodeType.Oecd);
            }
        }

        public WasteCodeInfo ExportCode
        {
            get { return GetWasteCodes(CodeType.ExportCode).SingleOrDefault(); }
        }

        public WasteCodeInfo ImportCode
        {
            get { return GetWasteCodes(CodeType.ImportCode).SingleOrDefault(); }
        }

        public WasteCodeInfo OtherCode
        {
            get { return GetWasteCodes(CodeType.OtherCode).SingleOrDefault(); }
        }

        public IEnumerable<WasteCodeInfo> EwcCodes
        {
            get { return GetWasteCodes(CodeType.Ewc); }
        }

        public IEnumerable<WasteCodeInfo> YCodes
        {
            get { return GetWasteCodes(CodeType.Y); }
        }

        public IEnumerable<WasteCodeInfo> HCodes
        {
            get { return GetWasteCodes(CodeType.H); }
        }

        public IEnumerable<WasteCodeInfo> UnClasses
        {
            get { return GetWasteCodes(CodeType.Un); }
        }

        public IEnumerable<WasteCodeInfo> UnNumbers
        {
            get { return GetWasteCodes(CodeType.UnNumber); }
        }

        public IEnumerable<WasteCodeInfo> CustomsCodes
        {
            get { return GetWasteCodes(CodeType.CustomsCode); }
        }
    }
}
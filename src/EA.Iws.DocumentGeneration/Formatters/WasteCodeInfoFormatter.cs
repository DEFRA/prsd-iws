﻿namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using Domain.NotificationApplication;

    internal class WasteCodeInfoFormatter
    {
        public const string NotApplicable = "Not applicable";

        public string CodeListToString(IEnumerable<WasteCodeInfo> codes)
        {
            if (codes == null)
            {
                return string.Empty;
            }

            var codesArray = codes.ToArray();

            if (codesArray.Length == 0)
            {
                return string.Empty;
            }

            if (codesArray.Any(c => c.IsNotApplicable))
            {
                return NotApplicable;
            }

            return string.Join(", ", codesArray.Select(c => CodeAsString(c.WasteCode)).OrderBy(c => c));
        }

        public string GetCustomCodeValue(WasteCodeInfo code)
        {
            if (code == null)
            {
                return string.Empty;
            }

            if (code.IsNotApplicable)
            {
                return NotApplicable;
            }

            return (string.IsNullOrWhiteSpace(code.CustomCode))
                ? string.Empty : code.CustomCode;
        }

        private string CodeAsString(WasteCode code)
        {
            if (code.CodeType != CodeType.H || !code.Code.StartsWith("HP"))
            {
                return code.Code;
            }

            return code.Code + " EU";
        }
    }
}

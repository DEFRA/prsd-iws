namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class WasteCodesViewModel
    {
        public string Basel { get; private set; }
        public string Oecd { get; private set; }
        public string Ewc { get; private set; }
        public string Nce { get; private set; }
        public string Nci { get; private set; }
        public string Other { get; private set; }
        public string Y { get; private set; }
        public string H { get; private set; }
        public string UnClass { get; private set; }
        public string UnNumber { get; private set; }
        public string UnShippingName { get; private set; }
        public string Customs { get; private set; }

        public bool IsAnnexNeeded { get; private set; }
        public List<AnnexTableWasteCodes> AnnexTableWasteCodes { get; private set; }
        public string UnNumberAndShippingName { get; private set; }

        public WasteCodesViewModel(NotificationApplication notification)
        {
            SetBaselAndOecdCode(notification);
            Ewc = notification.EwcCodes != null ? MakeStringOfCodes(notification.EwcCodes.OrderBy(c => c.WasteCode.Code)) : string.Empty;
            Nce = notification.ExportCode != null ? notification.ExportCode.CustomCode : string.Empty;
            Nci = notification.ImportCode != null ? notification.ImportCode.CustomCode : string.Empty;
            Other = notification.OtherCode != null ? notification.OtherCode.CustomCode : string.Empty;
            Y = notification.YCodes != null ? MakeStringOfCodes(notification.YCodes.OrderBy(c => c.WasteCode.Code)) : string.Empty;
            H = notification.HCodes != null ? MakeStringOfCodes(notification.HCodes.OrderBy(c => c.WasteCode.Code)) : string.Empty;
            UnClass = notification.UnClasses != null ? MakeStringOfCodes(notification.UnClasses.OrderBy(c => c.WasteCode.Code)) : string.Empty;
            SetUnNumbersAndShippingNames(notification);
            Customs = notification.CustomsCodes != null ? MakeStringOfCustomCodes(notification.CustomsCodes) : string.Empty;
            SetIsAnnexNeeded();
        }

        public WasteCodesViewModel SetAnnexMessagesAndData(int annexNumber)
        {
            var a = new List<AnnexTableWasteCodes>();
            var annexMessage = "See Annex " + annexNumber;

            if (Basel.Length > BaselLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.Basel), Basel));
                Basel = annexMessage;
            }

            if (Oecd.Length > OecdLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.Oecd), Oecd));
                Oecd = annexMessage;
            }

            if (Ewc.Length > EwcLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.Ewc), Ewc));
                Ewc = annexMessage;
            }

            if (Nce.Length > NceLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.ExportCode), Nce));
                Nce = annexMessage;
            }

            if (Nci.Length > NciLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.ImportCode), Nci));
                Nci = annexMessage;
            }

            if (Other.Length > OtherLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.OtherCode), Other));
                Other = annexMessage;
            }

            if (Y.Length > YLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.Y), Y));
                Y = annexMessage;
            }

            if (H.Length > HLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.H), H));
                H = annexMessage;
            }

            if (UnClass.Length > UnClassLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.Un), UnClass));
                UnClass = annexMessage;
            }

            if (UnNumber.Length > UnNumberLength || UnShippingName.Length > UnShippingNameLength)
            {
                a.Add(AddToAnnexTableWasteCodes("UN Number and Shipping name", UnNumberAndShippingName));
                UnNumber = annexMessage;
                UnShippingName = annexMessage;
            }

            if (Customs.Length > CustomsLength)
            {
                a.Add(AddToAnnexTableWasteCodes(EnumHelper.GetDisplayName(CodeType.CustomsCode), Customs));
                Customs = annexMessage;
            }

            AnnexTableWasteCodes = a;

            return this;
        }

        private AnnexTableWasteCodes AddToAnnexTableWasteCodes(string name, string codes)
        {
            return new AnnexTableWasteCodes
            {
                Name = name,
                Codes = codes
            };
        }

        private void SetBaselAndOecdCode(NotificationApplication notification)
        {
            if (notification.BaselOecdCode.WasteCode.CodeType == CodeType.Basel)
            {
                Basel = notification.BaselOecdCode.WasteCode.Code;
                Oecd = string.Empty;
            }

            if (notification.BaselOecdCode.WasteCode.CodeType == CodeType.Oecd)
            {
                Oecd = notification.BaselOecdCode.WasteCode.Code;
                Basel = string.Empty;
            } 
        }

        private void SetUnNumbersAndShippingNames(NotificationApplication notification)
        {
            var codesString = string.Empty;
            var descriptionsString = string.Empty;
            var combinedString = string.Empty;

            if (notification.UnNumbers != null)
            {
                var codes = notification.UnNumbers.OrderBy(c => c.WasteCode.Code);

                foreach (var c in codes)
                {
                    codesString = codesString + c.WasteCode.Code + ", ";
                    descriptionsString = descriptionsString + c.WasteCode.Description + ", ";
                    combinedString = combinedString + c.WasteCode.Code + " - " + c.WasteCode.Description + Environment.NewLine;
                }
            }

            UnNumber = codesString != string.Empty ? codesString.Substring(0, (codesString.Length - 2)) : string.Empty;
            UnShippingName = descriptionsString != string.Empty ? descriptionsString.Substring(0, (descriptionsString.Length - 2)) : string.Empty;
            UnNumberAndShippingName = combinedString;
        }

        private string MakeStringOfCodes(IEnumerable<WasteCodeInfo> codes)
        {
            var codesString = string.Empty;

            foreach (var c in codes)
            {
                codesString = codesString + c.WasteCode.Code + ", ";
            }

            return codesString.Substring(0, (codesString.Length - 2));
        }

        private string MakeStringOfCustomCodes(IEnumerable<WasteCodeInfo> codes)
        {
            var codesString = string.Empty;

            foreach (var c in codes)
            {
                codesString = codesString + c.CustomCode + ", ";
            }

            return codesString.Substring(0, (codesString.Length - 2));
        }

        private void SetIsAnnexNeeded()
        {
            bool result = (Basel.Length > BaselLength ||
                           Oecd.Length > OecdLength ||
                           Ewc.Length > EwcLength ||
                           Nce.Length > NceLength ||
                           Nci.Length > NciLength ||
                           Other.Length > OtherLength ||
                           Y.Length > YLength ||
                           H.Length > HLength ||
                           UnClass.Length > UnClassLength ||
                           UnNumber.Length > UnNumberLength ||
                           UnShippingName.Length > UnShippingNameLength ||
                           Customs.Length > CustomsLength);

            IsAnnexNeeded = result;
        }

        private const int BaselLength = 25;
        private const int OecdLength = 30;
        private const int EwcLength = 40;
        private const int NceLength = 30;
        private const int NciLength = 30;
        private const int OtherLength = 40;
        private const int YLength = 50;
        private const int HLength = 50;
        private const int UnClassLength = 50;
        private const int UnNumberLength = 50;
        private const int UnShippingNameLength = 40;
        private const int CustomsLength = 40;
    }
}

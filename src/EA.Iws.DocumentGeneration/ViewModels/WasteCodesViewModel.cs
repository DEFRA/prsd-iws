namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Formatters;
    using Prsd.Core.Helpers;

    internal class WasteCodesViewModel
    {
        private readonly WasteCodeInfoFormatter wasteCodeInfoFormatter;
        private const string NotApplicable = "Not applicable";

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

        public WasteCodesViewModel(NotificationApplication notification, WasteCodeInfoFormatter wasteCodeInfoFormatter)
        {
            this.wasteCodeInfoFormatter = wasteCodeInfoFormatter;
            InitializeAllCodesToEmptyString();

            SetBaselAndOecdCode(notification);

            Ewc = wasteCodeInfoFormatter.CodeListToString(notification.EwcCodes);
            Y = wasteCodeInfoFormatter.CodeListToString(notification.YCodes);
            H = wasteCodeInfoFormatter.CodeListToString(notification.HCodes);
            UnClass = wasteCodeInfoFormatter.CodeListToString(notification.UnClasses);

            Nce = wasteCodeInfoFormatter.GetCustomCodeValue(notification.ExportCode);
            Nci = wasteCodeInfoFormatter.GetCustomCodeValue(notification.ImportCode);
            Other = wasteCodeInfoFormatter.GetCustomCodeValue(notification.OtherCode);
            Customs = wasteCodeInfoFormatter.GetCustomCodeValue(notification.CustomsCode);

            SetUnNumbersAndShippingNames(notification);
            SetIsAnnexNeeded();
        }

        private void InitializeAllCodesToEmptyString()
        {
            Basel = string.Empty;
            Oecd = string.Empty;
            Ewc = string.Empty;
            Nce = string.Empty;
            Nci = string.Empty;
            Other = string.Empty;
            Y = string.Empty;
            H = string.Empty;
            UnClass = string.Empty;
            UnNumber = string.Empty;
            UnShippingName = string.Empty;
            Customs = string.Empty;
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

        protected AnnexTableWasteCodes AddToAnnexTableWasteCodes(string name, string codes)
        {
            return new AnnexTableWasteCodes
            {
                Name = name,
                Codes = codes
            };
        }

        protected void SetBaselAndOecdCode(NotificationApplication notification)
        {
            Basel = string.Empty;
            Oecd = string.Empty;

            if (notification.BaselOecdCode == null)
            {
                return;
            }

            var value = (notification.BaselOecdCode.IsNotApplicable) ? NotApplicable : notification.BaselOecdCode.WasteCode.Code;

            if (notification.BaselOecdCode.CodeType == CodeType.Basel)
            {
                Basel = value;
            }
            else if (notification.BaselOecdCode.CodeType == CodeType.Oecd)
            {
                Oecd = value;
            }
        }

        protected void SetUnNumbersAndShippingNames(NotificationApplication notification)
        {
            UnNumber = string.Empty;
            UnNumberAndShippingName = string.Empty;
            UnShippingName = string.Empty;
            var separator = ", ";

            if (notification.UnNumbers == null)
            {
                return;
            }

            if (notification.UnNumbers.Any(c => c.IsNotApplicable))
            {
                UnNumber = NotApplicable;
                UnNumberAndShippingName = UnNumber;
                return;
            }

            var codes = notification.UnNumbers.OrderBy(c => c.WasteCode.Code);

            UnNumber = string.Join(separator, codes.Select(c => c.WasteCode.Code));
            UnShippingName = string.Join(separator, codes.Select(c => c.WasteCode.Description));
            UnNumberAndShippingName = string.Join(Environment.NewLine,
                codes.Select(c => c.WasteCode.Code + " - " + c.WasteCode.Description));
        }

        protected void SetIsAnnexNeeded()
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

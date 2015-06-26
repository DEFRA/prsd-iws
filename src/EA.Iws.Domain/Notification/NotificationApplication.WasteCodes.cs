namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Linq;

    public partial class NotificationApplication
    {
        public void AddWasteCode(WasteCodeInfo wasteCodeInfo)
        {
            if (WasteCodeInfoCollection == null)
            {
                throw new InvalidOperationException(
                    string.Format("WasteCodeInfoCollection cannot be null for notification {0}", Id));
            }

            if (wasteCodeInfo.WasteCode.CodeType == CodeType.Basel &&
                WasteCodeInfoCollection.Any(c => c.WasteCode.CodeType == CodeType.Basel))
            {
                throw new InvalidOperationException(string.Format("A Basel code already exists for notification {0}", Id));
            }

            if (wasteCodeInfo.WasteCode.CodeType == CodeType.Oecd &&
                WasteCodeInfoCollection.Any(c => c.WasteCode.CodeType == CodeType.Oecd))
            {
                throw new InvalidOperationException(string.Format("A Oecd code already exists for notification {0}", Id));
            }

            if (
                WasteCodeInfoCollection.Any(
                    c =>
                        c.WasteCode.Code == wasteCodeInfo.WasteCode.Code &&
                        !wasteCodeInfo.IsOptional(c.WasteCode.CodeType)))
            {
                throw new InvalidOperationException(
                    string.Format("The same code cannot be entered twice for notification {0}", Id));
            }

            WasteCodeInfoCollection.Add(wasteCodeInfo);
        }
    }
}
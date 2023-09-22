namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;

    public partial class NotificationApplication
    {
        public void SetWasteComponentInfo(IEnumerable<WasteComponentInfo> wasteComponentInfos)
        {
            WasteComponentInfosCollection.Clear();

            foreach (var wasteComponent in wasteComponentInfos)
            {
                WasteComponentInfosCollection.Add(wasteComponent);
            }
        }
    }
}

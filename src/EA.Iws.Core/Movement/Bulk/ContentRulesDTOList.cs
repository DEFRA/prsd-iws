namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;

    public class ContentRulesDTOList
    {
        public ContentRulesDTOList()
        {
            ContentRulesDTO obj1 = new ContentRulesDTO()
            {
                NotificationNumber = string.Empty,
                ShipmentNumber = "1",
                Quantity = string.Empty,
                Unit = string.Empty,
                PackagingType = string.Empty,
                ActualDateOfShipment = string.Empty
            };
            ObjectsList.Add(obj1);
            ContentRulesDTO obj2 = new ContentRulesDTO()
            {
                NotificationNumber = string.Empty,
                ShipmentNumber = "1",
                Quantity = string.Empty,
                Unit = string.Empty,
                PackagingType = string.Empty,
                ActualDateOfShipment = string.Empty
            };
            ObjectsList.Add(obj1);
        }

        public List<ContentRulesDTO> ObjectsList = new List<ContentRulesDTO>();
    }
}

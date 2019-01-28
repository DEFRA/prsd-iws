namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;
    using Rules;

    public class ContentRuleResult<BulkMovementContentRules>
    {
        public ContentRuleResult(BulkMovementContentRules rule, MessageLevel messageLevel, List<string> erroneousShipmentNumbers, int remainingShipments = 0, int numberOfShipments = 0)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErroneousShipmentNumbers = erroneousShipmentNumbers;
        }

        public BulkMovementContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public List<string> ErroneousShipmentNumbers { get; set; }

        public int RemainingShipments { get; private set; }
        public int NumberOfShipments { get; private set; }

        private string GetShipmentNumbers
        {
            get
            {
                if (ErroneousShipmentNumbers != null && ErroneousShipmentNumbers.Count > 0)
                {
                    string shipmentNosString = string.Empty;
                    foreach (string shipmentNo in ErroneousShipmentNumbers)
                    {
                        shipmentNosString += string.Concat(shipmentNo, ", ");
                    }
                    // Remove the final instance of ", "
                    shipmentNosString = shipmentNosString.Remove(shipmentNosString.Length - 2);

                    return shipmentNosString;
                }
                return "None";
            }
        }

        public string GetErrorMessage
        {
            get
            {
                if (Rule.ToString() == Bulk.BulkMovementContentRules.ExcessiveShipments.ToString())
                {
                    return string.Format(EA.Prsd.Core.Helpers.EnumHelper.GetDisplayName(Rule), NumberOfShipments, RemainingShipments);
                }
                return string.Format(EA.Prsd.Core.Helpers.EnumHelper.GetDisplayName(Rule), GetShipmentNumbers);
            }
        }
    }
}

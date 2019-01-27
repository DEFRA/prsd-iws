namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;
    using Rules;

    public class ContentRuleResult<BulkMovementContentRules>
    {
        public ContentRuleResult()
        {
        }

        public ContentRuleResult(BulkMovementContentRules rule, MessageLevel messageLevel, List<string> erroneousShipmentNumbers)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErroneousShipmentNumbers = erroneousShipmentNumbers;
        }

        public ContentRuleResult(BulkMovementContentRules rule, MessageLevel messageLevel, string errorMessage)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            this.errorMessage = errorMessage;
        }

        public BulkMovementContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public List<string> ErroneousShipmentNumbers { get; set; }

        private string errorMessage;

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
                if (errorMessage != null && !errorMessage.Equals(string.Empty))
                {
                    return errorMessage;
                }

                var errorMessageArg = string.Empty;
                if (ErroneousShipmentNumbers != null && ErroneousShipmentNumbers.Count > 0)
                {
                    errorMessageArg = GetShipmentNumbers;
                }
                else
                {
                    errorMessageArg = ErroneousShipmentCount.ToString();
                }

                return string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(Rule), errorMessageArg);
            }
        }

        private int erroneousShipmentCount = 0;

        public int ErroneousShipmentCount
        {
            get
            {
                if (ErroneousShipmentNumbers != null && ErroneousShipmentNumbers.Count > 0)
                {
                    return ErroneousShipmentNumbers.Count;
                }
                else
                {
                    return erroneousShipmentCount;
                }
            }
            set
            {
                erroneousShipmentCount = value;
            }
        }
    }
}

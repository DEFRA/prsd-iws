namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;
    using Rules;

    public class ContentRuleResult<BulkMovementContentRules>
    {
        public ContentRuleResult(BulkMovementContentRules rule, MessageLevel messageLevel, List<string> erroneousShipmentNumbers, string errorMessage)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErroneousShipmentNumbers = erroneousShipmentNumbers;
            ErrorMessage = errorMessage;
        }

        public BulkMovementContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public List<string> ErroneousShipmentNumbers { get; set; }

        public string ErrorMessage { get; private set; }
    }
}

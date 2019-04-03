namespace EA.Iws.Core.Movement.BulkPrenotification
{
    using Rules;

    public class PrenotificationContentRuleResult<PrenotificationContentRules>
    {
        public PrenotificationContentRuleResult(PrenotificationContentRules rule, 
            MessageLevel messageLevel, 
            string errorMessage,
            int minShipmentNumber)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErrorMessage = errorMessage;
            MinShipmentNumber = minShipmentNumber;
        }

        public PrenotificationContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public string ErrorMessage { get; private set; }

        public int MinShipmentNumber { get; private set; }
    }
}

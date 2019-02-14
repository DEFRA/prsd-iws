namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using Rules;

    public class ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>
    {
        public ReceiptRecoveryContentRuleResult(ReceiptRecoveryContentRules rule, 
            MessageLevel messageLevel, 
            string errorMessage,
            int minShipmentNumber)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErrorMessage = errorMessage;
            MinShipmentNumber = minShipmentNumber;
        }

        public ReceiptRecoveryContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public string ErrorMessage { get; private set; }

        public int MinShipmentNumber { get; private set; }
    }
}

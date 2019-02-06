namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using Rules;

    public class ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>
    {
        public ReceiptRecoveryContentRuleResult(ReceiptRecoveryContentRules rule, MessageLevel messageLevel, string errorMessage)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErrorMessage = errorMessage;
        }

        public ReceiptRecoveryContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}

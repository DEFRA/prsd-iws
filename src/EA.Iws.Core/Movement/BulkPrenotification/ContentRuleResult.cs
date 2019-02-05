namespace EA.Iws.Core.Movement.BulkPrenotification
{
    using Rules;

    public class ContentRuleResult<BulkMovementContentRules>
    {
        public ContentRuleResult(BulkMovementContentRules rule, MessageLevel messageLevel, string errorMessage)
        {
            Rule = rule;
            MessageLevel = messageLevel;
            ErrorMessage = errorMessage;
        }

        public BulkMovementContentRules Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}

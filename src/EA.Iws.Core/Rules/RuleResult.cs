namespace EA.Iws.Core.Rules
{
    public class RuleResult<TRule>
    {
        public RuleResult(TRule rule, MessageLevel messageLevel)
        {
            Rule = rule;
            MessageLevel = messageLevel;
        }

        public TRule Rule { get; private set; }

        public MessageLevel MessageLevel { get; private set; }
    }
}
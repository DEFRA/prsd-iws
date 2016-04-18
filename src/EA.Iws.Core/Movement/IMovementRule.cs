namespace EA.Iws.Core.Movement
{
    using System;
    using System.Threading.Tasks;
    using Rules;

    public interface IMovementRule
    {
        Task<RuleResult<MovementRules>> GetResult(Guid notificationId);
    }
}
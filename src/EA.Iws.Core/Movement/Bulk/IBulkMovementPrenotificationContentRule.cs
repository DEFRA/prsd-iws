namespace EA.Iws.Core.Movement.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBulkMovementPrenotificationContentRule
    {
        Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId);
    }
}

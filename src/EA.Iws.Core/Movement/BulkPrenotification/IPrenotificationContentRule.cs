namespace EA.Iws.Core.Movement.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPrenotificationContentRule
    {
        Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId);
    }
}

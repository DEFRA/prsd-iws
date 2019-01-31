namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;
        private readonly IDraftMovementRepository repository;

        public PerformBulkUploadContentValidationHandler(IEnumerable<IBulkMovementPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper,
            IDraftMovementRepository repository)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            var movements = mapper.Map(message.DataTable);

            var addFirstRowWarningRule = message.IsCsv && !CheckFirstRow(movements);

            result.ContentRulesResults = await GetContentRules(movements, message.NotificationId, addFirstRowWarningRule);

            if (result.IsContentRulesSuccess)
            {
                result.ShipmentNumbers =
                    movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value);

                result.DraftBulkUploadId = await repository.Add(message.NotificationId, movements, message.FileName);
            }

            return result;
        }

        private async Task<List<ContentRuleResult<BulkMovementContentRules>>> GetContentRules(List<PrenotificationMovement> movements, Guid notificationId, bool addFirstRowWarningRule)
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            foreach (var rule in contentRules)
            {
                rules.Add(await rule.GetResult(movements, notificationId));
            }

            if (addFirstRowWarningRule)
            {
                rules.Add(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.HeaderDataRemoved, Core.Rules.MessageLevel.Warning, Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.HeaderDataRemoved)));
            }

            return rules;
        }

        private static bool CheckFirstRow(IList<PrenotificationMovement> movements)
        {
            if (!IsValidNotificationNumber(movements[0].NotificationNumber))
            {
                movements.RemoveAt(0);
                return false;
            }
            
            return true;
        }

        private static bool IsValidNotificationNumber(string input)
        {
            var match = Regex.Match(input.Replace(" ", string.Empty), @"(GB)(\d{4})(\d{6})");

            if (match.Success)
            {
                return true;
            }

            return false;
        }
    }
}

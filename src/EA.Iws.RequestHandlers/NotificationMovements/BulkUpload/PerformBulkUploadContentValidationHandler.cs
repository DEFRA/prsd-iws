namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class PerformBulkUploadContentValidationHandler : IRequestHandler<PerformBulkUploadContentValidation, BulkMovementRulesSummary>
    {
        private readonly IEnumerable<IBulkMovementPrenotificationContentRule> contentRules;
        private readonly IMap<DataTable, List<PrenotificationMovement>> mapper;

        public PerformBulkUploadContentValidationHandler(IEnumerable<IBulkMovementPrenotificationContentRule> contentRules,
            IMap<DataTable, List<PrenotificationMovement>> mapper)
        {
            this.contentRules = contentRules;
            this.mapper = mapper;
        }

        public async Task<BulkMovementRulesSummary> HandleAsync(PerformBulkUploadContentValidation message)
        {
            var result = message.BulkMovementRulesSummary;

            result.PrenotificationMovements = mapper.Map(message.DataTable);

            bool addFirstRowWarningRule = false;

            if (message.IsCsv)
            {
                if (!CheckFirstRow(result.PrenotificationMovements))
                {
                    addFirstRowWarningRule = true;
                }
            }

            result.ContentRulesResults = await GetContentRules(result.PrenotificationMovements, message.NotificationId, addFirstRowWarningRule);

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
                rules.Add(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.HeaderDataRemoved, Core.Rules.MessageLevel.Warning, "We think the first row was header data and have removed it.If the data was not header data, please check the first row and try to upload again"));
            }

            return rules;
        }

        private bool CheckFirstRow(List<PrenotificationMovement> movements)
        {
            if (!IsValidNotificationNumber(movements[0].NotificationNumber))
            {
                movements.RemoveAt(0);
                return false;
            }
            
            return true;
        }

        private bool IsValidNotificationNumber(string input)
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

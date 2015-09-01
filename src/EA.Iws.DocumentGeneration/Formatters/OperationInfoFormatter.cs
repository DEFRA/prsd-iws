namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.NotificationApplication;

    internal class OperationInfoFormatter
    {
        public string OperationInfosToCommaDelimitedList(IEnumerable<OperationInfo> operationInfos)
        {
            if (operationInfos == null ||
                !operationInfos.Any())
            {
                return string.Empty;
            }

            return string.Join(", ", operationInfos
                .OrderBy(c => c.OperationCode.Value)
                .Select(v => v.OperationCode.DisplayName));
        }
    }
}

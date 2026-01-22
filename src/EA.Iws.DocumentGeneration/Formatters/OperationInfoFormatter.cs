namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.OperationCodes;
    using Domain.NotificationApplication;
  using EA.Iws.Core.Extensions;
  using Prsd.Core.Helpers;

    internal class OperationInfoFormatter
    {
        public string OperationInfosToCommaDelimitedList(IEnumerable<OperationInfo> operationInfos)
        {
            if (operationInfos == null ||
                !operationInfos.Any())
            {
                return string.Empty;
            }

            var operationInfosList = operationInfos.OrderByInterimsFirst(x => x.OperationCode).ToList();

            return string.Join(", ", operationInfosList.Select(v => EnumHelper.GetDisplayName(v.OperationCode)));
        }
    }
}

namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.OperationCodes;
    using Domain.NotificationApplication;
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

            var operationInfosList = operationInfos.ToList();
            operationInfosList.Sort(InterimsFirst);

            return string.Join(", ", operationInfosList.Select(v => EnumHelper.GetDisplayName(v.OperationCode)));
        }

        private bool IsInterimCode(OperationCode code)
        {
            if (code == OperationCode.R12 || code == OperationCode.R13 || code == OperationCode.D15 || code == OperationCode.D14 || code == OperationCode.D13 || code == OperationCode.D12)
            {
                return true;
            }
            return false;
        }

        private int InterimsFirst(OperationInfo x, OperationInfo y)
        {
            if (IsInterimCode(x.OperationCode) && IsInterimCode(y.OperationCode))
            {
                return x.OperationCode.CompareTo(y.OperationCode);
            }

            if (!IsInterimCode(x.OperationCode) && !IsInterimCode(y.OperationCode))
            {
                return x.OperationCode.CompareTo(y.OperationCode);
            }

            if (IsInterimCode(x.OperationCode) && !IsInterimCode(y.OperationCode))
            {
                return -1;
            }

            if (!IsInterimCode(x.OperationCode) && IsInterimCode(y.OperationCode))
            {
                return 1;
            }
            
            return 0;
        } 
    }
}

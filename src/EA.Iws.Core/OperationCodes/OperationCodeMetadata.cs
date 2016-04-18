namespace EA.Iws.Core.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Shared;

    public class OperationCodeMetadata
    {
        public static IEnumerable<OperationCode> GetCodesForOperation(NotificationType type)
        {
            return Enum.GetValues(typeof(OperationCode))
                .Cast<OperationCode>()
                .Where(e => GetCodeType(e) == type);
        }

        public static NotificationType GetCodeType(OperationCode operationCode)
        {
            var attribute = typeof(OperationCode).GetField(operationCode.ToString())
                .GetCustomAttribute<OperationCodeTypeAttribute>(false);
            
            if (attribute == null)
            {
                throw new InvalidOperationException("Operation Code " 
                    + operationCode 
                    + " does not provide a Operation Code type attribute.");
            }

            return attribute.OperationType;
        }

        public static bool IsCodeOfOperationType(OperationCode code, NotificationType type)
        {
            return GetCodeType(code) == type;
        }
    }
}

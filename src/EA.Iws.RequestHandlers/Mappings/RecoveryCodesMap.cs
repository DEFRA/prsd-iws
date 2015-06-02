namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using RecoveryCode = Requests.OperationCodes.RecoveryCode;

    internal class RecoveryCodesMap : IMap<IList<RecoveryCode>, IList<OperationCode>>
    {
        public IList<OperationCode> Map(IList<RecoveryCode> codes)
        {
            var recoveryCodes = new List<OperationCode>();

            foreach (var selectedRecoveryCode in codes)
            {
                switch (selectedRecoveryCode)
                {
                    case RecoveryCode.R1: 
                        recoveryCodes.Add(OperationCode.R1);
                        break;
                    case RecoveryCode.R2: 
                        recoveryCodes.Add(OperationCode.R2);
                        break;
                    case RecoveryCode.R3: 
                        recoveryCodes.Add(OperationCode.R3);
                        break;
                    case RecoveryCode.R4: 
                        recoveryCodes.Add(OperationCode.R4);
                        break;
                    case RecoveryCode.R5: 
                        recoveryCodes.Add(OperationCode.R5);
                        break;
                    case RecoveryCode.R6: 
                        recoveryCodes.Add(OperationCode.R6);
                        break;
                    case RecoveryCode.R7: 
                        recoveryCodes.Add(OperationCode.R7);
                        break;
                    case RecoveryCode.R8: 
                        recoveryCodes.Add(OperationCode.R8);
                        break;
                    case RecoveryCode.R9: 
                        recoveryCodes.Add(OperationCode.R9);
                        break;
                    case RecoveryCode.R10: 
                        recoveryCodes.Add(OperationCode.R10);
                        break;
                    case RecoveryCode.R11: 
                        recoveryCodes.Add(OperationCode.R11);
                        break;
                    case RecoveryCode.R12: 
                        recoveryCodes.Add(OperationCode.R12);
                        break;
                    case RecoveryCode.R13: 
                        recoveryCodes.Add(OperationCode.R13);
                        break;
                    default: throw new InvalidOperationException("Unknown Recovery Code type");
                }
            }

            return recoveryCodes;
        }
    }
}

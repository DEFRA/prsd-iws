namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using DisposalCode = Core.OperationCodes.DisposalCode;

    internal class DisposalCodesMap : IMap<IList<DisposalCode>, IList<OperationCode>>
    {
        public IList<OperationCode> Map(IList<DisposalCode> codes)
        {
            var disposalCodes = new List<OperationCode>();

            foreach (var selectedDisposalCode in codes)
            {
                switch (selectedDisposalCode)
                {
                    case DisposalCode.D1: 
                        disposalCodes.Add(OperationCode.D1);
                        break;
                    case DisposalCode.D2: 
                        disposalCodes.Add(OperationCode.D2);
                        break;
                    case DisposalCode.D3: 
                        disposalCodes.Add(OperationCode.D3);
                        break;
                    case DisposalCode.D4: 
                        disposalCodes.Add(OperationCode.D4);
                        break;
                    case DisposalCode.D5: 
                        disposalCodes.Add(OperationCode.D5);
                        break;
                    case DisposalCode.D6: 
                        disposalCodes.Add(OperationCode.D6);
                        break;
                    case DisposalCode.D7: 
                        disposalCodes.Add(OperationCode.D7);
                        break;
                    case DisposalCode.D8: 
                        disposalCodes.Add(OperationCode.D8);
                        break;
                    case DisposalCode.D9: 
                        disposalCodes.Add(OperationCode.D9);
                        break;
                    case DisposalCode.D10: 
                        disposalCodes.Add(OperationCode.D10);
                        break;
                    case DisposalCode.D11: 
                        disposalCodes.Add(OperationCode.D11);
                        break;
                    case DisposalCode.D12: 
                        disposalCodes.Add(OperationCode.D12);
                        break;
                    case DisposalCode.D13: 
                        disposalCodes.Add(OperationCode.D13);
                        break;
                    case DisposalCode.D14: 
                        disposalCodes.Add(OperationCode.D14);
                        break;
                    case DisposalCode.D15: 
                        disposalCodes.Add(OperationCode.D15);
                        break;
                    default: throw new InvalidOperationException("Unknown Disposal Code type");
                }
            }

            return disposalCodes;
        }
    }
}

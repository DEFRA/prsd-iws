namespace EA.Iws.Requests.WasteCodes
{
    using Core.WasteCodes;
    using Core.WasteType;
    using Prsd.Core.Mediator;

    public class GetWasteCodesByType : IRequest<WasteCodeData[]>
    {
        public GetWasteCodesByType(CodeType codeType)
        {
            CodeType = codeType;
        }

        public CodeType CodeType { get; private set; }
    }
}
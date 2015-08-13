namespace EA.Iws.Requests.WasteCodes
{
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class GetWasteCodesByType : IRequest<WasteCodeData[]>
    {
        public GetWasteCodesByType(params CodeType[] codeTypeses)
        {
            CodeTypes = codeTypeses;
        }

        public CodeType[] CodeTypes { get; private set; }
    }
}
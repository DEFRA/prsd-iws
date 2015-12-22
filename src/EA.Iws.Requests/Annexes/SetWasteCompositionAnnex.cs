namespace EA.Iws.Requests.Annexes
{
    using Core.Annexes;
    using Prsd.Core.Mediator;

    public class SetWasteCompositionAnnex : IRequest<bool>
    {
        public AnnexUpload Annex { get; private set; }

        public SetWasteCompositionAnnex(AnnexUpload annex)
        {
            Annex = annex;
        }
    }
}

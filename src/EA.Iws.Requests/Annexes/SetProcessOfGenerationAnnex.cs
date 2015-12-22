namespace EA.Iws.Requests.Annexes
{
    using Core.Annexes;
    using Prsd.Core.Mediator;

    public class SetProcessOfGenerationAnnex : IRequest<bool>
    {
        public AnnexUpload Annex { get; private set; }

        public SetProcessOfGenerationAnnex(AnnexUpload annex)
        {
            Annex = annex;
        }
    }
}

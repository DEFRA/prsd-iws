namespace EA.Iws.Requests.Annexes
{
    using Core.Annexes;
    using Prsd.Core.Mediator;

    public class SetTechnologyEmployedAnnex : IRequest<bool>
    {
        public AnnexUpload Annex { get; private set; }

        public SetTechnologyEmployedAnnex(AnnexUpload annex)
        {
            Annex = annex;
        }
    }
}

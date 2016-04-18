namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    public class AnnexRequirements
    {
        public bool IsTechnologyEmployedRequired { get; private set; }

        public bool IsWasteCompositionRequired { get; private set; }

        public bool IsProcessOfGenerationRequired { get; private set; }

        public AnnexRequirements(bool isTechnologyEmployedRequired, bool isWasteCompositionRequired, bool isProcessOfGenerationRequired)
        {
            IsTechnologyEmployedRequired = isTechnologyEmployedRequired;
            IsWasteCompositionRequired = isWasteCompositionRequired;
            IsProcessOfGenerationRequired = isProcessOfGenerationRequired;
        }
    }
}

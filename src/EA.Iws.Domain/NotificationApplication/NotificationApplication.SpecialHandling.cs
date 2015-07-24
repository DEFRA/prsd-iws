namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;

    public partial class NotificationApplication
    {
        public bool? HasSpecialHandlingRequirements { get; private set; }

        public string SpecialHandlingDetails { get; private set; }

        public void SetSpecialHandlingRequirements(string specialHandlingDetails)
        {
            Guard.ArgumentNotNullOrEmpty(() => specialHandlingDetails, specialHandlingDetails);

            HasSpecialHandlingRequirements = true;
            SpecialHandlingDetails = specialHandlingDetails;
        }

        public void RemoveSpecialHandlingRequirements()
        {
            HasSpecialHandlingRequirements = false;
            SpecialHandlingDetails = null;
        }
    }
}
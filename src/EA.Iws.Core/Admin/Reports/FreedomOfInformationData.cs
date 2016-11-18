namespace EA.Iws.Core.Admin.Reports
{
    using System;

    public class FreedomOfInformationData
    {
        public string NotificationNumber { get; set; }

        public string NotifierName { get; set; }

        public string NotifierAddress { get; set; }

        public string ProducerName { get; set; }

        public string ProducerAddress { get; set; }

        public string PointOfExport { get; set; }

        public string PointOfEntry { get; set; }

        public string ImportCountryName { get; set; }

        public string NameOfWaste { get; set; }

        public string Ewc { get; set; }

        public string YCode { get; set; }

        public string OperationCodes { get; set; }

        public string ImporterName { get; set; }

        public string ImporterAddress { get; set; }

        public string FacilityName { get; set; }

        public string FacilityAddress { get; set; }

        public decimal QuantityReceived { get; set; }

        public string QuantityReceivedUnit { get; set; }

        public decimal IntendedQuantity { get; set; }

        public string IntendedQuantityUnit { get; set; }

        public DateTime? ConsentFrom { get; set; }

        public DateTime? ConsentTo { get; set; }

        public string LocalArea { get; set; }
    }
}
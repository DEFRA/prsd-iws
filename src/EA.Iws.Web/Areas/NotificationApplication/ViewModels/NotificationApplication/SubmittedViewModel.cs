namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Notification;

    public class SubmittedViewModel
    {
        public Guid Id { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public List<string> CompetentAuthorityContactInfo
        {
            get
            {
                switch (CompetentAuthority)
                {
                    case CompetentAuthority.England:
                        return new List<string> { "International Waste Shipments (IWS) Team", "Environment Agency", "Richard Fairclough House,", "Knutsford Road,", "Latchford,", "Warrington,", "WA4 1HT", "Tel: 01925 542265" };
                    case CompetentAuthority.Scotland:
                        return new List<string> { "Producer Compliance and Waste Shipment Unit ", "Scottish Environment Protection Agency", "Strathallan House,", "Castle Business Park,", "Stirling,", "FK9 4TZ", "Tel: 01786457700" };
                    case CompetentAuthority.NorthernIreland:
                        return new List<string> { "TFS Section", "Northern Ireland Environment Agency", "1st Floor Klondyke Building,", "Cromac Avenue,", "Gasworks Business Park,", "Malone Lower,", "Belfast,", "BT7 2JA", "Tel: 028 9056 9742" };
                    case CompetentAuthority.Wales:
                        return new List<string> { "Natural Resources Wales", "Rivers House,", "St. Mellons Business Park,", "Cardiff,", "CF3 0EY,", "Tel: 03000 65 3000" };
                    default:
                        return new List<string>();
                }
            }
        }

        public Dictionary<string, string> CompetentAuthorityBacsInfo
        {
            get
            {
                switch (CompetentAuthority)
                {
                    case CompetentAuthority.England:
                        return new Dictionary<string, string> { { "Company name", "Environment Agency" }, { "Remittance Address", "Income Dept 311, PO Box 263, Peterborough, PE2 8YD" }, { "Bank", "Citibank" }, { "Address", "Citigroup Centre, Canada Square, London, E14 5LB" }, { "Sort code", "08-33-00" }, { "Account number", "12800543" }, { "IBAN", "GB23 CITI0833 0012 8005 43" }, { "SWIFTBIC", "CITI GB2LXXX" } };
                    case CompetentAuthority.Scotland:
                        return new Dictionary<string, string> { { "Company name", "Environment Agency" }, { "Remittance Address", "Income Dept 311, PO Box 263, Peterborough, PE2 8YD" }, { "Bank", "Citibank" }, { "Address", "Citigroup Centre, Canada Square, London, E14 5LB" }, { "Sort code", "08-33-00" }, { "Account number", "12800543" }, { "IBAN", "GB23 CITI0833 0012 8005 43" }, { "SWIFTBIC", "CITI GB2LXXX" } };
                    case CompetentAuthority.NorthernIreland:
                        return new Dictionary<string, string> { { "Company name", "Environment Agency" }, { "Remittance Address", "Income Dept 311, PO Box 263, Peterborough, PE2 8YD" }, { "Bank", "Citibank" }, { "Address", "Citigroup Centre, Canada Square, London, E14 5LB" }, { "Sort code", "08-33-00" }, { "Account number", "12800543" }, { "IBAN", "GB23 CITI0833 0012 8005 43" }, { "SWIFTBIC", "CITI GB2LXXX" } };
                    case CompetentAuthority.Wales:
                        return new Dictionary<string, string> { { "Company name", "Environment Agency" }, { "Remittance Address", "Income Dept 311, PO Box 263, Peterborough, PE2 8YD" }, { "Bank", "Citibank" }, { "Address", "Citigroup Centre, Canada Square, London, E14 5LB" }, { "Sort code", "08-33-00" }, { "Account number", "12800543" }, { "IBAN", "GB23 CITI0833 0012 8005 43" }, { "SWIFTBIC", "CITI GB2LXXX" } };
                    default:
                        return new Dictionary<string, string>();
                }
            }
        }
    }
}
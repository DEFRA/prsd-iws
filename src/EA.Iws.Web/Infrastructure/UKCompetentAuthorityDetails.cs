namespace EA.Iws.Web.Infrastructure
{
    using EA.Iws.Core.Notification;
    using System;
    using System.Web.Mvc;

    public class UKCompetentAuthorityDetails
    {
        public MvcHtmlString HelplineNumber { get; private set; }
        public MvcHtmlString ContactEmail { get; private set; }
        public MvcHtmlString IwsDepartmentDescription { get; private set; }

        private UKCompetentAuthorityDetails(string helplineNumber, string contactEmail, string iwsDepartment)
        {
            HelplineNumber = new MvcHtmlString(helplineNumber);
            ContactEmail = new MvcHtmlString(contactEmail);
            IwsDepartmentDescription = new MvcHtmlString(iwsDepartment);
        }

        public static UKCompetentAuthorityDetails EA
        {
            get
            {
                return new UKCompetentAuthorityDetails(
                    "03708 506 506", 
                    "askshipments@environment-agency.gov.uk", 
                    "IWS team");
            }
        }

        public static UKCompetentAuthorityDetails SEPA
        {
            get
            {
                return new UKCompetentAuthorityDetails(
                    "01786 457 700",
                    "transfrontier@sepa.org.uk",
                    "Producer Compliance and Waste Shipment Unit");
            }
        }

        public static UKCompetentAuthorityDetails NIEA
        {
            get
            {
                return new UKCompetentAuthorityDetails(
                    "028 9056 9742",
                    "tfs@doeni.gov.uk",
                    "Hazardous Waste/TFS Section");
            }
        }

        public static UKCompetentAuthorityDetails NRW
        {
            get
            {
                return new UKCompetentAuthorityDetails(
                    "03000 653 073",
                    "waste-shipments@naturalresourceswales.gov.uk",
                    "Waste Shipment Unit");
            }
        }

        public static UKCompetentAuthorityDetails ForCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            switch (competentAuthority)
            {
                case UKCompetentAuthority.England:
                    return EA;
                case UKCompetentAuthority.Scotland:
                    return SEPA;
                case UKCompetentAuthority.NorthernIreland:
                    return NIEA;
                case UKCompetentAuthority.Wales:
                    return NRW;
                default:
                    throw new ArgumentException(
                        string.Format("No Competent Authority details for {0} found", competentAuthority), "competentAuthority");
            }
        }
    }
}
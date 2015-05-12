namespace EA.Iws.Requests.Registration
{
    public class OrganisationRegistrationData
    {
        public int OrganisationId { get; set; }
        public string Name { get; set; }
        public string CompaniesHouseNumber { get; set; }
        public string TownOrCity { get; set; }
        public string CountyOrProvince { get; set; }
        public string Building { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string EntityType { get; set; }
    }
}
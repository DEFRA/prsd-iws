namespace EA.Iws.Domain
{
    using Core.Domain;

    public class CompetentAuthority : Entity
    {
        public string Name { get; private set; }

        public string Abbreviation { get; private set; }

        public bool IsSystemUser { get; private set; }

        public CompetentAuthority(string name, 
            string abbreviation, 
            bool isSystemUser)
        {
            this.Name = name;
            this.Abbreviation = abbreviation;
            this.IsSystemUser = isSystemUser;
        }
    }
}

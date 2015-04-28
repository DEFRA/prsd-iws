namespace EA.Iws.Cqrs.Registration
{
    using Domain;
    using Iws.Core.Cqrs;

    public class CreateOrganisation : ICommand
    {
        public CreateOrganisation(Organisation organisation)
        {
            Organisation = organisation;
        }

        public Organisation Organisation { get; set; }
    }
}
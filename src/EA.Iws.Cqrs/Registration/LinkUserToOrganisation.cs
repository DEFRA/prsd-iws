namespace EA.Iws.Cqrs.Registration
{
    using Core.Cqrs;
    using Domain;

    public class LinkUserToOrganisation : ICommand
    {
        public readonly string UserId;

        public readonly Organisation Organisation;

        public LinkUserToOrganisation(string userId, Organisation organisation)
        {
            this.UserId = userId;
            this.Organisation = organisation;
        }
    }
}

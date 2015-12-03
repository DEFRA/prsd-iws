namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    internal class StateOfImportValidator : AbstractValidator<StateOfImport>
    {
        public StateOfImportValidator()
        {
            RuleFor(x => x.CompetentAuthorityId).NotNull();
            RuleFor(x => x.EntryPointId).NotNull();
        }
    }
}
namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GeneratePostageLabelHandler : IRequestHandler<GeneratePostageLabel, byte[]>
    {
        private readonly IPostageLabelGenerator postageLabelGenerator;

        public GeneratePostageLabelHandler(IPostageLabelGenerator postageLabelGenerator)
        {
            this.postageLabelGenerator = postageLabelGenerator;
        }

        public Task<byte[]> HandleAsync(GeneratePostageLabel message)
        {
            return Task.FromResult(postageLabelGenerator.GeneratePostageLabel(message.CompetentAuthority));
        }
    }
}

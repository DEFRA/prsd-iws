namespace EA.Iws.Requests.Feedback
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanSendFeedbackData)]
    public class FeedbackData : IRequest<bool>
    {
        public string Option { get; set; }

        public string Description { get; set; }
    }
}
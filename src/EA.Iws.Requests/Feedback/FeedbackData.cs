namespace EA.Iws.Requests.Feedback
{
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class FeedbackData : IRequest<bool>
    {
        public string Option { get; set; }

        public string Description { get; set; }
    }
}
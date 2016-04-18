namespace EA.Iws.RequestHandlers.Feedback
{
    using System.Threading.Tasks;
    using EmailMessaging;
    using Prsd.Core.Mediator;
    using Requests.Feedback;

    public class FeedbackDataRequestHandler : IRequestHandler<FeedbackData, bool>
    {
        private readonly IEmailService emailService;
        private readonly FeedbackInformation feedbackInformation;

        public FeedbackDataRequestHandler(IEmailService emailService, FeedbackInformation feedbackInformation)
        {
            this.emailService = emailService;
            this.feedbackInformation = feedbackInformation;
        }

        public async Task<bool> HandleAsync(FeedbackData message)
        {
            bool isEmailSent = await emailService.SendEmail("Feedback", feedbackInformation.FeedbackEmailTo, "Feedback", message);
            return isEmailSent;
        }
    }
}
namespace EA.Iws.RequestHandlers.Feedback
{
    public class FeedbackInformation
    {
        public FeedbackInformation(string feedbackEmailTo)
        {
            FeedbackEmailTo = feedbackEmailTo;
        }

        public string FeedbackEmailTo { get; private set; }
    }
}

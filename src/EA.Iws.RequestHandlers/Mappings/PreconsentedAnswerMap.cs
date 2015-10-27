namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class PreconsentedAnswerMap : IMap<NotificationApplication, string>
    {
        public string Map(NotificationApplication source)
        {
            string preconsentedAnswer = string.Empty;
            
            if (source.IsPreconsentedRecoveryFacility.HasValue)
            {
                preconsentedAnswer = source.IsPreconsentedRecoveryFacility.GetValueOrDefault()
                    ? "Yes"
                    : "No";
            }

            return preconsentedAnswer;
        }
    }
}

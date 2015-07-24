namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class PreconsentedAnswerMap : IMap<NotificationApplication, string>
    {
        public string Map(NotificationApplication source)
        {
            string preconsentedAnswer = string.Empty;
            string preconsentPositive = string.Empty;
            string preconsentNegative = string.Empty;

            if (source.Facilities.Count() > 1)
            {
                preconsentPositive = "The facilities are pre-consented";
                preconsentNegative = "The facilities are not pre-consented";
            }
            else
            {
                preconsentPositive = "The facility is pre-consented";
                preconsentNegative = "The facility is not pre-consented";
            }

            if (source.IsPreconsentedRecoveryFacility.HasValue)
            {
                preconsentedAnswer = source.IsPreconsentedRecoveryFacility.GetValueOrDefault()
                    ? preconsentPositive
                    : preconsentNegative;
            }
            return preconsentedAnswer;
        }
    }
}

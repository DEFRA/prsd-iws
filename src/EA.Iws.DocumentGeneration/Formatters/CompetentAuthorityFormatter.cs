namespace EA.Iws.DocumentGeneration.Formatters
{
    using System;
    using Core.Notification;

    public static class CompetentAuthorityFormatter
    {
        public static string GetCompetentAuthority(UKCompetentAuthority competentAuthority)
        {
            switch (competentAuthority)
            {
                case UKCompetentAuthority.England:
                    return "GB01 - EA";
                case UKCompetentAuthority.Scotland:
                    return "GB02 - SEPA";
                case UKCompetentAuthority.NorthernIreland:
                    return "GB03 - NIEA";
                case UKCompetentAuthority.Wales:
                    return "GB04 - NRW";
                default:
                    throw new ArgumentOutOfRangeException("competentAuthority", competentAuthority,
                        "Unknown competent authority");
            }
        }
    }
}
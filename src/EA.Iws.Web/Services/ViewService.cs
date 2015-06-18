namespace EA.Iws.Web.Services
{
    public static class ViewService
    {
        public static string WordForNumber(int i)
        {
            switch (i)
            {
                case 1:
                    return "First";
                case 2:
                    return "Second";
                case 3:
                    return "Third";
                case 4:
                    return "Fourth";
                case 5:
                    return "Fifth";
                case 6:
                    return "Sixth";
                case 7:
                    return "Seventh";
                case 8:
                    return "Eighth";
                case 9:
                    return "Ninth";
                case 10:
                    return "Tenth";
                case 11:
                    return "Eleventh";
                case 12:
                    return "Twelfth";
                case 13:
                    return "Thirteenth";
                case 14:
                    return "Fourteenth";
                case 15:
                    return "Fifteenth";
                case 16:
                    return "Sixteenth";
                default:
                    return i + "th";
            }
        }
    }
}
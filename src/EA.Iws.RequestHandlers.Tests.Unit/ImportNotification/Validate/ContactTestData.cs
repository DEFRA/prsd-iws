namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;

    internal class ContactTestData
    {
        public static Contact GetValidTestContact()
        {
            return new Contact
            {
                ContactName = "Mike Merry",
                Email = "mike@merry.com",
                Telephone = "01234 567890"
            };
        }
    }
}
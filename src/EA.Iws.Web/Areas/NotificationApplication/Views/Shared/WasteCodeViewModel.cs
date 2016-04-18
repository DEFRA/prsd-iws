namespace EA.Iws.Web.Areas.NotificationApplication.Views.Shared
{
    using System;
    using Core.WasteCodes;

    public class WasteCodeViewModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public CodeType CodeType { get; set; }
    }
}
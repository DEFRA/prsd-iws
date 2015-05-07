namespace EA.Iws.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using Shared;

    public class YesNoChoice
    {
        public RadioButtonStringCollection Choices { get; set; }

        public YesNoChoice()
        {
            List<string> choices = new List<string> {"Yes", "No"};
            this.Choices = new RadioButtonStringCollection(choices);
        }
    }
}
namespace EA.Iws.Web.ViewModels.Shared
{
    using System.Collections.Generic;

    public class StringIntRadioButtons : KeyValueRadioButtons<string, int>
    {
        public StringIntRadioButtons()
        {
            SelectedValue = 0;
        }

        public StringIntRadioButtons(IEnumerable<KeyValuePair<string, int>> keysAndValues)
            : base(keysAndValues)
        {
        }
    }
}
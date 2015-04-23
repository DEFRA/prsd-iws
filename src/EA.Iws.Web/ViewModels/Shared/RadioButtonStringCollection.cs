namespace EA.Iws.Web.ViewModels.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public sealed class RadioButtonStringCollection : RadioButtonStringCollectionBase
    {
        [Required(ErrorMessage = "This answer is required.")]
        public override string SelectedValue { get; set; }

        public override IList<string> PossibleValues { get; set; }

        public RadioButtonStringCollection()
        {    
        }

        public RadioButtonStringCollection(IEnumerable<string> stringsToUse)
        {
            this.PossibleValues = stringsToUse.ToList();
        }
    }
}
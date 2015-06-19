namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.MeansOfTransport
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Web.ViewModels.Shared;

    public class MeansOfTransportViewModel : IValidatableObject
    {
        public IList<RadioButtonPair<string, int>> PossibleMeans { get; set; }

        public IList<int> SelectedMeans { get; set; }

        [Required(ErrorMessage = "Please answer this question")]
        public int? SelectedValue { get; set; }

        public MeansOfTransportViewModel()
        {
            SelectedMeans = new List<int>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedMeans.Count > 0 && SelectedValue.HasValue && SelectedMeans.Last() == SelectedValue)
            {
                yield return new ValidationResult("Cannot add a means of transport which is the same as the previous means of transport.", new[] { "SelectedValue" });
            }

            for (int i = 0; i < SelectedMeans.Count; i++)
            {
                if (i > 0 && SelectedMeans[i - 1] == SelectedMeans[i])
                {
                    yield return new ValidationResult("Cannot change a means of transport to be the same as a previous means of transport.", new[] { "SelectedMeans[" + i + "]" });
                }
            }
        }
    }
}
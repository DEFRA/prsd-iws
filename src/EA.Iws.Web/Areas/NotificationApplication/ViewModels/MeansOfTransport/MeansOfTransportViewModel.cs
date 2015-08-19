namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.MeansOfTransport
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.MeansOfTransport;
    using Prsd.Core.Domain;

    public class MeansOfTransportViewModel : IValidatableObject
    {
        public MeansOfTransportViewModel()
        {
            PossibleMeans = Enumeration.GetAll<MeansOfTransport>();
        }

        public IEnumerable<MeansOfTransport> PossibleMeans { get; set; }

        [Required(ErrorMessage = "Please answer this question")]
        [RegularExpression(@"^([RTSWA]\-)*?[RTSWA]$",
            ErrorMessage = "Means of transport is not in a valid format. Please enter a value such as R-S-R")]
        [Display(Name = "Means of transport")]
        public string SelectedMeans { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedMeans != null)
            {
                var means = SelectedMeans.Split('-');
                for (var i = 1; i < means.Length; i++)
                {
                    if (means[i] == means[i - 1])
                    {
                        yield return
                            new ValidationResult(
                                "Cannot have a means of transport which is the same as the previous means of transport",
                                new[] { "SelectedMeans" });
                        break;
                    }
                }
            }
        }
    }
}
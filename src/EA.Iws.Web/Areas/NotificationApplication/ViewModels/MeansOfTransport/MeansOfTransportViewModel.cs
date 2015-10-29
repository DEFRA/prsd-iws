namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.MeansOfTransport
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.MeansOfTransport;
    using Prsd.Core.Domain;
    using Views.MeansOfTransport;

    public class MeansOfTransportViewModel : IValidatableObject
    {
        public MeansOfTransportViewModel()
        {
            PossibleMeans = Enumeration.GetAll<MeansOfTransport>();
        }

        public IEnumerable<MeansOfTransport> PossibleMeans { get; set; }

        [Required(ErrorMessageResourceName = "SelectedMeansRequired", ErrorMessageResourceType = typeof(MeansOfTransportResources))]
        [RegularExpression(@"^([RTSWA]\-)*?[RTSWA]$", ErrorMessageResourceName = "SelectedMeansErrorMessage", ErrorMessageResourceType = typeof(MeansOfTransportResources))]
        [Display(Name = "SelectedMeans", ResourceType = typeof(MeansOfTransportResources))]
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
                        yield return new ValidationResult(MeansOfTransportResources.SelectedMeansValidationMessage, new[] { "SelectedMeans" });
                        break;
                    }
                }
            }
        }
    }
}
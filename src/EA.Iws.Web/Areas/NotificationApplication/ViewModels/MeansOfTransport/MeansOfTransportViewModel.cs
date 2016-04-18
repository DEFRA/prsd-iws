namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.MeansOfTransport;
    using Views.MeansOfTransport;

    public class MeansOfTransportViewModel : IValidatableObject
    {
        public MeansOfTransportViewModel()
        {
            PossibleMeans = Enum
                .GetValues(typeof(TransportMethod))
                .Cast<TransportMethod>();
        }

        public IEnumerable<TransportMethod> PossibleMeans { get; set; }

        [Required(ErrorMessageResourceName = "SelectedMeansRequired", ErrorMessageResourceType = typeof(MeansOfTransportResources))]
        [RegularExpression(@"^([RrTtSsWwAa]\-)*?[RrTtSsWwAa]$", ErrorMessageResourceName = "SelectedMeansErrorMessage", ErrorMessageResourceType = typeof(MeansOfTransportResources))]
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
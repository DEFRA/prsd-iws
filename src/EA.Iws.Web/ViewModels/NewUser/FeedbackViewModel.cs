namespace EA.Iws.Web.ViewModels.NewUser
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Requests.Feedback;
    using Shared;
    using Views.Feedback;

    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {
            var collection = new List<string>
            {
                NewUser.FeedbackOptions.VerySatisfied, 
                NewUser.FeedbackOptions.Satisfied, 
                NewUser.FeedbackOptions.NeitherSatisfiedOrDissatisfied,
                NewUser.FeedbackOptions.Dissatisfied,
                NewUser.FeedbackOptions.VeryDissatisfied
            };

            FeedbackOptions = new RadioButtonStringCollectionViewModel
            {
                PossibleValues = collection
            };
        }

        [Required(ErrorMessageResourceName = "FeedbackOptionsRequired", ErrorMessageResourceType = typeof(FeedbackResources))]
        public RadioButtonStringCollectionViewModel FeedbackOptions { get; set; }

        [StringLength(1200)]
        [Display(Name = "Feedback", ResourceType = typeof(FeedbackResources))]
        public string FeedbackDescription { get; set; }

        public FeedbackData ToRequest()
        {
            return new FeedbackData
            {
                Option = FeedbackOptions.SelectedValue,
                Description = FeedbackDescription
            };
        }
    }
}
namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteOperations
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.OperationCodes;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class OperationCodesViewModel : IValidatableObject
    {
        public IList<KeyValuePairViewModel<OperationCode, bool>> PossibleCodes { get; set; }

        public NotificationType NotificationType { get; set; }

        public IList<string> DisplayValues
        {
            get
            {
                return PossibleCodes.Select(c => string.Format("{0} - {1}",
                        EnumHelper.GetDisplayName(c.Key), EnumHelper.GetDescription(c.Key))).ToList();
            }
        }

        public IList<OperationCode> SelectedValues
        {
            get
            {
                if (PossibleCodes == null)
                {
                    return new List<OperationCode>(0);
                }

                return PossibleCodes.Where(c => c.Value).Select(c => c.Key).ToList();
            }
        }

        public OperationCodesViewModel()
        {
        }

        public OperationCodesViewModel(NotificationType type, IEnumerable<OperationCode> selectedCodes)
        {
            NotificationType = type;

            PossibleCodes = OperationCodeMetadata.GetCodesForOperation(type)
                .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, selectedCodes.Contains(c)))
                .ToList();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PossibleCodes != null && !PossibleCodes.Any(c => c.Value))
            {
                var error = string.Format(OperationCodesViewModelResources.SelectionRequired,
                    NotificationType.ToString().ToLowerInvariant());

                yield return new ValidationResult(error, new[] { "PossibleCodes" });
            }
        }
    }
}
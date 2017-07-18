namespace EA.Iws.Web.ViewModels.Shared
{
    using System;

    public class MaskedDateInputViewModel
    {
        public DateTime? Date { get; set; }

        public bool IsCompleted
        {
            get { return Date.HasValue; }
        }

        public MaskedDateInputViewModel()
        {
        }

        public MaskedDateInputViewModel(DateTime date)
        {
            Date = date;
        }
    }
}
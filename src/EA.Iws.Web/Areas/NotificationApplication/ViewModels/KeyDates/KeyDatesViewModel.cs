﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.KeyDates
{
    using System;
    using System.Collections.Generic;
    using Core.NotificationAssessment;

    public class KeyDatesViewModel
    {
        public KeyDatesViewModel(NotificationDatesData dates)
        {
            NotificationReceivedDate = dates.NotificationReceivedDate;
            PaymentReceivedDate = dates.PaymentReceivedDate;
            CommencementDate = dates.CommencementDate;
            CompletedDate = dates.CompletedDate;
            TransmittedDate = dates.TransmittedDate;
            AcknowledgedDate = dates.AcknowledgedDate;
            DecisionRequiredDate = dates.DecisionRequiredDate;
            Decisions = new List<NotificationAssessmentDecision>();
            CurrentStatus = dates.CurrentStatus;
            NameOfOfficer = dates.NameOfOfficer;
        }

        public NotificationStatus CurrentStatus { get; set; }

        public Guid NotificationId { get; set; }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentReceivedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public string NameOfOfficer { get; set; }

        public IList<NotificationAssessmentDecision> Decisions { get; set; }
    }
}
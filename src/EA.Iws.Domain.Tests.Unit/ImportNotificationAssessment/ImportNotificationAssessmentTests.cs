namespace EA.Iws.Domain.Tests.Unit.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class ImportNotificationAssessmentTests
    {
        private readonly ImportNotificationAssessment assessment;

        private static readonly DateTime AnyDate = new DateTime(2016, 1, 1, 0, 0, 0);
        private const string AnyString = "bill and ben the flowerpot men";

        public ImportNotificationAssessmentTests()
        {
            assessment = new ImportNotificationAssessment(Guid.Empty);
        }

        [Fact]
        public void ReceiveNotification_SetsStatus()
        {
            assessment.Receive(AnyDate);

            Assert.Equal(ImportNotificationStatus.NotificationReceived, assessment.Status);
        }

        [Fact]
        public void ReceiveNotification_SetsDate()
        {
            assessment.Receive(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.NotificationReceivedDate);
        }

        [Fact]
        public void CannotReceiveReceivedNotification()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.NotificationReceived);

            Assert.Throws<InvalidOperationException>(() => assessment.Receive(AnyDate));
        }

        [Fact]
        public void CannotReceiveNotificationAwaitingAssessment()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingAssessment);

            Assert.Throws<InvalidOperationException>(() => assessment.Receive(AnyDate));
        }

        [Fact]
        public void SubmitNotification_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.NotificationReceived);

            assessment.Submit();

            Assert.Equal(ImportNotificationStatus.AwaitingPayment, assessment.Status);
        }

        [Fact]
        public void CannotResubmit()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            Assert.Throws<InvalidOperationException>(() => assessment.Submit());
        }

        [Fact]
        public void PaymentFullyReceived_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            assessment.PaymentComplete(AnyDate);

            Assert.Equal(ImportNotificationStatus.AwaitingAssessment, assessment.Status);
        }

        [Fact]
        public void PaymentFullyReceived_SetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            assessment.PaymentComplete(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.PaymentReceivedDate);
        }

        [Fact]
        public void BeginAssessment_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingAssessment);

            assessment.BeginAssessment(AnyDate, AnyString);

            Assert.Equal(ImportNotificationStatus.InAssessment, assessment.Status);
        }

        [Fact]
        public void BeginAssessment_SetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingAssessment);

            assessment.BeginAssessment(AnyDate, AnyString);

            Assert.Equal(AnyDate, assessment.Dates.AssessmentStartedDate);
        }

        [Fact]
        public void BeginAssessment_SetsNameOfOfficer()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingAssessment);

            assessment.BeginAssessment(AnyDate, AnyString);

            Assert.Equal(AnyString, assessment.Dates.NameOfOfficer);
        }

        [Fact]
        public void BeginAssessment_FromAwaitingPayment_Throws()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.AwaitingPayment);

            Assert.Throws<InvalidOperationException>(() => assessment.BeginAssessment(AnyDate, AnyString));
        }

        [Fact]
        public void NotificationComplete_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.InAssessment);

            assessment.CompleteNotification(AnyDate);

            Assert.Equal(ImportNotificationStatus.ReadyToAcknowledge, assessment.Status);
        }
        
        [Fact]
        public void NotificationComplete_SetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.InAssessment);

            assessment.CompleteNotification(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.NotificationCompletedDate);
        }

        [Fact]
        public void CannotCompleteNotificationNotInAssessment()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.NotificationReceived);

            Assert.Throws<InvalidOperationException>(() => assessment.CompleteNotification(AnyDate));
        }

        [Fact]
        public void Acknowledge_SetsStatus()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.ReadyToAcknowledge);

            assessment.Acknowledge(AnyDate);

            Assert.Equal(ImportNotificationStatus.DecisionRequiredBy, assessment.Status);
        }

        [Fact]
        public void Acknowledge_SetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.ReadyToAcknowledge);

            assessment.Acknowledge(AnyDate);

            Assert.Equal(AnyDate, assessment.Dates.AcknowledgedDate);
        }

        [Fact]
        public void CannotAcknowledgeWhenNotReadyForAssessment()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.InAssessment);

            Assert.Throws<InvalidOperationException>(() => assessment.Acknowledge(AnyDate));
        }

        private void SetNotificationAssessmentStatus(ImportNotificationStatus status)
        {
            ObjectInstantiator<ImportNotificationAssessment>.SetProperty(x => x.Status, status, assessment);
        }

        [Theory]
        [InlineData(ImportNotificationStatus.Consented)]
        [InlineData(ImportNotificationStatus.ConsentWithdrawn)]
        [InlineData(ImportNotificationStatus.Objected)]
        [InlineData(ImportNotificationStatus.Withdrawn)]
        public void CanMarkFileAfterDecisionMade(ImportNotificationStatus currentStatus)
        {
            SetNotificationAssessmentStatus(currentStatus);
            assessment.MarkFileClosed(AnyDate);

            Assert.Equal(ImportNotificationStatus.FileClosed, assessment.Status);
        }

        [Theory]
        [InlineData(ImportNotificationStatus.NotificationReceived)]
        [InlineData(ImportNotificationStatus.AwaitingPayment)]
        [InlineData(ImportNotificationStatus.AwaitingAssessment)]
        [InlineData(ImportNotificationStatus.InAssessment)]
        [InlineData(ImportNotificationStatus.ReadyToAcknowledge)]
        [InlineData(ImportNotificationStatus.DecisionRequiredBy)]
        public void CanNotMarkFileClosedBeforeDecisionMade(ImportNotificationStatus currentStatus)
        {
            SetNotificationAssessmentStatus(currentStatus);
            Assert.Throws<InvalidOperationException>(() => assessment.MarkFileClosed(AnyDate));
        }

        [Fact]
        public void MarkFileClosedSetsDate()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.Consented);
            assessment.MarkFileClosed(AnyDate);

            Assert.Equal(assessment.Dates.FileClosedDate, AnyDate);
        }

        [Fact]
        public void SetArchiveReference_FileClosedDateIsSet_UpdatesValue()
        {
            SetNotificationAssessmentStatus(ImportNotificationStatus.Consented);
            assessment.MarkFileClosed(AnyDate);
            assessment.SetArchiveReference("ref");

            Assert.Equal("ref", assessment.Dates.ArchiveReference);
        }

        [Fact]
        public void SetArchiveReference_FileClosedDateIsNotSet_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => assessment.SetArchiveReference("ref"));
        }

        [Theory]
        [InlineData(ImportNotificationStatus.NotificationReceived)]
        [InlineData(ImportNotificationStatus.AwaitingPayment)]
        [InlineData(ImportNotificationStatus.AwaitingAssessment)]
        [InlineData(ImportNotificationStatus.InAssessment)]
        [InlineData(ImportNotificationStatus.ReadyToAcknowledge)]
        [InlineData(ImportNotificationStatus.DecisionRequiredBy)]
        public void CanWithdrawInAllowedStates(ImportNotificationStatus currentStatus)
        {
            SetNotificationAssessmentStatus(currentStatus);
            assessment.Withdraw(AnyDate, "test");

            Assert.Equal(ImportNotificationStatus.Withdrawn, assessment.Status);
        }
    }
}

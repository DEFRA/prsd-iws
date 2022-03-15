﻿namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Linq;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationAssessmentStatusTests
    {
        private readonly Guid notificationId;
        private readonly NotificationAssessment notificationAssessment;
        private readonly INotificationProgressService progressService;
        private DateTime receivedDate = new DateTime(2015, 8, 1);
        private DateTime paymentDate = new DateTime(2015, 8, 2);
        private DateTime commencementDate = new DateTime(2015, 8, 10);
        private string nameOfOfficer = "officer";
        private DateTime completedDate = new DateTime(2015, 8, 20);
        private DateTime transmittedDate = new DateTime(2015, 8, 22);
        private DateTime acknowledgedDate = new DateTime(2015, 8, 23);
        private DateTime decisionByDate = new DateTime(2015, 8, 24);
        private DateTime consentedDate = new DateTime(2015, 9, 1);
        private DateTime withdrawnDate = new DateTime(2015, 9, 10);
        private DateTime fileClosedDate = new DateTime(2015, 12, 1);
        private static readonly string AnyString = "Where is Wilfred hiding?";

        public NotificationAssessmentStatusTests()
        {
            notificationId = new Guid("C4C62654-048C-45A2-BF7F-9837EFCF328F");
            notificationAssessment = new NotificationAssessment(notificationId);
            progressService = A.Fake<INotificationProgressService>();
            A.CallTo(() => progressService.IsComplete(notificationId)).Returns(true);
        }

        private void SetNotificationStatus(NotificationStatus status)
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, status,
                notificationAssessment);
        }

        [Fact]
        public void DefaultStatusIsNotSubmitted()
        {
            Assert.Equal(NotificationStatus.NotSubmitted, notificationAssessment.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(NotificationStatus.Submitted, notificationAssessment.Status);
        }

        [Fact]
        public void CantSubmitTwice()
        {
            notificationAssessment.Submit(progressService);
            Action submitAgain = () => notificationAssessment.Submit(progressService);

            Assert.Throws<InvalidOperationException>(submitAgain);
        }

        [Fact]
        public void SubmitRaisesStatusChangeEvent()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(notificationAssessment,
                notificationAssessment.Events.OfType<NotificationStatusChangeEvent>()
                    .SingleOrDefault()
                    .NotificationAssessment);
        }

        [Fact]
        public void SubmitRaisesNotificationSubmittedEvent()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(notificationId,
                notificationAssessment.Events.OfType<NotificationSubmittedEvent>()
                    .SingleOrDefault()
                    .NotificationApplicationId);
        }

        [Fact]
        public void CantSubmitIncompleteApplication()
        {
            A.CallTo(() => progressService.IsComplete(A<Guid>._)).Returns(false);

            Action submit = () => notificationAssessment.Submit(progressService);

            Assert.Throws<InvalidOperationException>(submit);
        }

        [Fact]
        public void SetNotificationReceivedSetsDate()
        {
            SetNotificationStatus(NotificationStatus.Submitted);

            notificationAssessment.NotificationReceived(receivedDate);

            Assert.Equal(receivedDate, notificationAssessment.Dates.NotificationReceivedDate);
        }

        [Fact]
        public void SetNotificationReceivedChangesStatusToNotificationReceived()
        {
            SetNotificationStatus(NotificationStatus.Submitted);

            notificationAssessment.NotificationReceived(receivedDate);

            Assert.Equal(NotificationStatus.NotificationReceived, notificationAssessment.Status);
        }

        [Fact]
        public void CantSetNotificationReceivedWhenNotSubmitted()
        {
            Action setNotificationReceived = () => notificationAssessment.NotificationReceived(receivedDate);

            Assert.Throws<InvalidOperationException>(setNotificationReceived);
        }

        [Fact]
        public void SetNotificationReceivedRaisesStatusChangeEvent()
        {
            SetNotificationStatus(NotificationStatus.Submitted);

            notificationAssessment.NotificationReceived(receivedDate);

            Assert.Equal(notificationAssessment,
                notificationAssessment.Events.OfType<NotificationStatusChangeEvent>()
                    .SingleOrDefault()
                    .NotificationAssessment);
        }

        [Fact]
        public void CanSetCommencementDateWhenReceived()
        {
            SetNotificationStatus(NotificationStatus.NotificationReceived);

            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, receivedDate, notificationAssessment.Dates);

            notificationAssessment.Commenced(commencementDate, nameOfOfficer);

            Assert.Equal(commencementDate, notificationAssessment.Dates.CommencementDate);
        }

        [Fact]
        public void CanSetCommencementDateWhenReceived_SetsNameOfOfficer()
        {
            SetNotificationStatus(NotificationStatus.NotificationReceived);

            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, receivedDate, notificationAssessment.Dates);

            notificationAssessment.Commenced(commencementDate, nameOfOfficer);

            Assert.Equal(nameOfOfficer, notificationAssessment.Dates.NameOfOfficer);
        }

        [Fact]
        public void CantSetCommencementDateWithoutPaymentDate()
        {
            Action setCommencementDate = () => notificationAssessment.Commenced(commencementDate, nameOfOfficer);

            Assert.Throws<InvalidOperationException>(setCommencementDate);
        }

        [Fact]
        public void SetCommencementDateUpdatesStatusToInAssessment()
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, notificationAssessment);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, receivedDate, notificationAssessment.Dates);

            notificationAssessment.Commenced(commencementDate, nameOfOfficer);

            Assert.Equal(NotificationStatus.InAssessment, notificationAssessment.Status);
        }

        [Fact]
        public void SetCommencementDate_NameOfOfficerCantBeNull()
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, notificationAssessment);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, receivedDate, notificationAssessment.Dates);

            Action setCommencementDate = () => notificationAssessment.Commenced(commencementDate, null);

            Assert.Throws<ArgumentNullException>("nameOfOfficer", setCommencementDate);
        }

        [Fact]
        public void SetCommencementDate_NameOfOfficerCantBeEmpty()
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.NotificationReceived, notificationAssessment);
            ObjectInstantiator<NotificationDates>.SetProperty(x => x.PaymentReceivedDate, receivedDate, notificationAssessment.Dates);

            Action setCommencementDate = () => notificationAssessment.Commenced(commencementDate, string.Empty);

            Assert.Throws<ArgumentException>("nameOfOfficer", setCommencementDate);
        }

        [Fact]
        public void SetNotificationCompletedSetsDate()
        {
            SetNotificationStatus(NotificationStatus.InAssessment);

            notificationAssessment.Complete(completedDate);

            Assert.Equal(completedDate, notificationAssessment.Dates.CompleteDate);
        }

        [Fact]
        public void SetNotificationCompleteChangesStatusToReadyToTransmit()
        {
            SetNotificationStatus(NotificationStatus.InAssessment);

            notificationAssessment.Complete(completedDate);

            Assert.Equal(NotificationStatus.ReadyToTransmit, notificationAssessment.Status);
        }

        [Fact]
        public void CantSetNotificationCompleteWhenNotInAssessment()
        {
            Action setNotificationComplete = () => notificationAssessment.Complete(completedDate);

            Assert.Throws<InvalidOperationException>(setNotificationComplete);
        }

        [Fact]
        public void SetNotificationTransmittedSetsDate()
        {
            SetNotificationStatus(NotificationStatus.ReadyToTransmit);

            notificationAssessment.Transmit(transmittedDate);

            Assert.Equal(transmittedDate, notificationAssessment.Dates.TransmittedDate);
        }

        [Fact]
        public void SetNotificationTransmittedChangesStatusToTransmitted()
        {
            SetNotificationStatus(NotificationStatus.ReadyToTransmit);

            notificationAssessment.Transmit(transmittedDate);

            Assert.Equal(NotificationStatus.Transmitted, notificationAssessment.Status);
        }

        [Fact]
        public void CantSetNotificationTransmittedWhenNotReadyToTransmit()
        {
            Action setNotificationTransmitted = () => notificationAssessment.Transmit(transmittedDate);

            Assert.Throws<InvalidOperationException>(setNotificationTransmitted);
        }

        [Fact]
        public void SetAcknowledgedSetsDate()
        {
            SetNotificationStatus(NotificationStatus.Transmitted);

            notificationAssessment.Acknowledge(acknowledgedDate);

            Assert.Equal(acknowledgedDate, notificationAssessment.Dates.AcknowledgedDate);
        }

        [Fact]
        public void SetAcknowledgedChangesStatusToDecisionRequired()
        {
            SetNotificationStatus(NotificationStatus.Transmitted);

            notificationAssessment.Acknowledge(acknowledgedDate);

            Assert.Equal(NotificationStatus.DecisionRequiredBy, notificationAssessment.Status);
        }

        [Fact]
        public void CantSetNotificationAcknowledgedWhenNotTransmitted()
        {
            Action setAcknowledged = () => notificationAssessment.Acknowledge(acknowledgedDate);

            Assert.Throws<InvalidOperationException>(setAcknowledged);
        }

        [Fact]
        public void WithdrawSetsDate()
        {
            var date = new DateTime(2017, 1, 1);
            var reason = "reason";
            
            SetNotificationStatus(NotificationStatus.Submitted);

            notificationAssessment.Withdraw(date, reason);
            
            Assert.Equal(date, notificationAssessment.Dates.WithdrawnDate);
        }

        [Fact]
        public void ConsentWithdrawn_PossibleFromConsentedNotification()
        {
            SetNotificationStatus(NotificationStatus.Consented);

            notificationAssessment.WithdrawConsent(withdrawnDate, AnyString);

            Assert.Equal(NotificationStatus.ConsentWithdrawn, notificationAssessment.Status);
        }

        [Fact]
        public void ConsentWithdrawn_SetsConsentWithdrawnDate()
        {
            SetNotificationStatus(NotificationStatus.Consented);

            notificationAssessment.WithdrawConsent(withdrawnDate, AnyString);

            Assert.Equal(withdrawnDate, notificationAssessment.Dates.ConsentWithdrawnDate);
        }

        [Fact]
        public void ConsentWithdrawn_SetsConsentWithdrawnReasons()
        {
            SetNotificationStatus(NotificationStatus.Consented);

            notificationAssessment.WithdrawConsent(withdrawnDate, AnyString);

            Assert.Equal(AnyString, notificationAssessment.Dates.ConsentWithdrawnReasons);
        }

        [Fact]
        public void ConsentWithdrawn_CannotWithdrawConsentFromObjectedNotification()
        {
            SetNotificationStatus(NotificationStatus.Objected);

            Action withdrawConsent = () => notificationAssessment.WithdrawConsent(withdrawnDate, AnyString);

            Assert.Throws<InvalidOperationException>(withdrawConsent);
        }

        [Fact]
        public void CanUnlockWhenAcknowledged()
        {
            SetNotificationStatus(NotificationStatus.DecisionRequiredBy);

            notificationAssessment.Unlock();

            Assert.Equal(NotificationStatus.Unlocked, notificationAssessment.Status);
        }

        [Fact]
        public void CanUnlockWhenTransmitted()
        {
            SetNotificationStatus(NotificationStatus.Transmitted);

            notificationAssessment.Unlock();

            Assert.Equal(NotificationStatus.Unlocked, notificationAssessment.Status);
        }

        [Fact]
        public void CanUnlockWhenConsented()
        {
            SetNotificationStatus(NotificationStatus.Consented);

            notificationAssessment.Unlock();

            Assert.Equal(NotificationStatus.ConsentedUnlock, notificationAssessment.Status);
        }

        [Fact]
        public void CanResubmitWhenConsentedUnlocked()
        {
            SetNotificationStatus(NotificationStatus.ConsentedUnlock);

            notificationAssessment.Resubmit();

            Assert.Equal(NotificationStatus.Consented, notificationAssessment.Status);
        }

        [Fact]
        public void CantResubmitWhenNotConsentedUnlocked()
        {
            Action resubmit = () => notificationAssessment.Resubmit();

            Assert.Throws<InvalidOperationException>(resubmit);
        }

        [Fact]
        public void CanNotMarkFileClosedInConcentedUnlock()
        {
            SetNotificationStatus(NotificationStatus.ConsentedUnlock);
            Assert.Throws<InvalidOperationException>(() => notificationAssessment.MarkFileClosed(fileClosedDate));
        }

        [Fact]
        public void CantUnlockWhenNotAcknowledged()
        {
            Action unlock = () => notificationAssessment.Unlock();

            Assert.Throws<InvalidOperationException>(unlock);
        }

        [Fact]
        public void CanResubmitWhenUnlocked()
        {
            SetNotificationStatus(NotificationStatus.Unlocked);

            notificationAssessment.Resubmit();

            Assert.Equal(NotificationStatus.Reassessment, notificationAssessment.Status);
        }

        [Fact]
        public void CantResubmitWhenNotUnlocked()
        {
            Action resubmit = () => notificationAssessment.Resubmit();

            Assert.Throws<InvalidOperationException>(resubmit);
        }

        [Fact]
        public void AcceptChanges_GoesToDecisionRequiredBy_FromReassessment()
        {
            SetNotificationStatus(NotificationStatus.Reassessment);
            notificationAssessment.Dates.AcknowledgedDate = acknowledgedDate;
            notificationAssessment.AcceptChanges();

            Assert.Equal(NotificationStatus.DecisionRequiredBy, notificationAssessment.Status);
        }
 
        [Fact]
        public void AcceptChanges_GoesToTransmitted_FromReassessment()
        {
            SetNotificationStatus(NotificationStatus.Reassessment);
            notificationAssessment.Dates.TransmittedDate = transmittedDate;
            notificationAssessment.Dates.AcknowledgedDate = null;
            notificationAssessment.AcceptChanges();

            Assert.Equal(NotificationStatus.Transmitted, notificationAssessment.Status);
        }

        [Fact]
        public void AcceptChanges_DoesnotGoesToTransmitted_FromReassessment()
        {
            SetNotificationStatus(NotificationStatus.Reassessment);
            notificationAssessment.Dates.TransmittedDate = transmittedDate;
            notificationAssessment.Dates.AcknowledgedDate = acknowledgedDate;
            notificationAssessment.AcceptChanges();

            Assert.NotEqual(NotificationStatus.Transmitted, notificationAssessment.Status);
        }

        [Fact]
        public void RejectChanges_GoesToUnlocked_FromReassessment()
        {
            SetNotificationStatus(NotificationStatus.Reassessment);

            notificationAssessment.RejectChanges();

            Assert.Equal(NotificationStatus.Unlocked, notificationAssessment.Status);
        }

        [Fact]
        public void CantAcceptChanges_NotInReassessment()
        {
            Action acceptChanges = () => notificationAssessment.AcceptChanges();

            Assert.Throws<InvalidOperationException>(acceptChanges);
        }
        
        [Fact]
        public void CantRejectChanges_NotInReassessment()
        {
            Action rejectChanges = () => notificationAssessment.RejectChanges();

            Assert.Throws<InvalidOperationException>(rejectChanges);
        }

        [Theory]
        [InlineData(NotificationStatus.ConsentWithdrawn)]
        [InlineData(NotificationStatus.Consented)]
        [InlineData(NotificationStatus.Objected)]
        [InlineData(NotificationStatus.Withdrawn)]
        public void CanMarkFileAfterDecisionMade(NotificationStatus currentStatus)
        {
            SetNotificationStatus(currentStatus);
            notificationAssessment.MarkFileClosed(fileClosedDate);

            Assert.Equal(NotificationStatus.FileClosed, notificationAssessment.Status);
        }

        [Theory]
        [InlineData(NotificationStatus.DecisionRequiredBy)]
        [InlineData(NotificationStatus.InAssessment)]
        [InlineData(NotificationStatus.NotSubmitted)]
        [InlineData(NotificationStatus.NotificationReceived)]
        [InlineData(NotificationStatus.ReadyToTransmit)]
        [InlineData(NotificationStatus.Reassessment)]
        [InlineData(NotificationStatus.Submitted)]
        [InlineData(NotificationStatus.Transmitted)]
        [InlineData(NotificationStatus.Unlocked)]
        public void CanNotMarkFileClosedBeforeDecisionMade(NotificationStatus currentStatus)
        {
            SetNotificationStatus(currentStatus);
            Assert.Throws<InvalidOperationException>(() => notificationAssessment.MarkFileClosed(fileClosedDate));
        }

        [Fact]
        public void MarkFileClosedSetsDate()
        {
            SetNotificationStatus(NotificationStatus.Consented);
            notificationAssessment.MarkFileClosed(fileClosedDate);

            Assert.Equal(notificationAssessment.Dates.FileClosedDate, fileClosedDate);
        }

        [Fact]
        public void MarkFileClosedSetsStatusAtFileClosed()
        {
            SetNotificationStatus(NotificationStatus.Consented);
            notificationAssessment.MarkFileClosed(fileClosedDate);

            Assert.Equal(NotificationStatus.Consented, notificationAssessment.Dates.StatusAtFileClosed);
        }

        [Fact]
        public void SetArchiveReference_FileClosedDateIsSet_UpdatesValue()
        {
            SetNotificationStatus(NotificationStatus.Consented);
            notificationAssessment.MarkFileClosed(fileClosedDate);
            notificationAssessment.SetArchiveReference("ref");

            Assert.Equal("ref", notificationAssessment.Dates.ArchiveReference);
        }

        [Fact]
        public void SetArchiveReference_FileClosedDateIsNotSet_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => notificationAssessment.SetArchiveReference("ref"));
        }
    }
}
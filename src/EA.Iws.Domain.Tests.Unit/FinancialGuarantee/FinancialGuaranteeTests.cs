namespace EA.Iws.Domain.Tests.Unit.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;

    public class FinancialGuaranteeTests
    {
        protected readonly FinancialGuaranteeCollection FinancialGuaranteeCollection;
        protected readonly Guid NotificationId = new Guid("80BC000C-14C5-47B6-B15A-F277A2B63F59");

        protected static readonly DateTime AnyDate = new DateTime(2014, 5, 5);
        protected static readonly string AnyString = "test";
        protected static readonly DateTime CompletedDate = AnyDate.AddDays(1);
        protected static readonly DateTime AfterCompletionDate = AnyDate.AddDays(2);
        protected static readonly DateTime BeforeCompletionDate = AnyDate;

        protected readonly FinancialGuarantee FinancialGuarantee;
        protected readonly FinancialGuarantee CompletedFinancialGuarantee;
        protected readonly FinancialGuarantee ReceivedFinancialGuarantee;
        protected readonly FinancialGuarantee ApprovedFinancialGuarantee;
        protected readonly FinancialGuarantee RefusedFinancialGuarantee;

        protected readonly IDeferredEventDispatcher Dispatcher;

        public FinancialGuaranteeTests()
        {
            FinancialGuaranteeCollection = new FinancialGuaranteeCollection(NotificationId);
            FinancialGuarantee = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);

            CompletedFinancialGuarantee = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationComplete, CompletedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, CompletedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, CompletedFinancialGuarantee);

            ApprovedFinancialGuarantee = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.Approved, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.DecisionDate, AfterCompletionDate, ApprovedFinancialGuarantee);

            RefusedFinancialGuarantee = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.Refused, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.DecisionDate, AfterCompletionDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.RefusalReason, AnyString, RefusedFinancialGuarantee);

            ReceivedFinancialGuarantee = FinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationReceived, ReceivedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, ReceivedFinancialGuarantee);

            Dispatcher = A.Fake<IDeferredEventDispatcher>();
            DomainEvents.Dispatcher = Dispatcher;
        }
    }
}

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
            FinancialGuarantee = FinancialGuarantee.Create(new Guid("C91DA02A-114A-44C3-8B12-3FF24950D6E4"));

            CompletedFinancialGuarantee = FinancialGuarantee.Create(new Guid("5DC6DB46-89DF-4F3D-BE47-4290EEE890D3"));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationComplete, CompletedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, CompletedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, CompletedFinancialGuarantee);

            ApprovedFinancialGuarantee = FinancialGuarantee.Create(new Guid("38CBD523-7F96-45AA-9251-955DB1632F3A"));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.Approved, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.DecisionDate, AfterCompletionDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ValidFrom, AfterCompletionDate, ApprovedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ValidTo, AfterCompletionDate.AddYears(1), ApprovedFinancialGuarantee);

            RefusedFinancialGuarantee = FinancialGuarantee.Create(new Guid("229F6957-3CBE-4C70-9A5F-40F42CF5BA11"));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.Refused, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.CompletedDate, CompletedDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.DecisionDate, AfterCompletionDate, RefusedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.RefusalReason, AnyString, RefusedFinancialGuarantee);

            ReceivedFinancialGuarantee = FinancialGuarantee.Create(new Guid("26342B36-15A4-4AC4-BAE0-9C2CA36B0CD9"));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationReceived, ReceivedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate, ReceivedFinancialGuarantee);

            Dispatcher = A.Fake<IDeferredEventDispatcher>();
            DomainEvents.Dispatcher = Dispatcher;
        }
    }
}

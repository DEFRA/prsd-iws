﻿namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using NotificationConsent;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementFactoryTests : IDisposable
    {
        private static readonly Guid NotificationId = new Guid("0E38E99F-A997-4014-8438-62B56E0398DF");
        private static readonly DateTime Today = new DateTime(2015, 1, 1);
        private readonly MovementFactory factory;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly INotificationConsentRepository consentRepository;
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IMovementDateValidator dateValidator;
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        public MovementFactoryTests()
        {
            SystemTime.Freeze(Today);

            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            movementRepository = A.Fake<IMovementRepository>();
            assessmentRepository = A.Fake<INotificationAssessmentRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();
            consentRepository = A.Fake<INotificationConsentRepository>();
            workingDayCalculator = A.Fake<IWorkingDayCalculator>();
            notificationApplicationRepository = A.Fake<INotificationApplicationRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            dateValidator = A.Fake<IMovementDateValidator>();

            var movementNumberGenerator = new MovementNumberGenerator(new NextAvailableMovementNumberGenerator(movementRepository), 
                movementRepository, 
                shipmentRepository);
            var numberOfMovements = new NumberOfMovements(movementRepository, shipmentRepository);
            var movementsQuatity = new NotificationMovementsQuantity(movementRepository, shipmentRepository);
            var numberOfActiveLoads = new NumberOfActiveLoads(movementRepository, financialGuaranteeRepository);
            var consentPeriod = new ConsentPeriod(consentRepository, workingDayCalculator, notificationApplicationRepository);

            factory = new MovementFactory(numberOfMovements,
                movementsQuatity,
                assessmentRepository,
                movementNumberGenerator,
                numberOfActiveLoads,
                consentPeriod,
                dateValidator,
                financialGuaranteeRepository,
                A.Fake<IUserContext>());
        }

        [Fact]
        public async Task NewMovementExceedsShipmentLimit_Throws()
        {
            CreateShipmentInfo(1);

            var existingMovement = new TestableMovement
            {
                Id = new Guid("1584B5F6-4E33-441D-A9C9-17C1C3B28BA2"),
                NotificationId = NotificationId,
            };

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new[] { existingMovement });

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, Today));
        }

        [Fact]
        public async Task NotificationNotConsented_Throws()
        {
            CreateShipmentInfo(1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new Movement[0]);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, Today));
        }

        [Fact]
        public async Task FinancialGuaranteeNotApproved_Throws()
        {
            SetupMovements(500, 100);
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.ApplicationComplete));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new Movement[0]);

            var date = Today.AddDays(5);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, date));
        }

        [Fact]
        public async Task DateIsValidated()
        {
            SetupMovements(500, 100);
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(1));

            var date = Today.AddDays(5);

            await factory.Create(NotificationId, date);

            A.CallTo(() => dateValidator.EnsureDateValid(NotificationId, date)).MustHaveHappened();
        }

        [Fact]
        public async Task ReturnsMovement()
        {
            CreateShipmentInfo(1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId))
                .Returns(new Movement[0]);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(1));
            A.CallTo(() => consentRepository.GetByNotificationId(NotificationId)).Returns(ValidConsent());

            var movement = await factory.Create(NotificationId, Today);

            Assert.NotNull(movement);
        }

        [Fact]
        public async Task CurrentActiveLoadByDateEqualsPermitted_Throws()
        {
            CreateShipmentInfo(1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId))
                .Returns(new Movement[0]);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(2));

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, Today));
        }

        [Fact]
        public async Task QuantityReached_Throws()
        {
            SetupMovements(1000, 1000);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, Today));
        }

        [Fact]
        public async Task QuantityExceeded_Throws()
        {
            SetupMovements(1000, 1001);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, Today));
        }

        [Fact]
        public async Task QuantityNotReached_DoesNotThrow()
        {
            SetupMovements(1000, 900);

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(1));

            await factory.Create(NotificationId, Today);
        }

        private void SetupMovements(int intendedQuantity, int quantityReceived)
        {
            var shipment = new TestableShipmentInfo
            {
                Id = new Guid("2DA8E281-A6A4-459A-A38A-B4B0643E0726"),
                NotificationId = NotificationId,
                NumberOfShipments = 10,
                Quantity = intendedQuantity,
                Units = ShipmentQuantityUnits.Tonnes
            };

            var existingMovement = new TestableMovement
            {
                Id = new Guid("1584B5F6-4E33-441D-A9C9-17C1C3B28BA2"),
                NotificationId = NotificationId,
                Status = MovementStatus.Received
            };

            var movementReceipt = new TestableMovementReceipt
            {
                Id = new Guid("28FFDD0B-1A1A-4CAC-B4E4-D232DA7B2AB8"),
                QuantityReceived = new ShipmentQuantity(quantityReceived, ShipmentQuantityUnits.Tonnes)
            };

            existingMovement.Receipt = movementReceipt;

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);
            A.CallTo(() => movementRepository.GetMovementsByStatus(NotificationId, MovementStatus.Received)).Returns(new[] { existingMovement });
            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId)).Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });
            A.CallTo(() => consentRepository.GetByNotificationId(NotificationId)).Returns(ValidConsent());
        }

        private void CreateShipmentInfo(int maxNumberOfShipments)
        {
            var shipment = new TestableShipmentInfo
            {
                Id = new Guid("2DA8E281-A6A4-459A-A38A-B4B0643E0726"),
                NotificationId = NotificationId,
                NumberOfShipments = maxNumberOfShipments,
                Quantity = 1000,
                Units = ShipmentQuantityUnits.Tonnes
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);
        }

        private static FinancialGuaranteeCollection GetFinancialGuarantee(FinancialGuaranteeStatus status)
        {
            var collection = new FinancialGuaranteeCollection(NotificationId);

            var fg = collection.AddFinancialGuarantee(Today);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.ActiveLoadsPermitted, 2, fg);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.Status, status, fg);

            return collection;
        }

        private IEnumerable<Movement> GetMovementArray(int n)
        {
            var movements = new List<Movement>();

            for (int i = 0; i < n; i++)
            {
                movements.Add(new Movement(i + 1, NotificationId, Today, userId));
            }

            return movements;
        }

        private Consent ValidConsent()
        {
            var now = SystemTime.UtcNow;

            var nowPlusOne = new DateTime(now.Year + 1, now.Month, now.Day);

            return new Consent(NotificationId, new DateRange(new DateTime(2015, 01, 01), nowPlusOne), string.Empty, new Guid("26342B36-15A4-4AC4-BAE0-9C2CA36B0CD5"));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
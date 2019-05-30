﻿namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class NumberOfActiveLoadsTests : IDisposable
    {
        private static readonly Guid NotificationId = new Guid("0E38E99F-A997-4014-8438-62B56E0398DF");
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly NumberOfActiveLoads numberOfActiveLoads;
        private readonly IMovementRepository movementRepository;
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        public NumberOfActiveLoadsTests()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            movementRepository = A.Fake<IMovementRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            numberOfActiveLoads = new NumberOfActiveLoads(movementRepository, financialGuaranteeRepository);
        }

        [Fact]
        public async Task ActiveLoadsPermittedReached_ReturnsTrue()
        {
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee());
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(2));

            var result = await numberOfActiveLoads.HasMaximum(NotificationId, SystemTime.UtcNow);

            Assert.True(result);
        }

        [Fact]
        public async Task ActiveLoadsPermittedExceeded_ReturnsTrue()
        {
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee());
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(3));

            var result = await numberOfActiveLoads.HasMaximum(NotificationId, SystemTime.UtcNow);

            Assert.True(result);
        }

        [Fact]
        public async Task ActiveLoadsPermittedNotReached_ReturnsFalse()
        {
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee());
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(1));

            var result = await numberOfActiveLoads.HasMaximum(NotificationId, SystemTime.UtcNow);

            Assert.False(result);
        }

        [Fact]
        public async Task ActiveLoadsPermittedReached_NewMovementForDifferentDate_ReturnsFalse()
        {
            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId)).Returns(GetFinancialGuarantee());
            A.CallTo(() => movementRepository.GetAllActiveMovements(NotificationId)).Returns(GetMovementArray(2));

            var result = await numberOfActiveLoads.HasMaximum(NotificationId, SystemTime.UtcNow.AddDays(5));

            Assert.False(result);
        }

        private static IEnumerable<TestableMovement> GetMovements(int numberOfMovements)
        {
            for (int i = 0; i < numberOfMovements; i++)
            {
                yield return new TestableMovement
                {
                    Id = Guid.NewGuid(),
                    NotificationId = NotificationId
                };
            }
        }

        private static FinancialGuaranteeCollection GetFinancialGuarantee()
        {
            var collection = new FinancialGuaranteeCollection(NotificationId);

            var fg = collection.AddFinancialGuarantee(new DateTime(2015, 1, 1));
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.ActiveLoadsPermitted, 2, fg);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.Status, FinancialGuaranteeStatus.Approved, fg);

            return collection;
        }

        private IEnumerable<Movement> GetMovementArray(int n)
        {
            var movements = new List<Movement>();

            for (int i = 0; i < n; i++)
            {
                movements.Add(new Movement(i + 1, NotificationId, SystemTime.UtcNow, userId));
            }

            return movements;
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}

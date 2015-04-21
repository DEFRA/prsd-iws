namespace EA.Iws.Web.Tests.Unit.TestHelpers.Factories
{
    using Core.Cqrs;
    using FakeItEasy;

    internal class MessageBusFactory
    {
        public IQueryBus GetQueryBus()
        {
            return A.Fake<IQueryBus>();
        }

        public ICommandBus GetCommandBus()
        {
            return A.Fake<ICommandBus>();
        }
    }
}

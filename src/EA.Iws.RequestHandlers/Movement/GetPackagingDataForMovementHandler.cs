namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.PackagingType;
    using DataAccess;
    using EA.Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetPackagingDataForMovementHandler : IRequestHandler<GetPackagingDataForMovement, PackagingData>
    {
        private readonly IwsContext context;

        public GetPackagingDataForMovementHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<PackagingData> HandleAsync(GetPackagingDataForMovement message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);

            var packagingData = new PackagingData();

            foreach (var packagingInfo in movement.PackagingInfos)
            {
                packagingData.PackagingTypes.Add((PackagingType)packagingInfo.PackagingType.Value);
                if (packagingInfo.OtherDescription != null)
                {
                    packagingData.OtherDescription = packagingInfo.OtherDescription;
                }
            }

            return packagingData;
        }
    }
}

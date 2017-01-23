namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using MovementTableData = Core.ImportNotificationMovements.MovementTableData;

    internal class GetImportMovementsByNotificationIdHandler :
        IRequestHandler<GetImportMovementsByNotificationId, MovementTableData[]>
    {
        private readonly IMap<IEnumerable<Domain.ImportMovement.MovementTableData>, IEnumerable<MovementTableData>>
            mapper;

        private readonly IImportMovementTableDataRepository tableDataRepository;

        public GetImportMovementsByNotificationIdHandler(IImportMovementTableDataRepository tableDataRepository,
            IMap<IEnumerable<Domain.ImportMovement.MovementTableData>, IEnumerable<MovementTableData>> mapper)
        {
            this.tableDataRepository = tableDataRepository;
            this.mapper = mapper;
        }

        public async Task<MovementTableData[]> HandleAsync(GetImportMovementsByNotificationId message)
        {
            var tableData = await tableDataRepository.GetById(message.ImportNotificationId);

            return mapper.Map(tableData).ToArray();
        }
    }
}
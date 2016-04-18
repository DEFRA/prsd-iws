namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovementBlockFactory
    {
        Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields);
    }
}
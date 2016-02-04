namespace EA.Iws.Core.OperationCodes
{
    using System.ComponentModel.DataAnnotations;
    using Shared;

    public enum OperationCode
    {
        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R1", ResourceType = typeof(OperationCodeResources), Description = "R1Description")]
        R1 = 1,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R2", ResourceType = typeof(OperationCodeResources), Description = "R2Description")]
        R2 = 2,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R3", ResourceType = typeof(OperationCodeResources), Description = "R3Description")]
        R3 = 3,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R4", ResourceType = typeof(OperationCodeResources), Description = "R4Description")]
        R4 = 4,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R5", ResourceType = typeof(OperationCodeResources), Description = "R5Description")]
        R5 = 5,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R6", ResourceType = typeof(OperationCodeResources), Description = "R6Description")]
        R6 = 6,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R7", ResourceType = typeof(OperationCodeResources), Description = "R7Description")]
        R7 = 7,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R8", ResourceType = typeof(OperationCodeResources), Description = "R8Description")]
        R8 = 8,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R9", ResourceType = typeof(OperationCodeResources), Description = "R9Description")]
        R9 = 9,
        
        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R10", ResourceType = typeof(OperationCodeResources), Description = "R10Description")]
        R10 = 10,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R11", ResourceType = typeof(OperationCodeResources), Description = "R11Description")]
        R11 = 11,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R12", ResourceType = typeof(OperationCodeResources), Description = "R12Description")]
        R12 = 12,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R13", ResourceType = typeof(OperationCodeResources), Description = "R13Description")]
        R13 = 13,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D1", ResourceType = typeof(OperationCodeResources), Description = "D1Description")]
        D1 = 14,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D2", ResourceType = typeof(OperationCodeResources), Description = "D2Description")]
        D2 = 15,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D3", ResourceType = typeof(OperationCodeResources), Description = "D3Description")]
        D3 = 16,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D4", ResourceType = typeof(OperationCodeResources), Description = "D4Description")]
        D4 = 17,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D5", ResourceType = typeof(OperationCodeResources), Description = "D5Description")]
        D5 = 18,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D6", ResourceType = typeof(OperationCodeResources), Description = "D6Description")]
        D6 = 19,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D7", ResourceType = typeof(OperationCodeResources), Description = "D7Description")]
        D7 = 20,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D8", ResourceType = typeof(OperationCodeResources), Description = "D8Description")]
        D8 = 21,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D9", ResourceType = typeof(OperationCodeResources), Description = "D9Description")]
        D9 = 22,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D10", ResourceType = typeof(OperationCodeResources), Description = "D10Description")]
        D10 = 23,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D11", ResourceType = typeof(OperationCodeResources), Description = "D11Description")]
        D11 = 24,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D12", ResourceType = typeof(OperationCodeResources), Description = "D12Description")]
        D12 = 25,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D13", ResourceType = typeof(OperationCodeResources), Description = "D13Description")]
        D13 = 26,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D14", ResourceType = typeof(OperationCodeResources), Description = "D14Description")]
        D14 = 27,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D15", ResourceType = typeof(OperationCodeResources), Description = "D15Description")]
        D15 = 28
    }
}

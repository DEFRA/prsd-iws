// ReSharper disable LocalizableElement
// ReSharper warning disabled because enum names and descriptions are not localize-able.
namespace EA.Iws.Core.OperationCodes
{
    using System.ComponentModel.DataAnnotations;
    using Shared;

    public enum OperationCode
    {
        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R1", Description 
            = "Use as a fuel (other than in a direct incineration) or other means to generate energy (Basel/OECD) – Use principally as a fuel or other means to generate energy (EU)")]
        R1 = 1,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R2", Description = "Solvent reclamation/regeneration")]
        R2 = 2,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R3", Description = "Recycling/reclamation of organic substances which are not used as solvents")]
        R3 = 3,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R4", Description = "Recycling/reclamation of metals and metal compounds")]
        R4 = 4,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R5", Description = "Recycling/reclamation of other inorganic materials")]
        R5 = 5,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R6", Description = "Regeneration of acids or bases")]
        R6 = 6,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R7", Description = "Recovery of components used for pollution abatement")]
        R7 = 7,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R8", Description = "Recovery of components from catalysts")]
        R8 = 8,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R9", Description = "Used oil re-refining or other reuses of previously used oil")]
        R9 = 9,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R10", Description = "Land treatment resulting in benefit to agriculture or ecological improvement")]
        R10 = 10,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R11", Description = "Uses of residual materials obtained from any of the operations numbered R1 - R10")]
        R11 = 11,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R12", Description = "Exchange of wastes for submission to any of the operations numbered R1 - R11")]
        R12 = 12,

        [OperationCodeType(NotificationType.Recovery)]
        [Display(Name = "R13", Description = "Accumulation of material intended for any operation in this list")]
        R13 = 13,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D1", Description = "Deposit into or onto land (e.g., landfill, etc.)")]
        D1 = 14,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D2", Description = "Land treatment, (e.g., biodegradation of liquid or sludgy discards in soils, etc.)")]
        D2 = 15,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D3", Description = "Deep injection, (e.g., injection of pumpable discards into wells, salt domes or naturally occurring repositories, etc.)")]
        D3 = 16,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D4", Description = "Surface impoundment, (e.g., placement of liquid or sludge discards into pits, ponds or lagoons, etc.)")]
        D4 = 17,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D5", Description = "Specially engineered landfill, (e.g., placement into lined discrete cells which are capped and isolated from one another and the environment, etc.)")]
        D5 = 18,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D6", Description = "Release into water body except seas / oceans")]
        D6 = 19,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D7", Description = "Release into seas/oceans including sea-bed insertion")]
        D7 = 20,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D8", Description 
            = "Biological treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list")]
        D8 = 21,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D9", Description 
            = "Physico-chemical treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list (e.g., evaporation, drying, calcination, etc.,)")]
        D9 = 22,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D10", Description = "Incineration on land")]
        D10 = 23,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D11", Description = "Incineration at sea")]
        D11 = 24,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D12", Description = "Permanent storage (e.g., emplacement of containers in a mine, etc.)")]
        D12 = 25,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D13", Description = "Blending or mixing prior to submission to any of the operations in this list")]
        D13 = 26,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D14", Description = "Repackaging prior to submission to any of the operations in this list")]
        D14 = 27,

        [OperationCodeType(NotificationType.Disposal)]
        [Display(Name = "D15", Description = "Storage pending any of the operations numbered in this list")]
        D15 = 28
    }
}

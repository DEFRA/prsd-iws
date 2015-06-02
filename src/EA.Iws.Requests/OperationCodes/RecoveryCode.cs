namespace EA.Iws.Requests.OperationCodes
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum RecoveryCode
    {
        [Display(Name = "R1")]
        [Description("Use as a fuel (other than in a direct incineration) or other means to generate energy (Basel/OECD) – Use principally as a fuel or other means to generate energy (EU)")]
        R1 = 1,

        [Display(Name = "R2")]
        [Description("Solvent reclamation/regeneration")]
        R2 = 2,

        [Display(Name = "R3")]
        [Description("Recycling/reclamation of organic substances which are not used as solvents")]
        R3 = 3,

        [Display(Name = "R4")]
        [Description("Recycling/reclamation of metals and metal compounds")]
        R4 = 4,

        [Display(Name = "R5")]
        [Description("Recycling/reclamation of other inorganic materials")]
        R5 = 5,

        [Display(Name = "R6")]
        [Description("Regeneration of acids or bases")]
        R6 = 6,

        [Display(Name = "R7")]
        [Description("Recovery of components used for pollution abatement")]
        R7 = 7,

        [Display(Name = "R8")]
        [Description("Recovery of components from catalysts")]
        R8 = 8,

        [Display(Name = "R9")]
        [Description("Used oil re-refining or other reuses of previously used oil")]
        R9 = 9,

        [Display(Name = "R10")]
        [Description("Land treatment resulting in benefit to agriculture or ecological improvement")]
        R10 = 10,

        [Display(Name = "R11")]
        [Description("Uses of residual materials obtained from any of the operations numbered R1-R10")]
        R11 = 11,

        [Display(Name = "R12")]
        [Description("Exchange of wastes for submission to any of the operations numbered R1-R11")]
        R12 = 12,

        [Display(Name = "R13")]
        [Description("Accumulation of material intended for any operation in this list")]
        R13 = 13
    }
}

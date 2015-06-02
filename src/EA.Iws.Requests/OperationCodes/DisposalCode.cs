namespace EA.Iws.Requests.OperationCodes
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum DisposalCode
    {
        [Display(Name = "D1")]
        [Description("Deposit into or onto land (e.g., landfill, etc.)")]
        D1 = 1,

        [Display(Name = "D2")]
        [Description("Land treatment, (e.g., biodegradation of liquid or sludgy discards in soils, etc.)")]
        D2 = 2,

        [Display(Name = "D3")]
        [Description("Deep injection, (e.g., injection of pumpable discards into wells, salt domes or naturally occurring repositories, etc.)")]
        D3 = 3,

        [Display(Name = "D4")]
        [Description("Surface impoundment, (e.g., placement of liquid or sludge discards into pits, ponds or lagoons, etc.)")]
        D4 = 4,

        [Display(Name = "D5")]
        [Description("Specially engineered landfill, (e.g., placement into lined discrete cells which are capped and isolated from one another and the environment, etc.)")]
        D5 = 5,

        [Display(Name = "D6")]
        [Description("Release into water body except seas / oceans")]
        D6 = 6,

        [Display(Name = "D7")]
        [Description("Release into seas/oceans including sea-bed insertion")]
        D7 = 7,

        [Display(Name = "D8")]
        [Description("Biological treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list")]
        D8 = 8,

        [Display(Name = "D9")]
        [Description("Physico-chemical treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list (e.g., evaporation, drying, calcination, etc.,)")]
        D9 = 9,

        [Display(Name = "D10")]
        [Description("Incineration on land")]
        D10 = 10,

        [Display(Name = "D11")]
        [Description("Incineration at sea")]
        D11 = 11,

        [Display(Name = "D12")]
        [Description("Permanent storage (e.g., emplacement of containers in a mine, etc.)")]
        D12 = 12,

        [Display(Name = "D13")]
        [Description("Blending or mixing prior to submission to any of the operations in this list")]
        D13 = 13,
        
        [Display(Name = "D14")]
        [Description("Repackaging prior to submission to any of the operations in this list")]
        D14 = 14,
        
        [Display(Name = "D15")]
        [Description("Storage pending any of the operations numbered in this list")]
        D15 = 15
    }
}

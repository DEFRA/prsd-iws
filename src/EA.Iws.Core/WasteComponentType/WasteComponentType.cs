namespace EA.Iws.Core.WasteComponentType
{
    using System.ComponentModel.DataAnnotations;

    public enum WasteComponentType
    {
        [Display(Name = "Mercury")]
        Mercury = 1,

        [Display(Name = "FGas/ODS")]
        FGasODS = 2,

        [Display(Name = "NORM")]
        NORM = 3        
    }
}

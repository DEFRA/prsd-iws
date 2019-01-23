namespace EA.Iws.Core.Movement.Bulk
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkMovementFileRules
    {
        [Display(Name = "The file type must be either an EXCEL or .CSV format.")]
        FileType,
        [Display(Name = "The file size must not be larger than 2GB.")]
        FileSize,
        [Display(Name = "We've detected a virus in the file you uploaded.")]
        Virus
    }
}

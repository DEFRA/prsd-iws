namespace EA.Iws.Core.Movement.BulkUpload
{
    using System.ComponentModel.DataAnnotations;

    public enum BulkFileRules
    {
        [Display(Name = "The file type must be either in .XLS or .XLSX or .CSV format.")]
        DataFileType,
        [Display(Name = "The data file must not exceed 2GB in size.")]
        FileSize,
        [Display(Name = "We've detected a virus in the file you uploaded.")]
        Virus,
        [Display(Name = "Unable to read the file, format is invalid.")]
        FileParse,
        [Display(Name = "The file does not contain any data.")]
        EmptyData,
        [Display(Name = "The file type is unsupported: please upload a PDF, image or standard MS Office or Open Office file.")]
        SupportingDocumentFileType
    }
}

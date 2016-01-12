namespace EA.Iws.Web.Infrastructure.Validation
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class RestrictToAllowedUploadTypesAttribute : ValidationAttribute
    {
        private static readonly string[] AllowedUploadTypes;

        static RestrictToAllowedUploadTypesAttribute()
        {
            AllowedUploadTypes = new[]
            {
                MimeTypes.MSWord,
                MimeTypes.MSWordXml,
                MimeTypes.MSExcel,
                MimeTypes.MSExcelXml,
                MimeTypes.MSPowerPoint,
                MimeTypes.MSPowerPointXml,
                MimeTypes.OpenOfficeText,
                MimeTypes.OpenOfficeSpreadsheet,
                MimeTypes.OpenOfficePresentation,
                MimeTypes.Pdf,
                MimeTypes.Jpeg,
                MimeTypes.Png,
                MimeTypes.Gif,
                MimeTypes.Bitmap,
                MimeTypes.Rtf
            };
        }

        public RestrictToAllowedUploadTypesAttribute()
        {
            ErrorMessage = RestrictToAllowedUploadTypesAttributeResources.DefaultErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpPostedFile = value as HttpPostedFileBase;

            if (value == null || (httpPostedFile != null && IsValidUploadType(httpPostedFile.ContentType)))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

        private static bool IsValidUploadType(string contentType)
        {
            return AllowedUploadTypes.Contains(contentType);
        }
    }
}
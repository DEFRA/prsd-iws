namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using Domain;
    using UKCompetentAuthority = Core.Notification.UKCompetentAuthority;

    public class PostageLabelGenerator : IPostageLabelGenerator
    {
        public byte[] GeneratePostageLabel(UKCompetentAuthority competentAuthority)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared(GetFileName(competentAuthority)))
            {
                return memoryStream.ToArray();
            }
        }

        private string GetFileName(UKCompetentAuthority competentAuthority)
        {
            string filename = string.Empty;

            if (competentAuthority == UKCompetentAuthority.England)
            {
                filename = "EaAddress.pdf";
            }

            if (competentAuthority == UKCompetentAuthority.NorthernIreland)
            {
                filename = "NieaAddress.pdf";
            }

            if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                filename = "SepaAddress.pdf";
            }

            if (competentAuthority == UKCompetentAuthority.Wales)
            {
                filename = "NrwAddress.pdf";
            }

            return filename;
        }
    }
}

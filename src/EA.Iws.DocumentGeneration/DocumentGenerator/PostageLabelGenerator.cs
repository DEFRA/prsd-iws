namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using Domain;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    public class PostageLabelGenerator : IPostageLabelGenerator
    {
        public byte[] GeneratePostageLabel(CompetentAuthority competentAuthority)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared(GetFileName(competentAuthority)))
            {
                return memoryStream.ToArray();
            }
        }

        private string GetFileName(CompetentAuthority competentAuthority)
        {
            string filename = string.Empty;

            if (competentAuthority == CompetentAuthority.England)
            {
                filename = "EaAddress.pdf";
            }

            if (competentAuthority == CompetentAuthority.NorthernIreland)
            {
                filename = "NieaAddress.pdf";
            }

            if (competentAuthority == CompetentAuthority.Scotland)
            {
                filename = "SepaAddress.pdf";
            }

            if (competentAuthority == CompetentAuthority.Wales)
            {
                filename = "NrwAddress.pdf";
            }

            return filename;
        }
    }
}

namespace EA.Iws.RequestHandlers.Documents
{
    internal interface IPdfGenerator
    {
        byte[] ConvertToPdf(byte[] docxBytes);
    }
}
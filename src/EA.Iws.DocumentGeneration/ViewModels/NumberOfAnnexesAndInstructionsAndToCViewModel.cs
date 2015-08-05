namespace EA.Iws.DocumentGeneration.ViewModels
{
    internal class NumberOfAnnexesAndInstructionsAndToCViewModel
    {
        public NumberOfAnnexesAndInstructionsAndToCViewModel(string toc, string instructions, int numberOfAnnexes)
        {
            ToC = toc;
            Instructions = instructions;
            NumberOfAnnexes = numberOfAnnexes;
        }

        public string ToC { get; private set; }
        public string Instructions { get; private set; }
        public int NumberOfAnnexes { get; private set; }
    }
}

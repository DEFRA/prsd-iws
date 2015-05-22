namespace EA.Iws.DocumentGeneration
{
    using System;

    public class MergeFieldName
    {
        public MergeFieldName(string name)
        {
            if (name.Contains("[")
                && name.Contains("]"))
            {
                var subTypeStart = name.IndexOf("[", StringComparison.InvariantCultureIgnoreCase);

                OuterTypeName = name.Substring(0, subTypeStart);
                InnerTypeName = name.Substring(subTypeStart + 1, name.Length - subTypeStart - 2);
            }
            else
            {
                InnerTypeName = name;
            }
        }

        public string OuterTypeName { get; private set; }

        public string InnerTypeName { get; private set; }

        public override string ToString()
        {
            return OuterTypeName + "[" + InnerTypeName + "]";
        }
    }
}
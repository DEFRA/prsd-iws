namespace EA.Iws.Domain.FileStore
{
    using System;
    using Prsd.Core.Identity;

    public class File
    {
        public File(string name, string type, byte[] content)
        {
            Id = GuidCombGenerator.GenerateComb();
            Name = name;
            Type = type;
            Content = content;
        }

        internal File(Guid id, string name, string type, byte[] content)
        {
            Id = id;
            Name = name;
            Type = type;
            Content = content;
        }

        protected File()
        {
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public byte[] Content { get; private set; }
    }
}
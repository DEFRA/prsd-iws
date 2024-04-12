namespace EA.Iws.Domain
{
    public class SystemSetting
    {
        protected SystemSetting()
        {
        }

        public int Id { get; private set; }

        public string Value { get; private set; }

        public string Description { get; private set; }

        public bool Equals(int id)
        {
            return this.Id == id;
        }
    }
}
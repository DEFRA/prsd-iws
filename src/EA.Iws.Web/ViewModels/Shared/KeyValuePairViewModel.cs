namespace EA.Iws.Web.ViewModels.Shared
{
    public class KeyValuePairViewModel<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public KeyValuePairViewModel()
        {
        }

        public KeyValuePairViewModel(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
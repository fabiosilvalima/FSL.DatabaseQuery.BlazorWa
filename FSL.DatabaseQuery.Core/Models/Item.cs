namespace FSL.DatabaseQuery.Core.Models
{
    public sealed class Item : IDropDownListItem
    {
        public Item(
            string text,
            string value)
        {
            Text = text;
            Value = value;
        }
        public string Text { get; }

        public string Value { get; }
    }
}

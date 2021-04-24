namespace LL1Generator.Entities
{
    public class RuleItem
    {
        public readonly bool IsTerminal;
        public readonly string Value;

        public RuleItem(string Value, bool IsTerminal)
        {
            this.Value = Value;
            this.IsTerminal = IsTerminal;
        }
    }
}
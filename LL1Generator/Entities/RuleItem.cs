namespace LL1Generator.Entities
{
    public class RuleItem
    {
        public readonly bool IsTerminal;
        public readonly string Value;

        public RuleItem(string value, bool isTerminal)
        {
            Value = value;
            IsTerminal = isTerminal;
        }

        public override string ToString()
        {
            return $"{Value} | {(IsTerminal ? "1" : "0")}";
        }
    }
}
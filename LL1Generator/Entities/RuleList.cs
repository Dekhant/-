using System.Collections.Generic;

namespace LL1Generator.Entities
{
    public class RuleList
    {
        public readonly List<Rule> Rules;
        public List<string> Alphabet = new();
        public readonly HashSet<string> NonTerminals;

        public RuleList(HashSet<string> nonTerminals, List<Rule> rules)
        {
            NonTerminals = nonTerminals;
            Rules = rules;
        }
    }
}
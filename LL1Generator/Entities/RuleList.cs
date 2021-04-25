using System.Collections.Generic;

namespace LL1Generator.Entities
{
    public class RuleList
    {
        public HashSet<string> NonTerminals;
        public readonly List<Rule> Rules;
        public List<string> Alphabet = new();

        public RuleList(HashSet<string> nonTerminals, List<Rule> rules)
        {
            NonTerminals = nonTerminals;
            Rules = rules;
        }
    }
}
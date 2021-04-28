using System.Collections.Generic;
using System.Linq;

namespace LL1Generator.Entities
{
    public class Rule
    {
        public string NonTerminal { get; set; }
        public List<RuleItem> Items { get; init; }

        public override string ToString()
        {
            return NonTerminal + " -> " + string.Join(" ", Items.Select(x => x.Value));
        }
    }
}
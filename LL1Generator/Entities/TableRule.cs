using System.Collections.Generic;

namespace LL1Generator.Entities
{
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        // Классно придумал
        public List<RuleItem> FirstsSet { get; init; }
        public HashSet<string> DirSet { get; set; }
        public int? GoTo { get; init; }
        public bool IsError { get; set; }
        public bool IsShift { get; init; }
        public bool MoveToStack { get; init; }
        public bool IsEnd { get; init; }
    }
}
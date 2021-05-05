using System.Collections.Generic;

namespace LL1Generator.Entities
{
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        public List<RuleItem> FirstsSet { get; set; }
        public int? GoTo { get; set; }
        public bool IsError { get; set; }
        public bool IsShift { get; init; }
        public bool MoveToStack { get; init; }
        public bool IsEnd { get; init; }
    }
}
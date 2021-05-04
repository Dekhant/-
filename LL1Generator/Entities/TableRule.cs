using System.Collections.Generic;

namespace LL1Generator.Entities
{
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        public HashSet<string> FirstsSet { get; init; }
        public int? GoTo { get; set; }
        public bool IsError { get; init; }
        public bool IsShift { get; init; }
        public bool MoveToStack { get; init; }
        public bool IsEnd { get; init; }

        public override string ToString()
        {
            return $"Id: {Id}, NonTerm: {NonTerminal}, Firsts: {string.Join(", ", FirstsSet)}, " +
                   $"Goto: {(GoTo == null ? "NULL" : GoTo)}, Err: {IsError}, " +
                   $"Shift: {IsShift}, Stack: {MoveToStack}, End: {IsEnd}";
        }
    }
}
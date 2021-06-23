using System.Collections.Generic;
using System.IO;
using LL1Generator.Entities;

namespace LL1Generator.Tests
{
    public class Extension
    {
        public static List<string> CheckTests(string way, List<string> rules)
        {
            var parsedRules = Parser.ParseInput(File.OpenRead(way));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules);
            if (leads == null) return null;

            for (var i = 0; i < removedRecursionRules.Rules.Count; i++)
                rules.Add(removedRecursionRules.Rules[i] + " / " + ConvertLead(leads[i]));

            return rules;
        }


        private static string ConvertLead(IReadOnlyList<RuleItem> lead)
        {
            var leadLine = "";

            // можно переделать на string.Join
            for (var i = 0; i < lead.Count; i++)
            {
                leadLine += lead[i].Value;
                if (lead.Count > 1 && i != lead.Count - 1) leadLine += ", ";
            }

            return leadLine;
        }
    }
}
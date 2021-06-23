using System.Collections.Generic;
using System.IO;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator.Tests
{
    public static class Extension
    {
        public static List<string> CheckTests(string way, List<string> rules)
        {
            var parsedRules = Parser.ParseInput(File.OpenRead(way));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules).ToList();

            rules.AddRange(removedRecursionRules.Rules.Select((t, i) => t + " / " + ConvertLead(leads[i])));

            return rules;
        }


        private static string ConvertLead(HashSet<RuleItem> lead)
        {
            var leadLine = "";

            // можно переделать на string.Join
            for (var i = 0; i < lead.Count; i++)
            {
                leadLine += lead.ToList()[i].Value;
                if (lead.Count > 1 && i != lead.Count - 1) leadLine += ", ";
            }

            return leadLine;
        }
    }
}
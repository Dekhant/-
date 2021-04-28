using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Factorization
    {
        private static IEnumerable<Rule> GetLongestCommonPrefix(IList<Rule> rules, ref List<string> alphabet,
            ref bool didChange,
            ref List<string> nonTermsToAdd)
        {
            var factorContainer = new List<List<Rule>>();
            var newRules = new List<Rule>();
            while (rules.Any())
            {
                var commonRulesList = new List<Rule>();
                var foundSimilar = false;
                for (var i = 1; i < rules.Count; i++)
                    if (rules[0].Items[0].Value == rules[i].Items[0].Value)
                    {
                        foundSimilar = true;
                        commonRulesList.Add(rules[i]);
                        rules.RemoveAt(i);
                        i--;
                    }

                if (foundSimilar)
                    commonRulesList.Add(rules[0]);
                else
                    newRules.Add(rules[0]);
                rules.RemoveAt(0);
                if (commonRulesList.Any()) factorContainer.Add(commonRulesList);
            }

            if (factorContainer.Any()) didChange = true;
            foreach (var factorContainerItem in factorContainer)
            {
                var maxRuleCount = int.MaxValue;
                foreach (var factorRules in factorContainerItem.Skip(1))
                {
                    var maxCount = 0;
                    for (var j = 0; j < factorRules.Items.Count; j++)
                        if (factorContainerItem[0].Items[j].Value == factorRules.Items[j].Value)
                            maxCount++;
                        else
                            break;
                    if (maxCount < maxRuleCount) maxRuleCount = maxCount;
                }

                var rule = new Rule {Items = new List<RuleItem>()};
                var freeLetter = alphabet[0];
                for (var j = 0; j < maxRuleCount; j++)
                    rule.Items.Add(new RuleItem(factorContainerItem[0].Items[j].Value,
                        factorContainerItem[0].Items[j].IsTerminal));
                rule.NonTerminal = factorContainerItem[0].NonTerminal;
                rule.Items.Add(new RuleItem(freeLetter, false));
                newRules.Add(rule);
                foreach (var factorItem in factorContainerItem)
                {
                    var factorRule = new Rule {Items = new List<RuleItem>()};
                    if (factorItem.Items.Count == maxRuleCount)
                        factorRule.Items.Add(new RuleItem(Constants.EmptySymbol, true));
                    for (var j = maxRuleCount; j < factorItem.Items.Count; j++)
                        factorRule.Items.Add(new RuleItem(factorItem.Items[j].Value, factorItem.Items[j].IsTerminal));
                    factorRule.NonTerminal = freeLetter;
                    newRules.Add(factorRule);
                }

                alphabet.RemoveAt(0);
                nonTermsToAdd.Add(freeLetter);
            }

            return newRules;
        }

        public static RuleList RemoveFactorization(RuleList ruleList)
        {
            var didChange = true;
            while (didChange)
            {
                didChange = false;
                var nonTermsToAdd = new List<string>();
                foreach (var nonTerm in ruleList.NonTerminals.ToList())
                {
                    var alphabet = ruleList.Alphabet;
                    var rulesToPrefixCheck = ruleList.Rules.Where(x => x.NonTerminal == nonTerm).ToList();

                    var factorizedRules = GetLongestCommonPrefix(rulesToPrefixCheck, ref alphabet, ref didChange,
                        ref nonTermsToAdd);
                    ruleList.Alphabet = alphabet;

                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm).ToList())
                        ruleList.Rules.Remove(rule);
                    foreach (var factorizedRule in factorizedRules) ruleList.Rules.Add(factorizedRule);
                    foreach (var item in nonTermsToAdd) ruleList.NonTerminals.Add(item);
                }
            }

            return ruleList;
        }
    }
}
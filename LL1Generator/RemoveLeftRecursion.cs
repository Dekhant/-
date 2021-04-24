using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1Generator
{
    public class ParsedRuleProcessor
    {
        public static List<Rule> RemoveLeftRecursion(RuleList rules)
        {
            var newRuleList = new List<Rule>();
            foreach(var NonTerm in rules.NonTerminals)
            {
                var freeLetter = rules.Alphabet[0];
                List<Rule> commonRules = new List<Rule>();
                List<Rule> leftRecursionRules = new List<Rule>();
                foreach (var rule in rules.Rules.Where(x => x.NonTerminal == NonTerm))
                {
                   
                    
                    if (rule.Items[0].Value == NonTerm)
                    {
                        leftRecursionRules.Add(rule);
                    }
                    else
                    {
                        commonRules.Add(rule);
                    }
                    
                }
                if(leftRecursionRules.Any())
                {
                    rules.Alphabet.RemoveAt(0);
                    foreach (var commonRule in commonRules)
                    {
                        commonRule.Items.Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(commonRule);
                    }
                    foreach (var leftRecursionRule in leftRecursionRules)
                    {
                        leftRecursionRule.Items.RemoveAt(0);
                        leftRecursionRule.Items.Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(new Rule
                        {
                            NonTerminal = freeLetter,
                            Items = leftRecursionRule.Items
                        });
                    }
                    var emptyRule = new Rule
                    {
                        NonTerminal = freeLetter,
                        Items = new List<RuleItem>
                        {
                            new RuleItem(Constants.EmptySymbol, true)
                        }
                    };
                    newRuleList.Add(emptyRule);
                }
                else
                {
                    foreach(var commonRule in commonRules)
                    {
                        newRuleList.Add(commonRule);
                    }
                }
            }
            return newRuleList;
        }
    }
}

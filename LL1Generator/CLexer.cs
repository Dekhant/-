using System;
using System.Collections.Generic;
using System.IO;

namespace LL1Generator
{
    public class CLexer
    {
        private readonly string[] m_arithmeticalOperators = {"+", "-", "*", "/", "="};
        private readonly string[] m_array = {"[", "]", "[]"};
        private readonly string[] m_binary = {"0", "1"};
        private readonly string[] m_brackets = {"{", "}", "(", ")"};
        private readonly string[] m_comments = {"//", "/*", "*/"};
        private readonly string[] m_comparisonOperators = {"equal", "!=", ">", "<", ">=", "<="};
        private readonly string[] m_decimal = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

        private readonly string[] m_hexadecimal =
            {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"};

        private readonly string[] m_keywords =
        {
            "if", "else", "while", "for", "read", "write", "return", "int", "float",
            "void", "func", "string", "char"
        };

        private readonly string[] m_letters =
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R",
            "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
            "n", "o", "p",
            "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };

        private readonly string[] m_logicOperators = {"or", "and", "!"};
        private readonly string[] m_octal = {"0", "1", "2", "3", "4", "5", "6", "7"};
        private readonly string[] m_separators = {",", ";"};
        private readonly StreamReader input;
        private readonly List<Token> m_tokens = new();
        private readonly StreamWriter output;

        public CLexer(ref StreamReader input, ref StreamWriter output)
        {
            this.input = input;
            this.output = output;
        }

        private string GetTokenName(Token token)
        {
            return token.type switch
            {
                TokenType.Identifier => "Identifier",
                TokenType.Integer => "Integer",
                TokenType.Float => "Float",
                TokenType.Binary => "Binary",
                TokenType.Octal => "Octal",
                TokenType.Hexadecimal => "Hexadecimal",
                TokenType.Char => "Char",
                TokenType.String => "String",
                TokenType.Array => "Array",
                TokenType.Keyword => "Keyword",
                TokenType.ArithmeticOperator => "ArithmeticOperation",
                TokenType.ComparisonOperator => "ComparisonOperator",
                TokenType.LogicOperator => "LogicOperator",
                TokenType.Bracket => "Bracket",
                TokenType.Separator => "Separator",
                TokenType.Comment => "Comment",
                TokenType.Error => "Error",
                TokenType.EoF => "End of File",
                _ => "Unknown type"
            };
        }

        private bool findStringInList(string[] list, string s)
        {
            return Array.Find(list, a => a == s) != null;
        }

        private bool IsKeyword(string s)
        {
            return findStringInList(m_keywords, s);
        }

        private bool IsArray(string s)
        {
            return findStringInList(m_array, s);
        }

        private bool IsComment(string s)
        {
            return findStringInList(m_comments, s);
        }

        private bool IsArithmeticalOperator(string s)
        {
            return findStringInList(m_arithmeticalOperators, s);
        }

        private bool IsComparisonOperator(string s)
        {
            return findStringInList(m_comparisonOperators, s);
        }

        private bool IsLogicOperator(string s)
        {
            return findStringInList(m_logicOperators, s);
        }

        private bool IsBracket(string s)
        {
            return findStringInList(m_brackets, s);
        }

        private bool IsSeparator(string s)
        {
            return findStringInList(m_separators, s);
        }

        private bool IsBinary(string s)
        {
            return findStringInList(m_binary, s);
        }

        private bool IsOctal(string s)
        {
            return findStringInList(m_octal, s);
        }

        private bool IsDecimal(string s)
        {
            return findStringInList(m_decimal, s);
        }

        private bool IsHexadecimal(string s)
        {
            return findStringInList(m_hexadecimal, s);
        }

        private bool IsLetter(string s)
        {
            return findStringInList(m_letters, s);
        }

        private TokenType GetTokenType(Token token)
        {
            string s;
            if (token.type == TokenType.EoF)
                return TokenType.EoF;
            if (IsKeyword(token.value))
                return TokenType.Keyword;
            if (IsArray(token.value))
                return TokenType.Array;
            if (token.value[0] == '"' && token.value[token.value.Length - 1] == '"')
                return token.value.Length == 3 ? TokenType.Char : TokenType.String;
            if (token.value[0] == '\'' && token.value[token.value.Length - 1] == '\'')
                if (token.value.Length == 3)
                    return TokenType.Char;
            if (IsArithmeticalOperator(token.value))
                return TokenType.ArithmeticOperator;
            if (IsComparisonOperator(token.value))
                return TokenType.ComparisonOperator;
            if (IsLogicOperator(token.value))
                return TokenType.LogicOperator;
            if (IsBracket(token.value))
                return TokenType.Bracket;
            if (IsSeparator(token.value))
                return TokenType.Separator;
            if (token.value[0] == '/' && token.value[1] == '*') return TokenType.MultiCommStart;
            if (token.value[0] == '*' && token.value[1] == '/') return TokenType.MultiCommEnd;
            for (var i = 0; i < token.value.Length; i++)
            {
                s = token.value[i].ToString();
                if (!(IsLetter(s) || IsDecimal(s) || s == "_"))
                    break;
                s = token.value[0].ToString();
                if (IsLetter(s) && i + 1 == token.value.Length)
                    return TokenType.Identifier;
            }

            if (token.value.Length > 2)
            {
                var Base = token.value.Substring(0, 2);
                string digits;

                if (Base == "0b")
                {
                    digits = token.value.Substring(2, token.value.Length - 3);
                    var isDigits = true;
                    for (var i = 0; i < digits.Length; i++)
                    {
                        s = digits[i].ToString();
                        if (!IsBinary(s))
                            isDigits = false;
                    }

                    if (isDigits)
                        return TokenType.Binary;
                }
                else if (Base == "0o")
                {
                    digits = token.value.Substring(2, token.value.Length - 3);
                    var isDigits = true;
                    for (var i = 0; i < digits.Length; i++)
                    {
                        s = digits[i].ToString();
                        if (!IsOctal(s))
                            isDigits = false;
                    }

                    if (isDigits)
                        return TokenType.Octal;
                }
                else if (Base == "0x")
                {
                    digits = token.value.Substring(2, token.value.Length - 3);
                    var isDigits = true;
                    for (var i = 0; i < digits.Length; i++)
                    {
                        s = digits[i].ToString();
                        if (!IsHexadecimal(s))
                            isDigits = false;
                    }

                    if (isDigits)
                        return TokenType.Hexadecimal;
                }
            }

            var wereDot = false;
            var wereE = false;
            var E = 0;
            for (var i = 0; i < token.value.Length; i++)
            {
                if (wereE)
                    E++;
                s = token.value[i].ToString();
                if (IsDecimal(s)) continue;

                if (s == ".")
                {
                    if (wereDot == false && wereE == false)
                        wereDot = true;
                    else
                        return TokenType.Error;
                }
                else if (s == "E")
                {
                    if (wereE)
                        return TokenType.Error;
                    wereE = true;
                }
                else if ((s == "-" || s == "+") && i > 1)
                {
                    if (!(token.value[i - 1] == 'E'))
                        return TokenType.Error;
                }
                else
                {
                    return TokenType.Error;
                }
            }

            if (wereE && E < 2)
                return TokenType.Error;
            if (wereDot || wereE)
                return TokenType.Float;
            return TokenType.Integer;

            return TokenType.Error;
        }

        private void PrintTokens()
        {
            output.WriteLine(m_tokens.Count.ToString());
            Console.WriteLine(m_tokens.Count.ToString());
            for (var i = 0; i < m_tokens.Count; i++)
            {
                output.WriteLine(GetTokenName(m_tokens[i]) + " <" + m_tokens[i].value + "> line: " + m_tokens[i].line +
                                 " position: " + m_tokens[i].position);
                Console.WriteLine(GetTokenName(m_tokens[i]) + " <" + m_tokens[i].value + "> line: " + m_tokens[i].line +
                                  " position: " + m_tokens[i].position);
            }
        }

        private void AddToken(Token token)
        {
            if (token.value != "")
            {
                token.type = GetTokenType(token);
                if (token.type == TokenType.Identifier && token.value.Length > 64)
                    token.type = TokenType.Error;
                if (token.type == TokenType.Integer)
                {
                    if (token.value.Length > 10)
                        token.type = TokenType.Error;
                    else if (int.Parse(token.value) > int.MaxValue)
                        token.type = TokenType.Error;
                }

                m_tokens.Add(token);
            }
        }

        public void Run(ref List<string> lexerRules)
        {
            var lineNumber = 0;
            var i = 0;
            var line = "";
            var multiStringComment = false;
            var s = false;
            while ((line = input.ReadLine()) != null)
            {
                lineNumber++;
                Token token;
                token.value = "";
                token.line = 0;
                token.position = 0;
                token.type = TokenType.Error;
                string ch;
                var identifier = true;
                for (i = 0; i < line.Length; ++i)
                {
                    if (token.value == "")
                    {
                        token.line = lineNumber;
                        token.position = i + 1;
                    }

                    if (!multiStringComment && !s)
                        if (line[i] == '(' || line[i] == ')')
                        {
                            AddToken(token);
                            token.value = line[i].ToString();
                            token.line = lineNumber;
                            token.position = i + 1;
                            AddToken(token);
                            token.value = "";
                            continue;
                        }

                    if (i > 0 && !s && !multiStringComment)
                        if (line[i - 1] == '/' && line[i] == '/')
                        {
                            token.value = "";
                            break;
                        }

                    if (i + 1 == line.Length && s && (line[i] != '"' || line[i] != '\'') && !multiStringComment)
                    {
                        s = false;
                        token.value += line[i];
                        AddToken(token);
                        token.value = "";
                        token.position = i + 1;
                        continue;
                    }

                    if (multiStringComment)
                    {
                        if (i > 0)
                            if (line[i - 1] == '*' && line[i] == '/')
                                multiStringComment = false;
                        if (i + 1 == line.Length && !multiStringComment)
                            AddToken(token);
                        continue;
                    }

                    if (i > 0 && !s)
                        if (line[i - 1] == '/' && line[i] == '*')
                        {
                            multiStringComment = true;
                            continue;
                        }

                    if ((line[i] == ' ' || line[i] == '	') && !s && !multiStringComment)
                    {
                        AddToken(token);
                        token.position = i + 1;
                        token.value = "";
                        if (i + 1 == line.Length && !multiStringComment)
                            AddToken(token);
                        continue;
                    }

                    if (s && !multiStringComment)
                    {
                        if (line[i] != '"' && line[i] != '\'')
                        {
                            token.value += line[i];
                        }
                        else
                        {
                            token.value += line[i];
                            AddToken(token);
                            token.position = i + 1;
                            s = false;
                            token.value = "";
                        }

                        if (i + 1 == line.Length && !multiStringComment)
                            AddToken(token);
                        continue;
                    }

                    if ((line[i] == '"' || line[i] == '\'') && !multiStringComment)
                    {
                        s = true;
                        AddToken(token);
                        token.position = i + 1;
                        token.value = line[i].ToString();
                        if (i + 1 == line.Length && !multiStringComment)
                            AddToken(token);
                        continue;
                    }

                    ch = line[i].ToString();

                    if ((ch == "-" || ch == "+") && token.value.Length > 0 && identifier && !s && !multiStringComment &&
                        token.value != "E")
                        if (token.value[token.value.Length - 1] == 'E')
                        {
                            token.value += line[i];
                            if (i + 1 == line.Length && !multiStringComment)
                                AddToken(token);
                            continue;
                        }

                    if (IsLetter(ch) || IsDecimal(ch) || ch == "." || ch == "_")
                    {
                        if (identifier)
                        {
                            token.value += line[i];
                        }
                        else
                        {
                            AddToken(token);
                            token.position = i + 1;
                            if (line[i] == ' ' || line[i] == '	')
                                token.value = "";
                            else
                                token.value = line[i].ToString();
                            identifier = true;
                        }
                    }
                    else
                    {
                        if (identifier)
                        {
                            AddToken(token);
                            token.position = i + 1;
                            if (line[i] == ' ' || line[i] == '	')
                                token.value = "";
                            else
                                token.value = line[i].ToString();
                            identifier = false;
                        }
                        else
                        {
                            if (line[i] != ' ')
                            {
                                token.value += line[i];
                            }
                            else
                            {
                                AddToken(token);
                                token.position = i + 1;
                                token.value = "";
                            }
                        }
                    }

                    if (i + 1 == line.Length && !multiStringComment)
                        AddToken(token);
                }
            }

            Token token1;
            token1.value = "EoF";
            token1.type = TokenType.EoF;
            token1.line = lineNumber;
            token1.position = i;
            AddToken(token1);

            PrintTokens();
        }

        private void PrintTokensReforged(ref string result)
        {
            output.WriteLine(m_tokens.Count.ToString());
            Console.WriteLine(m_tokens.Count.ToString());
            for (var i = 0; i < m_tokens.Count; i++)
            {
                output.WriteLine(GetTokenName(m_tokens[i]) + " <" + m_tokens[i].value + ">");
                result = GetTokenName(m_tokens[i]); // + " <" + m_tokens[i].value + ">";
            }
        }

        public void RunPerToken(string wordToToken, ref string result)
        {
            var lineNumber = 0;
            var i = 0;
            var line = "";
            var multiStringComment = false;
            var s = false;
            line = wordToToken;

            lineNumber++;
            Token token;
            token.value = "";
            token.line = 0;
            token.position = 0;
            token.type = TokenType.Error;
            string ch;
            var identifier = true;
            for (i = 0; i < line.Length; ++i)
            {
                if (token.value == "")
                {
                    token.line = lineNumber;
                    token.position = i + 1;
                }

                if (!multiStringComment && !s)
                    if (line[i] == '(' || line[i] == ')')
                    {
                        AddToken(token);
                        token.value = line[i].ToString();
                        token.line = lineNumber;
                        token.position = i + 1;
                        AddToken(token);
                        token.value = "";
                        continue;
                    }

                if (i > 0 && !s && !multiStringComment)
                    if (line[i - 1] == '/' && line[i] == '/')
                    {
                        token.value = "";
                        break;
                    }

                if (i + 1 == line.Length && s && (line[i] != '"' || line[i] != '\'') && !multiStringComment)
                {
                    s = false;
                    token.value += line[i];
                    AddToken(token);
                    token.value = "";
                    token.position = i + 1;
                    continue;
                }

                if (multiStringComment)
                {
                    if (i > 0)
                        if (line[i - 1] == '*' && line[i] == '/')
                            multiStringComment = false;
                    if (i + 1 == line.Length && !multiStringComment)
                        AddToken(token);
                    continue;
                }

                if (i > 0 && !s)
                    if (line[i - 1] == '/' && line[i] == '*')
                    {
                        multiStringComment = true;
                        continue;
                    }

                if ((line[i] == ' ' || line[i] == '	') && !s && !multiStringComment)
                {
                    AddToken(token);
                    token.position = i + 1;
                    token.value = "";
                    if (i + 1 == line.Length && !multiStringComment)
                        AddToken(token);
                    continue;
                }

                if (s && !multiStringComment)
                {
                    if (line[i] != '"' && line[i] != '\'')
                    {
                        token.value += line[i];
                    }
                    else
                    {
                        token.value += line[i];
                        AddToken(token);
                        token.position = i + 1;
                        s = false;
                        token.value = "";
                    }

                    if (i + 1 == line.Length && !multiStringComment)
                        AddToken(token);
                    continue;
                }

                if ((line[i] == '"' || line[i] == '\'') && !multiStringComment)
                {
                    s = true;
                    AddToken(token);
                    token.position = i + 1;
                    token.value = line[i].ToString();
                    if (i + 1 == line.Length && !multiStringComment)
                        AddToken(token);
                    continue;
                }

                ch = line[i].ToString();

                if ((ch == "-" || ch == "+") && token.value.Length > 0 && identifier && !s && !multiStringComment &&
                    token.value != "E")
                    if (token.value[token.value.Length - 1] == 'E')
                    {
                        token.value += line[i];
                        if (i + 1 == line.Length && !multiStringComment)
                            AddToken(token);
                        continue;
                    }

                if (IsLetter(ch) || IsDecimal(ch) || ch == "." || ch == "_")
                {
                    if (identifier)
                    {
                        token.value += line[i];
                    }
                    else
                    {
                        AddToken(token);
                        token.position = i + 1;
                        if (line[i] == ' ' || line[i] == '	')
                            token.value = "";
                        else
                            token.value = line[i].ToString();
                        identifier = true;
                    }
                }
                else
                {
                    if (identifier)
                    {
                        AddToken(token);
                        token.position = i + 1;
                        if (line[i] == ' ' || line[i] == '	')
                            token.value = "";
                        else
                            token.value = line[i].ToString();
                        identifier = false;
                    }
                    else
                    {
                        if (line[i] != ' ')
                        {
                            token.value += line[i];
                        }
                        else
                        {
                            AddToken(token);
                            token.position = i + 1;
                            token.value = "";
                        }
                    }
                }

                if (token.value == "$") token.type = TokenType.EoF;

                if (i + 1 == line.Length && !multiStringComment)
                    AddToken(token);
            }


            //Token token1;
            //token1.value = "EoF";
            //token1.type = TokenType.EoF;
            //token1.line = lineNumber;
            //token1.position = i;
            //AddToken(token1);

            PrintTokensReforged(ref result);
        }
    }
}
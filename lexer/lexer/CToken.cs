using System;
using System.Collections.Generic;
using System.Text;

namespace lexer
{
    public struct Token
    {
        public TokenType type;
        public string value;
        public int line;
        public int position;
    }
}

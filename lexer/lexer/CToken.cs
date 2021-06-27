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
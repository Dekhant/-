using System;
using System.Collections.Generic;
using System.Text;

namespace LL1Generator
{
    public enum TokenType
    {
		Identifier,
		Integer,
		Float,
		Binary,
		Octal,
		Hexadecimal,
		Char,
		String,
		Array,
		Keyword,
		ArithmeticOperator,
		ComparisonOperator,
		LogicOperator,
		Bracket,
		Separator,
		Comment,
		Error,
		EoF,
		MultiCommStart,
		MultiCommEnd
	}
}

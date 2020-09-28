﻿namespace SP2.Tokens.Expressions
{
    internal class ParExpression : RvalueExpression
    {
        public Expression Expression;

        public override string ToString() => $"({Expression})";
    }
}
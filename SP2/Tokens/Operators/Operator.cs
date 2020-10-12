using System;

namespace SP2.Tokens.Operators
{
    internal abstract class Operator
    {
        public override string ToString()
        {
            return this switch
            {
                UnaryOperator uo => uo.ToString(),
                BinaryOperator bo => bo.ToString(),
                _ => throw new Exception()
            };
        }
    }

    internal abstract class UnaryOperator : Operator
    {
        public override string ToString()
        {
            return this switch
            {
                PrefixOperator po => po.ToString(),
                PostfixOperator po => po.ToString(),
                _ => throw new Exception()
            };
        }
    }

    internal abstract class PrefixOperator : UnaryOperator
    {
        public override string ToString()
        {
            return this switch
            {
                PrefixIncDecOperator pido => pido.ToString(),
                UnaryValOperator uvo => uvo.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    internal abstract class PostfixOperator : UnaryOperator
    {
        public override string ToString()
        {
            return this switch
            {
                PostfixIncDecOperator pido => pido.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    internal abstract class BinaryOperator : Operator
    {
        public override string ToString()
        {
            return this switch
            {
                AssOperator ao => ao.ToString(),
                BinaryBitOperator bbo => bbo.ToString(),
                BinaryCmpOperator bco => bco.ToString(),
                BinaryLogOperator blo => blo.ToString(),
                BinaryMemOperator bmo => bmo.ToString(),
                BinaryValOperator bvo => bvo.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
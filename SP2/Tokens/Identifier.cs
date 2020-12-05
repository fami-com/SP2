using Sprache;

namespace SP2.Tokens
{
    class Identifier : IPositionAware<Identifier>, IToken
    {
        public string Name;
        public Position StartPos;
        public int Length;

        public Identifier SetPos(Position startPos, int length)
        {
            StartPos = startPos;
            Length = length;
            return this;
        }

        public override string ToString() => Name;
    }
}
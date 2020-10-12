using System.Collections.Generic;
#pragma warning disable 8618

namespace SP2.Tokens
{
    internal class Program
    {
        public List<TopLevel> All;

        public override string ToString()
        {
            return string.Join("\n\n", All);
        }
    }
}
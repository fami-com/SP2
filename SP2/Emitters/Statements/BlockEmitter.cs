using System.Collections.Generic;
using System.Linq;
using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class BlockEmitter : Emitter
    {
        private readonly Block block;

        public readonly List<(string s, Type t)> Symbols;
        
        public BlockEmitter(Block block)
        {
            this.block = block;
            Symbols = new List<(string s, Type t)>();
        }

        public override void Emit()
        {
            foreach (var t in block.Statements.Select(s => new StatementEmitter(s)))
            {
                code.AddRange(t.CodeI);
                Symbols.AddRange(t.Symbols);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class BlockEmitter : Emitter
    {
        private readonly Block block;
        private int stm;

        
        public BlockEmitter(Block block)
        {
            this.block = block;
        }

        public override void Emit()
        {
            foreach (var t in block.Statements.Select(s => new StatementEmitter(s)))
            {
                code.AddRange(t.CodeI);
            }
        }
    }
}
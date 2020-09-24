using System.Collections.Generic;

namespace SP2.Emitters
{
    class BlockEmitter : Emitter
    {
        private readonly Block block;

        public BlockEmitter(Block block)
        {
            this.block = block;
            code = new List<string>();
        }

        public override void Emit()
        {
            foreach (var s in block.Statements)
            {
                code.AddRange(new StatementEmitter(s).CodeI);
            }
        }
    }
}
using System.Text.RegularExpressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class BlockEmitter : Emitter
    {
        private readonly Block _block;
        private static readonly Regex _rx = new Regex(@"#@([_A-Za-z][_A-Za-z0-9]*)");

        
        public BlockEmitter(Block block)
        {
            _block = block;
        }

        public override void Emit()
        {
            foreach (var t in _block.Statements)
            {
                foreach (var s in new StatementEmitter(t).CodeI)
                {
                    var m = _rx.Match(s);
                    if (!m.Success)
                    {
                        code.Add(s);
                        continue;
                    }

                    var g = m.Groups[1].ToString();
                    var y = _rx.Replace(s, $"#@{_block.BlockID}{g}");
                    code.Add(y);
                }
            }
        }
    }
}
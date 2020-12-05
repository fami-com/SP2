using System;
using SP2.Emitters.Expressions;
using SP2.Emitters.Statements;
using SP2.Tokens.Partial;

namespace SP2.Emitters
{
    class ForLoopInitEmitter:Emitter
    {
        private ForLoopInit _init;

        public ForLoopInitEmitter(ForLoopInit init)
        {
            _init = init;
        }

        public override void Emit()
        {
            if (_init.Init2 is null && !(_init.Init1 is null))
            {
                var vdae = new PartialVariableAssEmitter(_init.Init1);
                code.AddRange(vdae.CodeI);
            }
            else if (_init.Init1 is null && !(_init.Init2 is null))
            {
                var ee = new ExpressionEmitter(_init.Init2);
                code.AddRange(ee.CodeI);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
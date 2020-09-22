using System;
using System.Collections.Generic;
using SP2.Blocks;

namespace SP2.Emitters
{
    class TopLevelEmitter : Emitter
    {
        private readonly TopLevel top;

        public List<string> Prototypes;
        
        public TopLevelEmitter(TopLevel tl)
        {
            top = tl;
            Prototypes = new List<string>();
        }

        public override void Emit()
        {
            if (top is Function fn)
            {
                var fe = new FunctionEmitter(fn);
                fe.Emit();
                code = fe.Code;
                data = fe.Data;
                Prototypes.AddRange(fe.Prototypes);
            }
            else if (top is {} e)
                throw new Exception($"Unexpected TopLevel: {e}");
        }
    }
}
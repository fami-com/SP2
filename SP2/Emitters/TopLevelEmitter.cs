using System;
using System.Collections.Generic;
using SP2.Tokens;

namespace SP2.Emitters
{
    internal class TopLevelEmitter : Emitter
    {
        private readonly TopLevel top;

        public readonly List<string> Prototypes;
        
        public TopLevelEmitter(TopLevel tl)
        {
            top = tl;
            Prototypes = new List<string>();
        }

        public override void Emit()
        {
            switch (top)
            {
                case Function fn:
                {
                    var fe = new FunctionEmitter(fn);
                    fe.Emit();
                    code = fe.Code;
                    data = fe.Data;
                    Prototypes.AddRange(fe.Prototypes);
                    break;
                }
                case {} e:
                    throw new Exception($"Unexpected TopLevel: {e}");
            }
        }
    }
}
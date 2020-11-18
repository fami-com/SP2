using System.Collections.Generic;
using SP2.Tokens;

namespace SP2.Emitters
{
    class FunctionDefinitionEmitter : Emitter
    {
        private readonly FunctionDefinition _def;

        public FunctionDefinitionEmitter(FunctionDefinition def)
        {
            _def = def;
        }
        
        public override void Emit()
        {
            code = new List<string>();
        }
    }
}
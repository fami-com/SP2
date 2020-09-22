using System.Collections.Generic;
using SP2.Blocks;

namespace SP2.Emitters
{
    class FunctionEmitter : Emitter
    {
        private readonly Function function;

        public List<string> Prototypes;

        public FunctionEmitter(Function func)
        {
            function = func;
            Prototypes = new List<string> {$"{function.Definition.Name} PROTO"};
            code = new List<string> {$"{function.Definition.Name} PROC", "push ebp", "mov ebp, esp"};
        }

        public override void Emit()
        {
            code.AddRange(new BlockEmitter(function.Body).CodeI);
            code.Add("mov esp, ebp");
            code.Add("pop ebp");
            code.Add("ret");
            code.Add($"{function.Definition.Name} ENDP");
        }
    }
}
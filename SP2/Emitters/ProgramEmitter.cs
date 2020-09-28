using System.Collections.Generic;
using System.Linq;
using SP2.Tokens;

namespace SP2.Emitters
{
    internal class ProgramEmitter : Emitter
    {
        private readonly Program prog;

        //private List<string> data;

        private readonly List<string> prototypes;
        
        public override string Assembly =>
            $"\n{string.Join('\n', Code)}";

        public ProgramEmitter(Program input)
        {
            prog = input;
            //data = new List<string>();
            prototypes = new List<string>();
            code = new List<string>
            {
                ".386",
                ".model flat,stdcall",
                "option casemap:none"
            };
        }

        public override void Emit()
        {
            var emitters = CreateAndEvaluateEmitters();
            GetAllPrototypes(emitters);
            
            code.AddRange(prototypes);
            code.AddRange(new []{".code", "_start:", "invoke main", "ret"});
            
            foreach (var e in emitters)
            {
                code.AddRange(e.Code);
            }
            
            code.Add("END _start");
        }

        private List<TopLevelEmitter> CreateAndEvaluateEmitters()
        {
            var emitters = prog.All.Select(t => new TopLevelEmitter(t)).ToList();
            emitters.ForEach(e => e.Emit());
            return emitters;
        }

        private void GetAllPrototypes(List<TopLevelEmitter> topLevelEmitters)
        {
            topLevelEmitters.ForEach(e => prototypes.AddRange(e.Prototypes));
        }
    }
}
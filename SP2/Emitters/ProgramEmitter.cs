using System.Collections.Generic;
using System.Linq;
using SP2.Tokens;

namespace SP2.Emitters
{
    internal class ProgramEmitter : Emitter
    {
        private readonly Program _prog;

        //private List<string> data;

        private readonly List<string> _prototypes;
        
        public override string Assembly =>
            $"\n{string.Join('\n', Code)}";

        public ProgramEmitter(Program input)
        {
            _prog = input;
            //data = new List<string>();
            _prototypes = new List<string>();
            code = new List<string>
            {
                ".386",
                ".model flat,stdcall",
                "option casemap:none",
                @"include \masm32\include\windows.inc",
                @"include \masm32\macros\macros.asm",
                @"include \masm32\include\masm32.inc",
                @"include \masm32\include\user32.inc",
                @"include \masm32\include\kernel32.inc",
                @"includelib \masm32\lib\masm32.lib",
                @"includelib \masm32\lib\user32.lib",
                @"includelib \masm32\lib\kernel32.lib",
                ".data",
                "Cap db \"5-9-CSharp-IO-81-Ivanov\",0"
            };
        }

        public override void Emit()
        {
            var emitters = CreateAndEvaluateEmitters();
            GetAllPrototypes(emitters);
            
            code.AddRange(_prototypes);
            code.AddRange(new []{".code", "_start:", "invoke main", "invoke MessageBox,0,str$(eax),offset Cap,MB_OK", "ret"});
            
            foreach (var e in emitters)
            {
                code.AddRange(e.Code);
            }
            
            code.Add("END _start");
        }

        private List<TopLevelEmitter> CreateAndEvaluateEmitters()
        {
            var emitters = _prog.All.Select(t => new TopLevelEmitter(t)).ToList();
            emitters.ForEach(e => e.Emit());
            return emitters;
        }

        private void GetAllPrototypes(List<TopLevelEmitter> topLevelEmitters)
        {
            topLevelEmitters.ForEach(e => _prototypes.AddRange(e.Prototypes));
        }
    }
}
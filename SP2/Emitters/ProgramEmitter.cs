﻿using System.Collections.Generic;
using System.Linq;
using SP2.Blocks;

namespace SP2.Emitters
{
    class ProgramEmitter : Emitter
    {
        private readonly Program prog;

        //private List<string> data;

        private List<string> prototypes;
        
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
                "option casemap:none",
                @"include C:\masm32\include\windows.inc",
                @"include C:\masm32\include\kernel32.inc",
                @"include C:\masm32\include\masm32.inc",
                @"includelib C:\masm32\lib\kernel32.lib",
                @"includelib C:\masm32\lib\masm32.lib",
                
            };
        }

        public override void Emit()
        {
            var emitters = CreateAndEvaluateEmitters();
            GetAllPrototypes(emitters);
            
            code.AddRange(prototypes);
            code.AddRange(new []{".code", "_start:", "invoke main"});
            
            foreach (var e in emitters)
            {
                code.AddRange(e.Code);
            }
            
            code.Add("END _start");
        }

        private List<TopLevelEmitter> CreateAndEvaluateEmitters()
        {
            var emitters = prog.All.Select(t => new TopLevelEmitter(t)).ToList();
            emitters.ForEach(e=>e.Emit());
            return emitters;
        }

        private void GetAllPrototypes(List<TopLevelEmitter> topLevelEmitters)
        {
            topLevelEmitters.ForEach(e => prototypes.AddRange(e.Prototypes));
        }
    }
}
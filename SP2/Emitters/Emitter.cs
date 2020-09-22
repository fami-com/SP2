﻿using System.Collections.Generic;

namespace SP2.Emitters
{
    abstract class Emitter
    {
        protected List<string> code;
        protected List<string> data;
        
        public List<string> Code => code;
        public List<string> CodeI => EmitAndGetCode();
        
        public List<string> Data => data;
        public List<string> DataI => EmitAndGetData();

        public abstract void Emit();
        public virtual string Assembly => string.Join('\n', Code);
        public string AssemblyI => EmitAndGetAssembly();

        protected Emitter()
        {
            code = new List<string>();
        }
        
        private string EmitAndGetAssembly()
        {
            Emit();
            return Assembly;
        }
        
        private List<string> EmitAndGetCode()
        {
            Emit();
            return Code;
        }
        
        private List<string> EmitAndGetData()
        {
            Emit();
            return Code;
        }
    }
}
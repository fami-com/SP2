namespace SP2.Emitters.Statements
{
    class BreakEmitter:Emitter
    {
        public override void Emit()
        {
            code.Add("jmp FORE{0}");
        }
    }
}
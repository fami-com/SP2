namespace SP2.Emitters.Statements
{
    class ContinueEmitter:Emitter
    {
        public override void Emit()
        {
            code.Add("jmp FORI{0}");
        }
    }
}
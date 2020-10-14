using System.Collections.Generic;

namespace SP2.Emitters
{
    class DummyEmitter : Emitter
    {
        public override void Emit()
        {
            code = new List<string>();
        }
    }
}
﻿using System.Collections.Generic;

namespace SP2.Tokens
{
    class Program
    {
        public List<TopLevel> All;

        public override string ToString()
        {
            return string.Join("\n\n", All);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SP2.Tokens;

namespace SP2
{
class AstWriter
{
    private static int _indent = 0;
    private IToken _token;

    public AstWriter(IToken token)
    {
        _token = token;
    }

    public void WriteAst()
    {
        var str = string.Concat(Enumerable.Repeat("| ", _indent));
        if (_indent != 0)
        {
            Console.WriteLine(str + "+");
            Console.WriteLine(str + "|" + _token.GetType().Name);
        }
        else
        {
            Console.WriteLine(_token.GetType().Name);
        }
        _indent++;

        foreach (var prop in _token.GetType().GetFields())
        {
            var v = prop.GetValue(_token);
            if (v is IList l)
            {
                foreach (var q in l)
                {
                    if (!(q is IToken p)) break;
                    var w = new AstWriter(p);
                    w.WriteAst();
                }
            }
            else
            {
                if (!(prop.GetValue(_token) is IToken p)) continue;
                var k = new AstWriter(p);
                k.WriteAst();
            }
        }

        _indent--;
    }
    }
}
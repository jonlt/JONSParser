using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JONSParser
{
    /// <summary>
    /// The parser!
    /// </summary>
    public static class JSON
    {
        public static dynamic Parse(string str)
        {
            var lexer = new Lxer(str);
            var paser = new Parser(lexer);

            return parser.Parse();
        }
    }
}

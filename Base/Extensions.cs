using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Base
{
    static class Extensions
    {
        public static string Join(this IEnumerable<string> strings, string separator)
            => string.Join(separator, strings);
    }


}

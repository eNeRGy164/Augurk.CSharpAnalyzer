using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Augurk.CSharpAnalyzer
{
    public class ConsoleStrackTraceCollector : IStackTraceCollector
    {
        private string prefix = "";

        public void StepInto(IMethodSymbol method)
        {
            Console.ForegroundColor = method.IsOverride ? ConsoleColor.Green : ConsoleColor.White;
            Console.WriteLine($"{this.prefix}{method.ToString()}");
            prefix = prefix + "\t";
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void StepOut()
        {
            prefix = prefix.Substring(1);
        }

        public void StepOver(IMethodSymbol method)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{this.prefix}{method.ToString()}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

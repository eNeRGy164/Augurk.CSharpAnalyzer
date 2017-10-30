using System;
using Oakton;
using Augurk.CSharpAnalyzer.Commands;

namespace Augurk.CSharpAnalyzer
{
    class Program
    {
        static int Main(string[] args)
        {
            var result = CommandExecutor.ExecuteCommand<AnalyzeCommand>(args);
#if DEBUG
            Console.ReadLine();
#endif
            return result;
        }
    }
}

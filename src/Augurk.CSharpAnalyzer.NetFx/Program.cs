/*
 Copyright 2017, Augurk
 
 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at
 
 http://www.apache.org/licenses/LICENSE-2.0
 
 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
*/
using System;
using Oakton;
using Augurk.CSharpAnalyzer.Commands;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Augurk.CSharpAnalyzer
{
    static class Program
    {
        /// <summary>
        /// Main entry-point for the CSharpAnalyzer command line.
        /// </summary>
        /// <param name="args">Arguments passed to the command line.</param>
        /// <returns>Returns an error code, or 0 if everything was succesful.</returns>
        static async Task<int> Main(string[] args)
        {
            var executor = CommandExecutor.For(_ =>
            {
                // Find and apply all command classes discovered in the Core assembly
                _.RegisterCommands(typeof(AnalyzeCommand).GetTypeInfo().Assembly);
                _.ConfigureRun = run =>
                {
                    switch (run.Command)
                    {
                        case AnalyzeCommand analyze:
                            analyze.GetWorkspaceFunc = GetWorkspace;
                            break;
                        default:
                            break;
                    }
                };
            });

            int result = await executor.ExecuteAsync(args);
#if DEBUG
            Console.ReadLine();
#endif
            return result;
        }

        static async Task<Workspace> GetWorkspace(string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();
            await workspace.OpenSolutionAsync(solutionPath);
            return workspace;
        }
    }
}

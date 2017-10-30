Feature: Analyze a C# project with a single test project

Scenario: Analyze
Given 'Augurk.CSharpAnalyzer.Specifications' contains feature files
And those features describe 'Augurk.CSharpAnalyzer'
When an analysis is run
Then the first 1 lines of the reported return for 'Augurk.CSharpAnalyzer' are
| Kind | Expression/Signature |
| When | an analysis is run   |
#| EntryPoint     | Augurk.CSharpAnalyzer.AnalyzeCommand.Execute(Augurk.CSharpAnalyzer.Options.AnalyzeOptions) |
#| Public-Invoke  | Oakton.ConsoleWriter.Write(ConsoleColor, string)                                           |
#| Private-Invoke | Augurk.CSharpAnalyzer.Analyze(Augurk.CSharpAnalyzer.Options.AnalyzeOptions)                |

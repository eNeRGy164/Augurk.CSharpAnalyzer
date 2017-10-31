Feature: Analyze a C# project with a single test project

Scenario: Analyze
Given 'Augurk.CSharpAnalyzer.Specifications' contains feature files
And those features describe 'Augurk.CSharpAnalyzer'
When an analysis is run
Then the first 3 lines of the reported return for 'Augurk.CSharpAnalyzer' are
| Kind   | Expression/Signature                                                                                                       |
| When   | an analysis is run                                                                                                         |
#TODO Kind = Entrypoint
| Public | Augurk.CSharpAnalyzer.Commands.AnalyzeCommand.Analyze(Augurk.CSharpAnalyzer.Options.AnalyzeOptions), Augurk.CSharpAnalyzer |
| Public | Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create(), Microsoft.CodeAnalysis.Workspace.Desktop                         |

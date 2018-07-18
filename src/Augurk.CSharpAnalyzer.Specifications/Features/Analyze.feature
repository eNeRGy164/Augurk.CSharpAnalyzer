Feature: Analyze a C# project with a single test project

Scenario: When calls directly into a single entrypoint

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is invoked directly'
	| Kind   | Local | Expression/Signature                       |
	| When   |       | entrypoint is invoked directly             |
	| Public | true  | Cucumis.Gardener.PlantGherkin(), Cucumis   |
	| Public |       | System.Console.WriteLine(string), mscorlib |

Scenario: Entrypoint is surrounded by other invocations

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is surrounded by other invocations'
	| Kind   | Local | Expression/Signature                                                                       |
	| When   |       | entrypoint is surrounded by other invocations                                              |
	| Public |       | System.Console.WriteLine(string), mscorlib                                                 |
	| Public | true  | Cucumis.Gardener.PlantGherkin(), Cucumis                                                   |
	| Public |       | System.Console.WriteLine(string), mscorlib                                                 |
	| Public |       | Cucumis.Specifications.Support.ConsoleWriter.WriteDefaultMessage(), Cucumis.Specifications |
	| Public |       | System.Console.WriteLine(string), mscorlib                                                 |

Scenario: When invokes two seperate entrypoints

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When two separate entrypoints are invoked'
	| Kind   | Local | Expression/Signature                       |
	| When   |       | two separate entrypoints are invoked       |
	| Public | true  | Cucumis.Gardener.PlantGherkin(), Cucumis   |
	| Public |       | System.Console.WriteLine(string), mscorlib |
	| Public | true  | Cucumis.Gardener.WaterPlants(), Cucumis    |
	| Public |       | System.Console.WriteLine(string), mscorlib |

Scenario: When an asynchronous entrypoint is invoked

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an asynchronous entrypoint is invoked'
	| Kind   | Local | Expression/Signature                                              |
	| When   |       | an asynchronous entrypoint is invoked                             |
	| Public | true  | Cucumis.Plant.Procreate(), Cucumis                                |
	| Public |       | System.Threading.Thread.Sleep(int), mscorlib                      |
	| Public |       | System.Threading.Tasks.Task.GetAwaiter(), mscorlib                |
	| Public |       | System.Runtime.CompilerServices.TaskAwaiter.GetResult(), mscorlib |
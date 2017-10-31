Feature: Analyze Through Interfaces
	The C# analyzer will attempt to resolve the concrete type with an interface
	when it is clearly declared in code.

Scenario: entrypoint is an explicit interface implementation

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is an explicit interface implementation'
	| Kind   | Local | Expression/Signature                               |
	| When   |       | entrypoint is an explicit interface implementation |
	| Public | true  | Cucumis.Gardener.Plant(), Cucumis                  |
	| Public |       | System.Console.WriteLine(string), mscorlib         |

Scenario: entrypoint is an implicit interface implementation

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is an implicit interface implementation'
	| Kind   | Local | Expression/Signature                               |
	| When   |       | entrypoint is an implicit interface implementation |
	| Public | true  | Cucumis.Gardener.WaterPlants(), Cucumis            |
	| Public |       | System.Console.WriteLine(string), mscorlib         |

Scenario: entrypoint is invoked after invocation on interface

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is invoked after invocation on interface'
	| Kind   | Local | Expression/Signature                                                          |
	| When   |       | entrypoint is invoked after invocation on interface                           |
	| Public |       | Cucumis.Specifications.Support.MockedGardener.Plant(), Cucumis.Specifications |
	| Public | true  | Cucumis.Gardener.WaterPlants(), Cucumis                                       |
	| Public |       | System.Console.WriteLine(string), mscorlib                                    |
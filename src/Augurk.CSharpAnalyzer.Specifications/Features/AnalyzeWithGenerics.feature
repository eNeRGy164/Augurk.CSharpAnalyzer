Feature: Analyze Generics

Scenario: A generic method is invoked

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When a generic method is invoked'
	| Kind   | Local | Level | Expression/Signature                       |
	| When   |       | 0     | a generic method is invoked                |
	| Public | true  | 1     | Cucumis.Gardener.Harvest<T>(T), Cucumis    |
	| Public |       | 2     | System.Console.WriteLine(string), mscorlib |
	| Public | true  | 2     | Cucumis.Plant.Prune(), Cucumis             |
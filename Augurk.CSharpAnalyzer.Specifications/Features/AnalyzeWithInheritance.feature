Feature: Analyze With Inheritance

Scenario: entrypoint is invoked on inherited automation class
There should be no trace of the inherited class, as the method is not overriden

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is invoked on inherited automation class'
	| Kind   | Local | Expression/Signature                                |
	| When   |       | entrypoint is invoked on inherited automation class |
	| Public | true  | Cucumis.Gardener.PlantGherkin(), Cucumis            |
	| Public |       | System.Console.WriteLine(string), mscorlib          |

Scenario: entrypoint is invoked through an inherited automation class

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is invoked through an inherited automation class'
	| Kind     | Local | Expression/Signature                                                                              |
	| When     |       | entrypoint is invoked through an inherited automation class                                       |
	| Internal |       | Cucumis.Specifications.Support.InheritedGardener.PlantGherkinAndWaterIt(), Cucumis.Specifications |
	| Public   | true  | Cucumis.Gardener.PlantGherkin(), Cucumis                                                          |
	| Public   |       | System.Console.WriteLine(string), mscorlib                                                        |
	| Public   | true  | Cucumis.Gardener.WaterPlants(), Cucumis                                                           |
	| Public   |       | System.Console.WriteLine(string), mscorlib                                                        |
Feature: Analyze Local Method Calls

Scenario: a local method is called within the entrypoint

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When a local method is called within the entrypoint'
	| Kind      | Local | Expression/Signature                                     |
	| When      |       | a local method is called within the entrypoint           |
	| Public    | true  | Cucumis.Gherkin.OnWater(Cucumis.WaterEventArgs), Cucumis |
	| Protected | true  | Cucumis.Gherkin.Grow(), Cucumis                          |
	| Public    |       | System.Console.WriteLine(string), mscorlib               |

Scenario: an explicit base method is called within the entrypoint

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an explicit base method is called within the entrypoint'
	| Kind      | Local | Expression/Signature                                          |
	| When      |       | an explicit base method is called within the entrypoint       |
	| Public    | true  | Cucumis.PickyGherkin.OnWater(Cucumis.WaterEventArgs), Cucumis |
	| Protected | true  | Cucumis.Gherkin.Grow(), Cucumis                               |
	| Public    |       | System.Console.WriteLine(string), mscorlib                    |
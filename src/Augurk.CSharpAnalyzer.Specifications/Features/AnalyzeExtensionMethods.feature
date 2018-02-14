Feature: Analyze Extension Methods

Scenario: an extension method is invoked
	
	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an extension method is invoked'
	| Kind     | Local | Level | Expression/Signature                                        |
	| When     |       | 0     | an extension method is invoked                              |
	| Public   | true  | 1     | Cucumis.GherkinExtensions.Harvest(Cucumis.Gherkin), Cucumis |
	| Public   | true  | 2     | Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis   |
	| Public   |       | 3     | System.Console.WriteLine(string), mscorlib                  |
	| Internal | true  | 3     | Cucumis.Gherkin.CutVine(), Cucumis                          |
	| Public   |       | 4     | System.Console.WriteLine(string), mscorlib                  |

Scenario: an extension method is invoked on a derived type
	
	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an extension method is invoked on a derived type'
	| Kind     | Local | Level | Expression/Signature                                        |
	| When     |       | 0     | an extension method is invoked on a derived type            |
	| Public   | true  | 1     | Cucumis.GherkinExtensions.Harvest(Cucumis.Gherkin), Cucumis |
	| Public   | true  | 2     | Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis   |
	| Public   |       | 3     | System.Console.WriteLine(string), mscorlib                  |
	| Internal | true  | 3     | Cucumis.PickyGherkin.CutVine(), Cucumis                     |
	| Internal | true  | 4     | Cucumis.Gherkin.CutVine(), Cucumis                          |
	| Public   |       | 5     | System.Console.WriteLine(string), mscorlib                  |
	| Public   |       | 4     | System.Console.WriteLine(string), mscorlib                  |

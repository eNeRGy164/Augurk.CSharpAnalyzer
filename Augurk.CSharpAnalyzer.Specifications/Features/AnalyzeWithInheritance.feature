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

Scenario: same method is invoked with different concrete types

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When same method is invoked with different concrete types'
	| Kind     | Local | Level | Expression/Signature                                      |
	| When     |       | 0     | same method is invoked with different concrete types      |
	| Public   | true  | 1     | Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis |
	| Public   |       | 2     | System.Console.WriteLine(string), mscorlib                |
	| Internal | true  | 2     | Cucumis.Gherkin.CutVine(), Cucumis                        |
	| Public   |       | 3     | System.Console.WriteLine(string), mscorlib                |
	| Public   | true  | 1     | Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis |
	| Public   |       | 2     | System.Console.WriteLine(string), mscorlib                |
	| Internal | true  | 2     | Cucumis.PickyGherkin.CutVine(), Cucumis                   |
	| Internal | true  | 3     | Cucumis.Gherkin.CutVine(), Cucumis                        |
	| Public   |       | 4     | System.Console.WriteLine(string), mscorlib                |
	| Public   |       | 3     | System.Console.WriteLine(string), mscorlib                |

Scenario: an instance method is invoked from its base

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an instance method is invoked from its base'
	| Kind    | Local | Level | Expression/Signature                         |
	| When    |       | 0     | an instance method is invoked from its base  |
	| Public  | true  | 1     | Cucumis.Plant.Bloom(), Cucumis               |
	| Public  | true  | 2     | Cucumis.Melothria.Wither(), Cucumis          |
	| Private | true  | 3     | Cucumis.Melothria.Rot(), Cucumis             |
	| Public  |       | 4     | System.Console.WriteLine(string), mscorlib   |

Scenario: an inherited instance method is invoked indirectly

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When an inherited instance method is invoked indirectly'
	| Kind     | Local | Level | Expression/Signature                                                                                     |
	| When     |       | 0     | an inherited instance method is invoked indirectly                                                       |
	| Private  |       | 1     | Cucumis.Specifications.Steps.InheritanceSteps.PrepareAndCutVine(Cucumis.Gherkin), Cucumis.Specifications |
	| Private  |       | 2     | Cucumis.Specifications.Steps.InheritanceSteps.CutTheVine(Cucumis.Gherkin), Cucumis.Specifications        |
	| Internal | true  | 3     | Cucumis.PickyGherkin.CutVine(), Cucumis                                                                  |
	| Internal | true  | 4     | Cucumis.Gherkin.CutVine(), Cucumis                                                                       |
	| Public   |       | 5     | System.Console.WriteLine(string), mscorlib                                                               |
	| Public   |       | 4     | System.Console.WriteLine(string), mscorlib                                                               |
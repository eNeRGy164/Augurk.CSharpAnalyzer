Feature: Analyzing Anti-patterns
	This feature describes various anti-patterns that might be used in the wild which we do want to support, but do not recommend to use.

Scenario: the automated code cannot be invoked directly
It might happen that the automated code cannot be invoked directly from a when step due to complexity. Therefore we provide a means to annotate
the actual method being tested by a When step.

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When the automated code cannot be invoked directly'
	| Kind    | Local | Expression/Signature                          | AutomationTargets                       |
	| When    |       | the automated code cannot be invoked directly | ["Cucumis.Melothria.Wither(), Cucumis"] |
	| Public  | true  | Cucumis.Plant.FreezeAndThaw(), Cucumis        |                                         |
	| Public  | true  | Cucumis.Melothria.Wither(), Cucumis           |                                         |
	| Private | true  | Cucumis.Melothria.Rot(), Cucumis              |                                         |
	| Public  |       | System.Console.WriteLine(string), mscorlib    |                                         |

Scenario: only the top level overload should match

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When only the top level overload should match'
	| Kind     | Local | Expression/Signature                                                                   | AutomationTargets                                  |
	| When     |       | only the top level overload should match                                               | ["Cucumis.Gardener.Water(Cucumis.Plant), Cucumis"] |
	| Public   | true  | Cucumis.Plant.Water(Cucumis.Gardener), Cucumis                                         |                                                    |
	| Internal | true  | Cucumis.Gardener.Water(Cucumis.Plant), Cucumis                                         |                                                    |
	| Internal | true  | Cucumis.Gardener.Water(System.Collections.Generic.IEnumerable<Cucumis.Plant>), Cucumis |                                                    |
	| Public   |       | System.Console.WriteLine(string), mscorlib                                             |                                                    |

Scenario: only the lowest level overload should match

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When only the lowest level overload should match'
	| Kind     | Local | Expression/Signature                                                                   | AutomationTargets                                                                          |
	| When     |       | only the lowest level overload should match                                            | ["Cucumis.Gardener.Water(System.Collections.Generic.IEnumerable<Cucumis.Plant>), Cucumis"] |
	| Public   | true  | Cucumis.Plant.Water(Cucumis.Gardener), Cucumis                                         |                                                                                            |
	| Internal | true  | Cucumis.Gardener.Water(Cucumis.Plant), Cucumis                                         |                                                                                            |
	| Internal | true  | Cucumis.Gardener.Water(System.Collections.Generic.IEnumerable<Cucumis.Plant>), Cucumis |                                                                                            |
	| Public   |       | System.Console.WriteLine(string), mscorlib                                             |                                                                                            |


Scenario: all overloads should match

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When all overloads should match'
	| Kind     | Local | Expression/Signature                                                                   | AutomationTargets                                                                                                                           |
	| When     |       | all overloads should match                                                             | ["Cucumis.Gardener.Water(Cucumis.Plant), Cucumis","Cucumis.Gardener.Water(System.Collections.Generic.IEnumerable<Cucumis.Plant>), Cucumis"] |
	| Public   | true  | Cucumis.Plant.Water(Cucumis.Gardener), Cucumis                                         |                                                                                                                                             |
	| Internal | true  | Cucumis.Gardener.Water(Cucumis.Plant), Cucumis                                         |                                                                                                                                             |
	| Internal | true  | Cucumis.Gardener.Water(System.Collections.Generic.IEnumerable<Cucumis.Plant>), Cucumis |                                                                                                                                             |
	| Public   |       | System.Console.WriteLine(string), mscorlib                                             |                                                                                                                                             |
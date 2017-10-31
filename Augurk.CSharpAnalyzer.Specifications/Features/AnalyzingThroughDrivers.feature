Feature: Analyzing Through Drivers
	While designing your automation layer, the usage of drivers is highly recommended.
	As such, the C# Analyzer supports this pattern. However, there are some constraints
	to take into consideration...


Scenario: entrypoint is invoked through a driver directly
As long as the driver invokes the methodes on the testable class directly,
the results will be complete.

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is invoked through a driver directly'
	| Kind   | Local | Expression/Signature                                                                |
	| When   |       | entrypoint is invoked through a driver directly                                     |
	| Public |       | Cucumis.Specifications.Support.GardenerDriver.WaterPlants(), Cucumis.Specifications |
	| Public | true  | Cucumis.Gardener.WaterPlants(), Cucumis                                             |
	| Public |       | System.Console.WriteLine(string), mscorlib                                          |

Scenario: entrypoint is indirectly invoked through a driver
If the driver uses the testable class via an interface, 
the analyzer will stop at the invocation tot the interface.

	Given 'Cucumis.Specifications' contains feature files
	When an analysis is run
	Then the resulting report contains 'When entrypoint is indirectly invoked through a driver'
	| Kind   | Local | Expression/Signature                                                                          |
	| When   |       | entrypoint is indirectly invoked through a driver                                             |
	| Public |       | Cucumis.Specifications.Support.GardenerDriver.WaterPlantsIndirectly(), Cucumis.Specifications |
	| Public | true  | Cucumis.IGardener.WaterPlants(), Cucumis                                                      |
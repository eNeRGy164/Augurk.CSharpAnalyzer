# Augurk.CSharpAnalyzer

Augurk.CSharpAnalyzer is an extension to the Augurk ecosystem that will (eventually) allow users of Augurk to visualize the dependencies between their
features, even across their entire product portfolio. To do this, Augurk.CSharpAnalyzer uses Roslyn to do static code analysis and tries to figure out
what is being called by the when steps of a feature. All that information is then accumulated and sent to Augurk for analysis.

## Alpha feature
At the time of this writing an **alpha** version of Augurk.CSharpAnalyzer is available as a NuGet package. It includes the command line tool that will
perform the analysis as well as a simple annotations library that can help guide the analyzer to make the right choices.

## Usage
There are two steps to using Augurk.CSharpAnalyzer, analyze and upload:

### Analyze command
The basic usage of the analyze command is as follows:

```shell
augurk.csharpanalyzer analyze <solution> <specificationsproject>
```
- *solution* is either a full path or a relative path to a Visual Studio solution file
- *specificationsproject* is the name of the project that contains the feature files and must be included in the same solution

If the analysis is run succesfully a file called <solution>.aar is generated next to the solution file specified.

## Upload command
The basic usage of the publish command is as follows:

```shell
augurk.csharpanalyzer upload <url> <productname> <version> <solution>
```

- *url* is the URL to an instance of Augurk (must be at least version 2.6.0)
- *productname* is the name of the product as it appears in Augurk
- *version* is the version of the product that has been analyzed
- *solution* is the (absolute or relative) path to the solution file
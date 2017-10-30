using Augurk.CSharpAnalyzer.Collectors;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    public class AnalyzeContext
    {
        public IDictionary<Project, Lazy<Compilation>> Projects { get; private set; }
        public IStackTraceCollector Collector { get; private set; }
        public AnalyzeOptions Options { get; private set; }

        public AnalyzeContext(IDictionary<Project, Lazy<Compilation>> projects, IStackTraceCollector collector, AnalyzeOptions options)
        {
            this.Projects = projects;
            this.Collector = collector;
            this.Options = options;
        }
    }
}

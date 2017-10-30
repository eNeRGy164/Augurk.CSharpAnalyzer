using Oakton;

namespace Augurk.CSharpAnalyzer.Options
{
    /// <summary>
    /// Available options for the analyze command.
    /// </summary>
    public class AnalyzeOptions
    {
        [Description("Path to the solution to be analyzed")]
        public string Solution { get; set; }

        [Description("Name of the project that contains the feature files")]
        public string SpecificationsProject { get; set; }

        [Description("Name of the project that contains the actual implementation")]
        public string SystemUnderTest { get; set; }
    }
}

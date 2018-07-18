using System;

namespace Augurk.CSharpAnalyzer.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AutomationTargetAttribute : Attribute
    {
        public AutomationTargetAttribute(Type declaringType, string targetMethod, OverloadHandling overloadHandling = OverloadHandling.First)
        {
            DeclaringType = declaringType;
            TargetMethod = targetMethod;
            OverloadHandling = overloadHandling;
        }

        public Type DeclaringType { get; }
        public string TargetMethod { get; }
        public OverloadHandling OverloadHandling { get; }
    }
}

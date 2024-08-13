namespace SSDTCodeAnalysis {

    internal static class SSDTCodeAnalysisConstants
    {
        internal static class RuleConstants
        {

            public const string AvoidWaitForDelay_RuleName = "Avoid using WAITFOR DELAY";

            public const string AvoidWaitForDelay_ProblemDescription = "WAITFOR DELAY statement use detected, which should be avoided";

            public const string CategoryDesign = "Design";

            public const string CategoryPerformance = "Design";
        }
    }
}
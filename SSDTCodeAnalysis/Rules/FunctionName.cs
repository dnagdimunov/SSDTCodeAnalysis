using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid function name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid function name {0}, missing fn prefix."
    )]
    public sealed class InvalidFunctionName : BaseObjectNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1003";

        protected override string Prefix => "fn";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.ScalarFunction,
            ModelSchema.PartitionFunction,
            ModelSchema.TableValuedFunction
        };
    }
}
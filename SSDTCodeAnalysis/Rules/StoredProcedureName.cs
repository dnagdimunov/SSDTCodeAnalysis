using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;

[ExportCodeAnalysisRule(
        RuleId,
        "Invalid stored procedure name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid stored procedure name {0}, missing usp prefix."
    )]
    public sealed class InvalidProcedureName : BaseObjectNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1008";
        protected override string Prefix => "usp";
        protected override ModelTypeClass[] SupportedSchemas => new[] { ModelSchema.Procedure };
    }

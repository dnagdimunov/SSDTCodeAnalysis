using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;
using System.Collections.Generic;
using System.Globalization;

    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid view name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid view name {0}, missing vw prefix."
    )]
    public sealed class InvalidViewName : BaseObjectNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1007";

        protected override string Prefix => "vw";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.View
        };

        // Constructor
        public InvalidViewName()
        {
            SupportedElementTypes = new[] { ModelSchema.View };
        }
    }

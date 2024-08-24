
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;
using System.Collections.Generic;
using System.Globalization;

    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid default constraint name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid default constraint name {0}, missing name or DF_ prefix."
    )]
    public sealed class DefaultConstraintName : BaseConstraintNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR2002";

        protected override string Prefix => "DF_";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.DefaultConstraint
        };

        // Constructor
        public DefaultConstraintName()
        {
            SupportedElementTypes = new[] { ModelSchema.DefaultConstraint };
        }
    }


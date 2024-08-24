
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;
using System.Collections.Generic;
using System.Globalization;

    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid primary key constraint name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid primary key constraint name {0}, missing name or PF_ prefix."
    )]
    public sealed class PrimaryKeyConstraintName : BaseConstraintNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR2001";

        protected override string Prefix => "PK_";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.DefaultConstraint
        };

        // Constructor
        public PrimaryKeyConstraintName()
        {
            SupportedElementTypes = new[] { ModelSchema.PrimaryKeyConstraint };
        }
    }


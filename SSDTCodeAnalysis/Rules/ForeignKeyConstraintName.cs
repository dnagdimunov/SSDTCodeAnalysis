
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;
using System.Collections.Generic;
using System.Globalization;

    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid foreign key constraint name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid foreign key constraint name {0}, missing name or FK_ prefix."
    )]
    public sealed class ForeignKeyConstraintName : BaseConstraintNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR2003";

        protected override string Prefix => "FK_";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.DefaultConstraint
        };

        // Constructor
        public ForeignKeyConstraintName()
        {
            SupportedElementTypes = new[] { ModelSchema.ForeignKeyConstraint };
        }
    }

using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;
using System.Collections.Generic;
using System.Globalization;

    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid table name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid table name {0}, missing tbl prefix."
    )]
    public sealed class InvalidTableName : BaseObjectNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1002";

        protected override string Prefix => "tbl";

        protected override ModelTypeClass[] SupportedSchemas => new[]
        {
            ModelSchema.Table
        };

        // Constructor
        public InvalidTableName()
        {
            SupportedElementTypes = new[] { ModelSchema.Table };
        }
    }

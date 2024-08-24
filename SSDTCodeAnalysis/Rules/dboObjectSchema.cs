using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid object schema dbo",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid object schema dbo on object {0}"
    )]
    public sealed class InvalidObjectSchema : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1001";

        public InvalidObjectSchema()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.Table,
                ModelSchema.ScalarFunction,
                ModelSchema.View,
                ModelSchema.Procedure,
                ModelSchema.PartitionFunction
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);
            TSqlObject objectSchema = modelElement.GetParent(DacQueryScopes.SameDatabase);

            if ((objectSchema?.Name?.Parts?[0] ?? string.Empty) == "dbo")
            {
                SqlRuleProblem problem = new SqlRuleProblem(
                    string.Format(CultureInfo.CurrentCulture, ruleDescriptor.DisplayDescription, modelElement.Name),
                    modelElement
                );
                problems.Add(problem);
            }

            return problems;
        }
    }
}

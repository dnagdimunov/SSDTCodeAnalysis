using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid or missing default constraint name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid or missing default constraint name {0} on column {1}"
    )]
    public sealed class InvalidDefaultConstraintName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1004";

        public InvalidDefaultConstraintName()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.DefaultConstraint
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);
            var tableName = modelElement.GetReferenced()
                                        .First(t => t.ObjectType.Name == "Table")
                                        .Name
                                        .Parts[1];

            var column = modelElement.GetReferenced()
                                     .First(t => t.ObjectType.Name == "Column");

            if (modelElement.Name.Parts.Count == 0 ||
                !modelElement.Name.Parts[1].StartsWith(string.Format("DF_{0}", tableName)))
            {
                string columnName = string.Join(".", column.Name.Parts);
                SqlRuleProblem problem = new SqlRuleProblem(
                    string.Format(CultureInfo.CurrentCulture, ruleDescriptor.DisplayDescription, modelElement.Name, columnName),
                    modelElement
                );
                problems.Add(problem);
            }

            return problems;
        }
    }
}

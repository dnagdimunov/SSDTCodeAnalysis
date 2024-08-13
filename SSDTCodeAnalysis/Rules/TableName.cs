using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(RuleId,
    "Invalid table name",
    Category = "Naming",
    RuleScope = SqlRuleScope.Element
    ,Description = "Invalid Table name {0}, missing tbl prefix.")]
    public sealed class InvalidTableName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1002";

        public InvalidTableName ()
        {
            SupportedElementTypes =
            [
                ModelSchema.PrimaryKeyConstraint
            ];
        }

        public override IList<SqlRuleProblem> Analyze(
            SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = [];

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);  

            var tableName = modelElement.GetReferenced().First(t => t.ObjectType.Name == "Table").Name.Parts[1];

            if (! tableName.StartsWith("tbl"))
            {
                SqlRuleProblem problem = new(
                    String.Format(CultureInfo.CurrentCulture,
                        ruleDescriptor.DisplayDescription, modelElement.Name),
                    modelElement);
                problems.Add(problem);
            }
            
            return problems;
        }

    }
}

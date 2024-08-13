using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(RuleId,
    "Invalid or missing Primary Key Constraint Name",
    Category = "Naming",
    RuleScope = SqlRuleScope.Element
    ,Description = "Invalid or missing Primary Key Constraint name {0} on Table {1}")]
    public sealed class InvalidPrimaryKeyConstraintName : SqlCodeAnalysisRule
    {

        public const string RuleId = "SSDTCodeAnalysis.SR1003";

        public InvalidPrimaryKeyConstraintName()
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

            if ((modelElement.Name.Parts.Count == 0 ? "" : modelElement.Name.Parts[1]) != String.Format("PK_{0}", tableName))
            {
                SqlRuleProblem problem = new(
                    String.Format(CultureInfo.CurrentCulture,
                        ruleDescriptor.DisplayDescription, modelElement.Name, tableName),
                    modelElement);
                problems.Add(problem);
            }
            
            return problems;
        }

    }
}

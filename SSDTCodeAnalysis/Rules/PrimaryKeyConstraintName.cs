using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid or missing Primary Key Constraint Name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid or missing Primary Key Constraint name {0} on Table {1}"
    )]
    public sealed class InvalidPrimaryKeyConstraintName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1003";

        public InvalidPrimaryKeyConstraintName()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.PrimaryKeyConstraint
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);
            var tableReference = modelElement.GetReferenced()
                                             .FirstOrDefault(t => t.ObjectType.Name == "Table");

            if (tableReference != null)
            {
                string tableName = tableReference.Name.Parts[1];

                string constraintName = modelElement.Name.Parts.Count > 1 
                    ? modelElement.Name.Parts[1] 
                    : string.Empty;

                string expectedConstraintName = string.Format("PK_{0}", tableName);

                if (constraintName != expectedConstraintName)
                {
                    SqlRuleProblem problem = new SqlRuleProblem(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            ruleDescriptor.DisplayDescription,
                            modelElement.Name,
                            tableName
                        ),
                        modelElement
                    );
                    problems.Add(problem);
                }
            }

            return problems;
        }
    }
}

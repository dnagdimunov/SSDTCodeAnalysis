using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid table name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid Table name {0}, missing tbl prefix."
    )]
    public sealed class InvalidTableName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1002";

        public InvalidTableName()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.Table
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);

            var tableReference = modelElement.GetReferenced().FirstOrDefault(t => t.ObjectType == ModelSchema.Table);
            
            if (tableReference != null)
            {
                string tableName = tableReference.Name.Parts.Count > 1 ? tableReference.Name.Parts[1] : string.Empty;

                if (!tableName.StartsWith("tbl"))
                {
                    SqlRuleProblem problem = new SqlRuleProblem(
                        string.Format(CultureInfo.CurrentCulture, ruleDescriptor.DisplayDescription, modelElement.Name),
                        modelElement
                    );
                    problems.Add(problem);
                }
            }
            
            return problems;
        }
    }
}

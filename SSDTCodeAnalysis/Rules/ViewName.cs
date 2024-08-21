using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid view name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid view name {0}, missing vw prefix."
    )]
    public sealed class InvalidViewName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1007";

        public InvalidViewName()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.View
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);

            var viewReference = modelElement.GetReferenced().First();
            
            if (viewReference != null)
            {
                string viewName = viewReference.Name.Parts.Count > 1 ? viewReference.Name.Parts[1] : string.Empty;

                if (!viewName.StartsWith("vw"))
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

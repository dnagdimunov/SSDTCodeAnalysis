using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRule(
        RuleId,
        "Invalid function name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid function name {0}, missing fn prefix."
    )]
    public sealed class InvalidFunctionName : SqlCodeAnalysisRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1006";

        public InvalidFunctionName()
        {
            SupportedElementTypes = new[]
            {
                ModelSchema.ScalarFunction,
                ModelSchema.PartitionFunction,
                ModelSchema.TableValuedFunction
            };
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            Console.WriteLine("Checking Function");
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);
            var functionReference = modelElement.GetReferenced().First();

            if (functionReference != null)
            {
                string functionName = functionReference.Name.Parts.Count > 1 ? functionReference.Name.Parts[1] : string.Empty;

                if (!functionName.StartsWith("fn"))
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

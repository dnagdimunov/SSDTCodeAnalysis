using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;
using System.Globalization;

namespace SSDTCodeAnalysis.Rules
{
    public abstract class BaseObjectNameRule : SqlCodeAnalysisRule
    {
        protected abstract string Prefix { get; }
        protected abstract ModelTypeClass[] SupportedSchemas { get; }

        public BaseObjectNameRule()
        {
            // Set the supported element types using a valid non-static array assignment
            SupportedElementTypes = SupportedSchemas;
        }

        public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);
            var reference = modelElement.GetReferenced().FirstOrDefault();

            if (reference != null)
            {
                string name = reference.Name.Parts.Count > 1 ? reference.Name.Parts[1] : string.Empty;

                if (!name.StartsWith(Prefix))
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
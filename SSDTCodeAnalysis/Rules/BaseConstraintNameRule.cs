using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis;
using System.Collections.Generic;
using System.Globalization;

    public abstract class BaseConstraintNameRule : SqlCodeAnalysisRule
    {
        protected abstract string Prefix { get; }
        protected abstract ModelTypeClass[] SupportedSchemas { get; }

        public BaseConstraintNameRule()
        {
            SupportedElementTypes = SupportedSchemas;
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
                !modelElement.Name.Parts[1].StartsWith(string.Format(Prefix + "{0}", tableName)))
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
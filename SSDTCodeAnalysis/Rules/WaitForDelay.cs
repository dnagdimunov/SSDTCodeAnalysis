using System.Globalization;
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

namespace SSDTCodeAnalysis.Rules
{

    [ExportCodeAnalysisRule(RuleId,
    RuleConstants.AvoidWaitForDelay_RuleName,
    
    Category = RuleConstants.CategoryPerformance,
    RuleScope = SqlRuleScope.Element
    ,Description = RuleConstants.AvoidWaitForDelay_ProblemDescription)]
    public sealed class WaitForDelay : SqlCodeAnalysisRule
    {
        /// <summary>  
        /// The Rule ID should resemble a fully-qualified class name. In the Visual Studio UI  
        /// rules are grouped by "Namespace + Category", and each rule is shown using "Short ID: DisplayName".  
        /// For this rule, that means the grouping will be "Public.Dac.Samples.Performance", with the rule  
        /// shown as "SR1004: Avoid using WaitFor Delay statements in stored procedures, functions and triggers."  
        /// </summary>  
        public const string RuleId = "SSDTBuild.SR1001";

        public WaitForDelay()
        {
            // This rule supports Procedures, Functions and Triggers. Only those objects will be passed to the Analyze method  
            SupportedElementTypes =
            [  
                // Note: can use the ModelSchema definitions, or access the TypeClass for any of these types  
                ModelSchema.ExtendedProcedure,
                ModelSchema.Procedure,
                ModelSchema.TableValuedFunction,
                ModelSchema.ScalarFunction,
                ModelSchema.DatabaseDdlTrigger,
                ModelSchema.DmlTrigger,
                ModelSchema.ServerDdlTrigger
            ];
        }

        /// <summary>  
        /// For element-scoped rules the Analyze method is executed once for every matching   
        /// object in the model.   
        /// </summary>  
        /// <param name="ruleExecutionContext">The context object contains the TSqlObject being   
        /// analyzed, a TSqlFragment  
        /// that's the AST representation of the object, the current rule's descriptor, and a   
        /// reference to the model being  
        /// analyzed.  
        /// </param>  
        /// <returns>A list of problems should be returned. These will be displayed in the Visual   
        /// Studio error list</returns>  
        public override IList<SqlRuleProblem> Analyze(
            SqlRuleExecutionContext ruleExecutionContext)
        {
            Console.WriteLine("Something new");
            IList<SqlRuleProblem> problems = [];

            TSqlObject modelElement = ruleExecutionContext.ModelElement;

            // this rule does not apply to inline table-valued function  
            // we simply do not return any problem in that case.  
            if (IsInlineTableValuedFunction(modelElement))
            {
                return problems;
            }

            string elementName = GetElementName(ruleExecutionContext, modelElement);

            // The rule execution context has all the objects we'll need, including the   
            // fragment representing the object,  
            // and a descriptor that lets us access rule metadata  
            TSqlFragment fragment = ruleExecutionContext.ScriptFragment;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            // To process the fragment and identify WAITFOR DELAY statements we will use a   
            // visitor   
            WaitForDelayVisitor visitor = new();
            fragment.Accept(visitor);
            IList<WaitForStatement> waitforDelayStatements = visitor.WaitForDelayStatements;

            // Create problems for each WAITFOR DELAY statement found   
            // When creating a rule problem, always include the TSqlObject being analyzed. This   
            // is used to determine  
            // the name of the source this problem was found in and a best guess as to the   
            // line/column the problem was found at.  
            //  
            // In addition if you have a specific TSqlFragment that is related to the problem   
            //also include this  
            // since the most accurate source position information (start line and column) will   
            // be read from the fragment  
            foreach (WaitForStatement waitForStatement in waitforDelayStatements)
            {
                SqlRuleProblem problem = new(
                    String.Format(CultureInfo.CurrentCulture,
                        ruleDescriptor.DisplayDescription, elementName),
                    modelElement,
                    waitForStatement);
                problems.Add(problem);
            }
            return problems;
        }

        private static string GetElementName(
            SqlRuleExecutionContext ruleExecutionContext,
            TSqlObject modelElement)
        {
            // Get the element name using the built in DisplayServices. This provides a number of   
            // useful formatting options to  
            // make a name user-readable  
            var displayServices = ruleExecutionContext.SchemaModel.DisplayServices;
            string elementName = displayServices.GetElementName(
                modelElement, ElementNameStyle.EscapedFullyQualifiedName);
            return elementName;
        }

        private static bool IsInlineTableValuedFunction(TSqlObject modelElement)
        {
            return TableValuedFunction.TypeClass.Equals(modelElement.ObjectType)
                           && FunctionType.InlineTableValuedFunction ==
                modelElement.GetMetadata<FunctionType>(TableValuedFunction.FunctionType);
        }


    }

}
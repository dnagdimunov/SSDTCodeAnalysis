using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using SSDTCodeAnalysis.Rules;

[ExportCodeAnalysisRule(
        RuleId,
        "Invalid stored procedure name",
        Category = "Naming",
        RuleScope = SqlRuleScope.Element,
        Description = "Invalid stored procedure name {0}, missing usp prefix."
    )]
    public sealed class InvalidProcedureName : BaseObjectNameRule
    {
        public const string RuleId = "SSDTCodeAnalysis.SR1008";
        protected override string Prefix => "usp";
        protected override ModelTypeClass[] SupportedSchemas => new[] { ModelSchema.Procedure };
    }
    
// using Microsoft.SqlServer.Dac.CodeAnalysis;
// using Microsoft.SqlServer.Dac.Model;
// using System.Collections.Generic;
// using System.Globalization;
// using static SSDTCodeAnalysis.SSDTCodeAnalysisConstants;

// namespace SSDTCodeAnalysis.Rules
// {
//     [ExportCodeAnalysisRule(
//         RuleId,
//         "Invalid stored procedure name",
//         Category = "Naming",
//         RuleScope = SqlRuleScope.Element,
//         Description = "Invalid stored procedure name {0}, missing vw prefix."
//     )]
//     public sealed class InvalidProcedureName : SqlCodeAnalysisRule
//     {
//         public const string RuleId = "SSDTCodeAnalysis.SR1008";

//         public InvalidProcedureName()
//         {
//             SupportedElementTypes = new[]
//             {
//                 ModelSchema.Procedure
//             };
//         }

//         public override IList<SqlRuleProblem> Analyze(SqlRuleExecutionContext ruleExecutionContext)
//         {
//             IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

//             TSqlObject modelElement = ruleExecutionContext.ModelElement;
//             RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

//             string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);

//             var procedureReference = modelElement.GetReferenced().First();
            
//             if (procedureReference != null)
//             {
//                 string viewName = procedureReference.Name.Parts.Count > 1 ? procedureReference.Name.Parts[1] : string.Empty;

//                 if (!viewName.StartsWith("vw"))
//                 {
//                     SqlRuleProblem problem = new SqlRuleProblem(
//                         string.Format(CultureInfo.CurrentCulture, ruleDescriptor.DisplayDescription, modelElement.Name),
//                         modelElement
//                     );
//                     problems.Add(problem);
//                 }
//             }
            
//             return problems;
//         }
//     }
// }

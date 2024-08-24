using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
using System.Globalization;


namespace SSDTCodeAnalysis.Rules
{
    [ExportCodeAnalysisRuleAttribute(RuleId,
    "Object on Primary filegroup",
    
    Category = "Schema",
    RuleScope = SqlRuleScope.Element
    ,Description = "{0} is on Primary Filegroup")]
    public sealed class ObjectOnPrimaryFileGroup : SqlCodeAnalysisRule
    {

        public const string RuleId = "SSDTCodeAnalysis.SR1002";

        public ObjectOnPrimaryFileGroup()
        {
            SupportedElementTypes = new[]
            {  
                ModelSchema.Index,
                ModelSchema.PrimaryKeyConstraint,
                ModelSchema.Table
            };
        }

        public override IList<SqlRuleProblem> Analyze(
            SqlRuleExecutionContext ruleExecutionContext)
        {
            IList<SqlRuleProblem> problems = new List<SqlRuleProblem>();

            TSqlObject modelElement = ruleExecutionContext.ModelElement;
            RuleDescriptor ruleDescriptor = ruleExecutionContext.RuleDescriptor;

            string elementName = Helper.GetElementName(ruleExecutionContext, modelElement);

            var fg = modelElement.GetReferencedRelationshipInstances().Where(t => t.Object.ObjectType.Name == "Filegroup").FirstOrDefault();
            var fgname = "";


            fgname = fg != null ? fg.ObjectName.Parts[0].ToString().ToUpper() : "Primary".ToUpper(); //if filegroup found get it's name otherwise set to primary
            

            if (fg == null && modelElement.ObjectType.Name == "Table") //if filegroup is not found and object type is table check if clustered index present
            {
                var indexes = modelElement.GetReferencingRelationshipInstances().Where(t => t.Object.ObjectType.Name == "Index" || t.FromObject.ObjectType.Name == "PrimaryKeyConstraint").Select(o => o.FromObject);
                if (indexes != null)
                {
                    var ci = indexes.Where(i => i.GetProperty<bool>(i.ObjectType.Name == "PrimaryKeyConstraint" ? PrimaryKeyConstraint.Clustered : Microsoft.SqlServer.Dac.Model.Index.Clustered) == true);
                    if (ci != null) //if ci index or clustered pk is found ignore filegroup name check, value will be checked against pk or ci index
                    {
                        fgname = "";
                    }
                }

            }
            
            if (fgname == "Primary".ToString().ToUpper())
            {
                SqlRuleProblem problem = new SqlRuleProblem(
                    String.Format(CultureInfo.CurrentCulture,
                        String.Format(ruleDescriptor.DisplayDescription, elementName),
                        elementName),
                    modelElement);
                problems.Add(problem);
            }

            return problems;
        }





    }
}

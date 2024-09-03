using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.CodeAnalysis;
using Microsoft.SqlServer.Dac.Model;
namespace SSDTCodeAnalysis.Tests;

[TestClass]
public class SSDTCodeAnalysisTests
{
    [TestMethod]
    public void Analyze_ShouldNotReturnProblem_WhenProcedureNameStartsWithUsp()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE PROCEDURE uspTest AS Select [col]=1;");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            var ruleSettings = new CodeAnalysisRuleSettings()
            {
                new RuleConfiguration("SSDTCodeAnalysis.SR1004")
            };

            ruleSettings.DisableRulesNotInSettings = true;

            CodeAnalysisService service = factory.CreateAnalysisService(model.Version, new CodeAnalysisServiceSettings()
            {
                RuleSettings = ruleSettings
            });

            CodeAnalysisResult analysisResults = service.Analyze(model);

            Assert.IsTrue(analysisResults.Problems.Count == 0, "Expected no problems to be reported, but some were found.");
        }
    }

    [TestMethod]
    public void Analyze_ShouldReturnProblem_WhenProcedureNameDoesNotStartWithUsp()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE PROCEDURE procTest AS Select [col]=1;");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
                        var ruleSettings = new CodeAnalysisRuleSettings()
                    {
                        new RuleConfiguration("SSDTCodeAnalysis.SR1004")
                    };
            ruleSettings.DisableRulesNotInSettings = true;

            CodeAnalysisService service = factory.CreateAnalysisService(model.Version, new CodeAnalysisServiceSettings()
            {
                RuleSettings = ruleSettings
            });

            CodeAnalysisResult analysisResults = service.Analyze(model);

            Assert.IsTrue(analysisResults.Problems.Any(), "Expected problems to be reported, but none were found.");
           
        }
    }

    [TestMethod]
    public void Analyze_ShouldReturnProblem_WhenViewNameDoesNotStartWithvw()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE VIEW someView AS Select columnname =1;");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
                        var ruleSettings = new CodeAnalysisRuleSettings()
                    {
                        new RuleConfiguration("SSDTCodeAnalysis.SR1006")
                    };
            ruleSettings.DisableRulesNotInSettings = true;

            CodeAnalysisService service = factory.CreateAnalysisService(model.Version, new CodeAnalysisServiceSettings()
            {
                RuleSettings = ruleSettings
            });

            CodeAnalysisResult analysisResults = service.Analyze(model);

            Assert.IsTrue(analysisResults.Problems.Any(), "Expected problems to be reported, but none were found.");
           
        }
    }
    [TestMethod]
    public void Analyze_ShouldNotReturnProblem_WhenProcedureNameStartsWithvw()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE VIEW vwSomeView AS Select [col]=1;");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            var ruleSettings = new CodeAnalysisRuleSettings()
            {
                new RuleConfiguration("SSDTCodeAnalysis.SR1006")
            };

            ruleSettings.DisableRulesNotInSettings = true;

            CodeAnalysisService service = factory.CreateAnalysisService(model.Version, new CodeAnalysisServiceSettings()
            {
                RuleSettings = ruleSettings
            });

            CodeAnalysisResult analysisResults = service.Analyze(model);

            Assert.IsTrue(analysisResults.Problems.Count == 0, "Expected no problems to be reported, but some were found.");
        }
    }

}
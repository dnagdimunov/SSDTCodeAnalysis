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
    public void Analyze_ShouldNotReturnProblem_WhenViewNameStartsWithvw()
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


    [TestMethod]
    public void Analyze_ShouldReturnProblem_WhenFunctionNameDoesNotStartWithfn()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects(@"CREATE FUNCTION [dbo].[invalidFunctionName]
                (@param1 int = 0)
                RETURNS INT
                AS
                BEGIN
                    RETURN @param1;
                END
                GO");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
                        var ruleSettings = new CodeAnalysisRuleSettings()
                    {
                        new RuleConfiguration("SSDTCodeAnalysis.SR1003")
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
    public void Analyze_ShouldNotReturnProblem_WhenFunctionNameStartsWithfn()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects(@"CREATE FUNCTION [dbo].[fnFunctionName]
                (@param1 int = 0)
                RETURNS INT
                AS
                BEGIN
                    RETURN @param1;
                END
                GO");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            var ruleSettings = new CodeAnalysisRuleSettings()
            {
                new RuleConfiguration("SSDTCodeAnalysis.SR1003")
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
    public void Analyze_ShouldReturnProblem_WhenTableNameDoesNotStartWithtbl()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE TABLE invalidTableName (ID int);");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
                        var ruleSettings = new CodeAnalysisRuleSettings()
                    {
                        new RuleConfiguration("SSDTCodeAnalysis.SR1005")
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
    public void Analyze_ShouldNotReturnProblem_WhenTableNameStartsWithtbl()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE TABLE tblUnitTest (ID int)");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            var ruleSettings = new CodeAnalysisRuleSettings()
            {
                new RuleConfiguration("SSDTCodeAnalysis.SR1005")
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
    public void Analyze_ShouldReturnProblem_WhenDefaultConstraintNameDoesNotStartWithDF()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE TABLE invalidTableName (ID int DEFAULT (0));");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
                        var ruleSettings = new CodeAnalysisRuleSettings()
                    {
                        new RuleConfiguration("SSDTCodeAnalysis.SR2002")
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
    public void Analyze_ShouldNotReturnProblem_WhenDefaultConstraintNameStartsWithDF()
    {
        using (var model = new TSqlModel(SqlServerVersion.Sql130, new TSqlModelOptions()))
        {
            model.AddObjects("CREATE TABLE tblUnitTest (ID int CONSTRAINT DF_tblUnitTest_ID DEFAULT (0))");

            CodeAnalysisServiceFactory factory = new CodeAnalysisServiceFactory();
            var ruleSettings = new CodeAnalysisRuleSettings()
            {
                new RuleConfiguration("SSDTCodeAnalysis.SR2002")
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
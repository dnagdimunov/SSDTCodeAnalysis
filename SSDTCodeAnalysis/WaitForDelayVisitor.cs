
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTCodeAnalysis
{
    internal class WaitForDelayVisitor : TSqlConcreteFragmentVisitor {

        public IList<WaitForStatement> WaitForDelayStatements { get; private set; }
        public WaitForDelayVisitor()
        {
             WaitForDelayStatements = [];
        }

        public override void ExplicitVisit(WaitForStatement node)
        {
            // We are only interested in WAITFOR DELAY occurrences  
            if (node.WaitForOption == WaitForOption.Delay)
                WaitForDelayStatements.Add(node);
        }

    }
}

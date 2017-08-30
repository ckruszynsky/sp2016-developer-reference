using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace Wingtip.WF.ConsoleSample
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity vacationWorkflow = new VacationWorkflow();
            WorkflowInvoker.Invoke(vacationWorkflow);
        }
    }
}

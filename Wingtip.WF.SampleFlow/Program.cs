using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Threading;

namespace Wingtip.WF.SampleFlow
{

    class Program
    {
        static void Main(string[] args)
        {
            //StartWorkflowViaApplication();
            StartWorkflowViaInvoker();
        }

        private static void StartWorkflowViaApplication()
        {
            ManualResetEvent rst = new ManualResetEvent(false);

            Dictionary<String, Object> arguments = new Dictionary<string, object>();
            //arguments.Add("TargetCountry", "Italy");

            Activity instance = new Workflow1();
            WorkflowApplication application = new WorkflowApplication(instance, arguments);

            application.Completed = (WorkflowApplicationCompletedEventArgs e) =>
            {
                rst.Set();
            };

            application.Idle = (WorkflowApplicationIdleEventArgs e) =>
            {
                Console.WriteLine("Workflow Idle");
            };

            application.Run();
            rst.WaitOne();
        }

        //private static void StartWorkflowViaApplicationWithPersistence()
        //{
        //    ManualResetEvent rst = new ManualResetEvent(false);

        //    Activity instance = new VacationWorkflow();
        //    WorkflowApplication application = new WorkflowApplication(instance);

        //    SqlWorkflowInstanceStore store =
        //        new SqlWorkflowInstanceStore("Server=WingtipServer;Initial Catalog=WF45_PersistenceStorage;Integrated Security=SSPI;Application Name=WF45");
        //    application.InstanceStore = store;

        //    application.Completed = (WorkflowApplicationCompletedEventArgs e) =>
        //    {
        //        rst.Set();
        //    };

        //    application.Idle = (WorkflowApplicationIdleEventArgs e) =>
        //    {
        //        Console.WriteLine("Workflow Idle");
        //    };

        //    application.PersistableIdle = (WorkflowApplicationIdleEventArgs e) =>
        //    {
        //        Console.WriteLine("Workflow PersistableIdle");
        //        return (PersistableIdleAction.Persist);
        //    };

        //    application.Run();
        //    rst.WaitOne();
        //}


        private static void StartWorkflowViaInvoker()
        {
            Dictionary<String, Object> arguments = new Dictionary<string, object>();
           arguments.Add("TargetCountry", "Italy");

            Activity instance = new Workflow1();
            WorkflowInvoker.Invoke(instance, arguments);
        }
    }
}

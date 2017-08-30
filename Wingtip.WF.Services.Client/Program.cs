using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wingtip.WF.Services.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args[0] == null)
            {
                throw new ArgumentException("You must pass what process you want to run.");
            }

            if(args[1] == null)
            {
                throw new ArgumentException("You must pass the url to run the process against");
            }


            var processToRun = args[0];
            var url = args[1];

            Console.WriteLine("Please enter the title of the list to check the workflow subscriptions for: ");
            var listTitle = Console.ReadLine();


            switch (processToRun.ToLower())
            {
                case "subscriptions":
                    EnumerateWorkflowSubscriptions(url,listTitle);
                    break;
                case "terminate":
                    TerminateWorkflows(url,listTitle);
                    break;
                case "publish":
                    Console.WriteLine("Please enter the path to the workflow to publish");
                    var xmlPath = Console.ReadLine();
                    String xaml = null;
                    using (FileStream fs = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            xaml = sr.ReadToEnd();
                        }
                    }
                    PublishXamlWorkflowToWorkflowStore(url, listTitle, xaml);
                    break;
            }
        }

        private static void PublishXamlWorkflowToWorkflowStore(string url,string title,string xaml)
        {
            ClientContext ctx = new ClientContext(url);
            Web web = ctx.Web;
            ctx.Load(web);
            List documents = web.Lists.GetByTitle(title);
            ctx.Load(documents);
            ctx.ExecuteQuery();

            WorkflowServicesManager wfManager = new WorkflowServicesManager(ctx, web);
            WorkflowDeploymentService deploymentService = wfManager.GetWorkflowDeploymentService();

            var validationResult = deploymentService.ValidateActivity(xaml);
            ctx.ExecuteQuery();

            WorkflowDefinition definition = new WorkflowDefinition(ctx);
            definition.Xaml = xaml;
            definition.DisplayName = "Sample XAML based Workflow";
            definition.Description = "Workflow saved by code";
            deploymentService.SaveDefinition(definition);
            ctx.ExecuteQuery();
        }


        private static void EnumerateWorkflowSubscriptions(string url, string listTitle)
        {
            ClientContext ctx = new ClientContext(url);
            Web web = ctx.Web;
            ctx.Load(web);
          
            List documents = web.Lists.GetByTitle(listTitle);
            ctx.Load(documents);
            ctx.ExecuteQuery();

            WorkflowServicesManager wfManager = new WorkflowServicesManager(ctx, web);
            WorkflowSubscriptionService subscriptionService =
                wfManager.GetWorkflowSubscriptionService();
            ctx.Load(subscriptionService);
            WorkflowSubscriptionCollection subscriptions =
                subscriptionService.EnumerateSubscriptionsByList(documents.Id);

            ctx.Load(subscriptions);
            ctx.ExecuteQuery();

            foreach (var s in subscriptions)
            {
                Console.WriteLine("******************************************");
                Console.WriteLine("Id: {0}", s.Id);
                Console.WriteLine("Name: {0}", s.Name);
                Console.WriteLine("DefinitionId: {0}", s.DefinitionId);
                Console.WriteLine("Enabled: {0}", s.Enabled);
                //Console.WriteLine("EventSourceId: {0}", s.EventSourceId);
                //Console.WriteLine("EventTypes");
                //foreach (var e in s.EventTypes)
                //{
                //    Console.WriteLine("EventType: {0}", e);
                //}
                //Console.WriteLine("ManualStartBypassesActivationLimit: {0}", s.ManualStartBypassesActivationLimit);
                //Console.WriteLine("PropertyDefinitions");
                //foreach (var p in s.PropertyDefinitions)
                //{
                //    Console.WriteLine("Property: {0} - Value: {1}", p.Key, p.Value);
                //}
                Console.WriteLine("StatusFieldName: {0}", s.StatusFieldName);
                Console.ReadLine();
            }
        }


        private static void TerminateWorkflows(string url, string listTitle)
        {
            ClientContext ctx = new ClientContext(url);
            Web web = ctx.Web;
            ctx.Load(web);
            List documents = web.Lists.GetByTitle(listTitle);
            ctx.Load(documents);
            ctx.ExecuteQuery();

            WorkflowServicesManager wfManager = new WorkflowServicesManager(ctx, web);
            WorkflowInstanceService instanceService = wfManager.GetWorkflowInstanceService();

            ListItemCollection items = documents.GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(items);
            ctx.ExecuteQuery();

            foreach (ListItem item in items)
            {
                WorkflowInstanceCollection instances = instanceService.EnumerateInstancesForListItem(documents.Id, item.Id);
                ctx.Load(instances);
                ctx.ExecuteQuery();

                foreach (var instance in instances)
                {
                    if (instance.Status == WorkflowStatus.Started || instance.Status == WorkflowStatus.Suspended)
                        instanceService.TerminateWorkflow(instance);
                }
                ctx.ExecuteQuery();
            }
        }
    }
}

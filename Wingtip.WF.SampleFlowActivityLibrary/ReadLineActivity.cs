using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wingtip.WF.SampleFlowActivityLibrary
{
    public class ReadLineActivity: CodeActivity<String>
    {
        public InArgument<String> Message { get; set; }

        protected override string Execute(CodeActivityContext context)
        {
            Console.WriteLine(this.Message.Get(context));
            return (Console.ReadLine());
        }
    }
}

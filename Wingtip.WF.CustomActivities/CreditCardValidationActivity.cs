using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel.DataAnnotations;

namespace Wingtip.WF.CustomActivities
{

    public sealed class CreditCardValidationActivity : CodeActivity<Boolean>
    {
        // Define an activity input argument of type string
        [RequiredArgument]
        public InArgument<string> CreditCard { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override bool Execute(CodeActivityContext context)
        {
            CreditCardAttribute cc = new CreditCardAttribute();
            return (cc.IsValid(this.CreditCard.Get(context)));
        }
    }
}

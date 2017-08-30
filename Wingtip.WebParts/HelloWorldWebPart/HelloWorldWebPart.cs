using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Wingtip.WebParts.HelloWorldWebPart
{
    [ToolboxItemAttribute(false)]
    public class HelloWorldWebPart : WebPart
    {       
        protected override void CreateChildControls()
        {
            var currentWeb = SPControl.GetContextWeb(HttpContext.Current);
            var currentUserName = currentWeb.CurrentUser.LoginName;
            var welcomeMessage = String.Format("<h1>Welcome {0}!</h1>", currentUserName);
            var currentDateTimeMessage = String.Format("<div>Current DateTime: {0}</div>", DateTime.Now);

            var welcomMessageLiteralctrl = new LiteralControl(welcomeMessage);
            var currentDateTimeLiteralctrl = new LiteralControl(currentDateTimeMessage);

            this.Controls.Add(welcomMessageLiteralctrl);
            this.Controls.Add(currentDateTimeLiteralctrl);
        }
    }
}

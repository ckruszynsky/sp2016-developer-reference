using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using Microsoft.SharePoint.Utilities;

namespace Wingtip.UI.Extensions.Layouts.Wingtip.UI.Extensions
{
    public static class FieldsIds
    {
        public static Guid WingtipInvoiceNumber_ID = new Guid("{F1D40C65-7A6C-4148-AE99-A6DAE71FD375}");
        public static Guid WingtipInvoiceDescription_ID = new Guid("{D29E8CB4-35C5-4EB0-AB59-9C834649C3A0}");
        public static Guid WingtipInvoiceStatus_ID = new Guid("{1F516F60-B435-464F-8B8F-016C04D64CA7}");

        public const String InvoiceStatus_Draft = "Draft";
        public const String InvoiceStatus_Approved = "Approved";
        public const String InvoiceStatus_Sent = "Sent";
        public const String InvoiceStatus_Archived = "Archived";
    }
    public partial class WingtipInvoiceChangeStatus : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var itemId = this.Request.QueryString["ItemId"];
            var listId = this.Request.QueryString["ListId"];
            var status = this.Request.QueryString["Status"];

            if(!string.IsNullOrEmpty(itemId) 
                && !string.IsNullOrEmpty(listId)
                && !string.IsNullOrEmpty(status))
            {
                SPWeb web = this.Web;
                try
                {
                    try
                    {
                        SPList list = web.Lists[new Guid(listId)];
                        SPListItem item = list.GetItemById(int.Parse(itemId));

                        web.AllowUnsafeUpdates = true;
                        item[FieldsIds.WingtipInvoiceStatus_ID] = status;
                        item.Update();

                        SPUtility.Redirect(list.DefaultViewUrl, SPRedirectFlags.Default, HttpContext.Current);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
                catch(ArgumentException)
                {
                    throw new ApplicationException("Invalid List or Item ID!");
                }
            }
        }
        

    }
}

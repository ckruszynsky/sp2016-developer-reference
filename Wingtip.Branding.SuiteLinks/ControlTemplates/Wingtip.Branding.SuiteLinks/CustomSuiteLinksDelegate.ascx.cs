using Microsoft.SharePoint.Portal.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Wingtip.Branding.SuiteLinks.ControlTemplates.Wingtip.Branding.SuiteLinks
{
    public partial class CustomSuiteLinksDelegate : MySuiteLinksUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.Write(".ms-core-suiteLinkList {display: inline-block;}");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ms-core-suiteLinkList");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            RenderSuiteLink(writer, "http://wingtip.com:2016", "Central Admin", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://dev.wingtip.com", "Dev Site", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://intranet.wingtip.com", "Intranet", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://search.wingtip.com", "Search", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://disco.wingtip.com", "Intranet", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://wingtip.com", "Publishing", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://bi.wingtip.com", "BI", "lnkSearchLink", false);
            RenderSuiteLink(writer, "http://disco.wingtip.com", "Intranet", "lnkSearchLink", false);
            writer.RenderEndTag();
            base.Render(writer);
        }
    }
}

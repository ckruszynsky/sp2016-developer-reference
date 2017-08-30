using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Wingtip.WebParts.CreateRequestForContactWebPart
{
    
    [ToolboxItemAttribute(false)]    
    public class CreateRequestForContactWebPart : WebPart
    {
        [WebBrowsable(false)]
        [Personalizable(PersonalizationScope.Shared)]
        [WebDisplayName("The list to save help desk requests to")]
        [Category("Help Desk Configuration Options")]
        public Guid TargetListID { get; set; }

        protected TextBox RequesterFullName;
        protected TextBox RequesterEMail;
        protected TextBox Reason;
        protected Button SubmitRequestForContact;
        protected Label ErrorMessage;


        public override object WebBrowsableObject
        {
            get
            {
                return (this);
            }
        }


        public override EditorPartCollection CreateEditorParts()
        {
            CreateRequestForContactEditorPart editorPart = new CreateRequestForContactEditorPart();
            editorPart.ID = this.ID + "_CreateRequestForContactEditorPart";

            EditorPartCollection editorParts =
              new EditorPartCollection(base.CreateEditorParts(), new EditorPart[] { editorPart });
            return editorParts;
        }
        
        protected override void CreateChildControls()
        {
            if (this.WebPartManager.DisplayMode == WebPartManager.BrowseDisplayMode)
            {
                RenderBrowseDisplay();
            }else if(this.WebPartManager.DisplayMode == WebPartManager.DesignDisplayMode)
            {
                this.Controls.Add(new LiteralControl("<div> Please move to display mode to use this web part."));
            }else if(this.WebPartManager.DisplayMode == WebPartManager.EditDisplayMode)
            {
                this.Controls.Add(new LiteralControl("<div>Please move to display mode to this web part or configure its properties, since you are in Edit mode.</div>"));
            }
        }

        public override WebPartVerbCollection Verbs
        {
            get
            {
                WebPartVerb serverSideVerb = new WebPartVerb("serverSiteVerbId", handleServerSideVerb);
                serverSideVerb.Text = "Server-side verb";
                WebPartVerb clientSideVerb = new WebPartVerb("clientSideVerbId", "javascript:alert('Client-side Verb selected');");
                clientSideVerb.Text = "Client-side verb";
                WebPartVerb clientAndServerSideVerb = new WebPartVerb("clientAndServerSideVerbId",
                    handleServerSideVerb, "javascript:alert('Client-side Verb selected');"
                    );
                clientAndServerSideVerb.Text = "Client and Server-side verb";

                WebPartVerbCollection newVerbs = new WebPartVerbCollection(
                    new WebPartVerb[] {
                        serverSideVerb, clientSideVerb, clientAndServerSideVerb,
                    }
                    );
                return (new WebPartVerbCollection(base.Verbs, newVerbs));
            }
        }

        protected void handleServerSideVerb(Object source, WebPartEventArgs args)
        {
            EnsureChildControls();

            this.ErrorMessage.Text = "You raised a server-side event!";
        }

        private void RenderBrowseDisplay()
        {
            this.RequesterFullName = new TextBox();
            this.RequesterFullName.Columns = 100;
            this.RequesterFullName.MaxLength = 255;
            this.Controls.Add(new LiteralControl("Requester Full Name: "));
            this.Controls.Add(new LiteralControl("<div>"));
            this.Controls.Add(this.RequesterFullName);
            this.Controls.Add(new LiteralControl("</div>"));

            this.RequesterEMail = new TextBox();
            this.RequesterEMail.Columns = 100;
            this.RequesterEMail.MaxLength = 100;
            this.Controls.Add(new LiteralControl("Requester EMail: "));
            this.Controls.Add(new LiteralControl("<div>"));
            this.Controls.Add(this.RequesterEMail);
            this.Controls.Add(new LiteralControl("</div>"));

            this.Reason = new TextBox();
            this.Reason.Columns = 100;
            this.Reason.MaxLength = 255;
            this.Controls.Add(new LiteralControl("Reason: "));
            this.Controls.Add(new LiteralControl("<div>"));
            this.Controls.Add(this.Reason);
            this.Controls.Add(new LiteralControl("</div>"));

            this.Controls.Add(new LiteralControl("<br />"));

            this.SubmitRequestForContact = new Button();
            this.SubmitRequestForContact.Text = "Submit Help Desk Request";
            this.Controls.Add(new LiteralControl("<div>"));
            this.Controls.Add(this.SubmitRequestForContact);
            this.SubmitRequestForContact.Click += new EventHandler(SubmitRequestForContact_Click);
            this.Controls.Add(new LiteralControl("</div>"));

            this.ErrorMessage = new Label();
            this.ErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.Controls.Add(new LiteralControl("<div>"));
            this.Controls.Add(this.ErrorMessage);
            this.Controls.Add(new LiteralControl("</div>"));
        }
        
        void SubmitRequestForContact_Click(object sender, EventArgs e)
        {
            SPWeb web = SPControl.GetContextWeb(HttpContext.Current);

            try
            {
                SPList targetList = web.Lists[TargetListID];

                SPListItem newItem = targetList.Items.Add();
                newItem["Reason"] = this.Reason.Text;
                newItem["Requester Full Name"] = this.RequesterFullName.Text;
                newItem["Requester Email Address"] = this.RequesterEMail.Text;
                newItem.Update();
                this.RequesterFullName.Text = string.Empty;
                this.RequesterEMail.Text = string.Empty;
                this.Reason.Text = string.Empty;
            }
            catch (IndexOutOfRangeException)
            {
                this.ErrorMessage.Text = "Cannot find list \"HelpDeskRequests\"";
            }
        }
    }

    public class CreateRequestForContactEditorPart : EditorPart
    {
        protected DropDownList targetLists;

        protected override void CreateChildControls()
        {
            this.targetLists = new DropDownList();

            SPWeb web = SPControl.GetContextWeb(HttpContext.Current);

            foreach (SPList list in web.Lists)
            {
                this.targetLists.Items.Add(new ListItem(list.Title, list.ID.ToString()));
            }

            this.Title = "Help Desk Request Editor Part";

            this.Controls.Add(new LiteralControl("Select the target List:<br>"));
            this.Controls.Add(this.targetLists);
            this.Controls.Add(new LiteralControl("<br>&nbsp;<br>"));
        }

        public override bool ApplyChanges()
        {
            EnsureChildControls();

            CreateRequestForContactWebPart wp = this.WebPartToEdit as CreateRequestForContactWebPart;
            if (wp != null)
            {
                wp.TargetListID = new Guid(this.targetLists.SelectedValue);
            }

            return (true);
        }

        public override void SyncChanges()
        {
            EnsureChildControls();

            CreateRequestForContactWebPart wp = this.WebPartToEdit as CreateRequestForContactWebPart;
            if (wp != null)
            {
                ListItem selectedItem = this.targetLists.Items.FindByValue(wp.TargetListID.ToString());
                if (selectedItem != null)
                {
                    this.targetLists.ClearSelection();
                    selectedItem.Selected = true;
                }
            }
        }
    }
}

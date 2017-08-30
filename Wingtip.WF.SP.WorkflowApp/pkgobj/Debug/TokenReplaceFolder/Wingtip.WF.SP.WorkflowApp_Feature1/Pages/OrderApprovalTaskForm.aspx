<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink name="sp.js" runat="server" OnDemand="true" LoadAfterUI="true" Localizable="false" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderMain" runat="server">

    <h1>Order Approval Task</h1>

    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />

    <table>
        <tr>
            <td>
               Please choose the outcome for the current task
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" name="approveTaskOutcome" value="Approved" onclick="setTaskOutcome('Approved');" />
                <input type="button" name="rejectTaskOutcome" value="Rejected" onclick="setTaskOutcome('Rejected');" />
                <input type="button" name="toReviewTaskOutcome" value="To Review" onclick="setTaskOutcome('To Review');" />
               <br />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        var ctx;
        var urlParams = null;

        function getUrlParams() {
            if (urlParams == null) {
                urlParams = {};
                var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    urlParams[key] = value;
                });
            }
            return urlParams;
        }

        function setTaskOutcome(outcome) {
            ctx = SP.ClientContext.get_current();
            var web = ctx.get_web();
            var tasksList = web.get_lists().getById(decodeURIComponent(getUrlParams()["List"]));
            var task = tasksList.getItemById(getUrlParams()["ID"]);

            task.set_item("OrderApprovalOutcome", outcome);
            task.set_item("Status", "Completed");
            task.update();

            ctx.executeQueryAsync(
                function (sender, args) {
                    redirFromInitForm();
                },
                function (sender, args) {
                    alert("Error while saving the task outcome: " + args.get_message());
                }
            );
        }

        function redirFromInitForm() {
            window.location = decodeURIComponent(getUrlParams()["Source"]);
        }


    </script>
    
</asp:Content>

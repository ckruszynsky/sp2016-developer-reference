<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ECMAScript.aspx.cs" Inherits="Wingtip.JSOM.Layouts.Wingtip.JSOM.ECMAScript" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink ID="SPScriptLink" runat="server" LoadAfterUI="true" Localizable="false"  Name="SP.js" />

    <script type="text/javascript">
        var clientContext,
            web,
            contactsList;

        function onQuerySucceeded(sender, args) {
            alert('Title of the list :' + this.contactsList.get_title());
        }

        function onQueryFailed(sender, args) {
            alert('Request failed ' + args.get_message() + '\n' + args.get_stackTrace());
        }

        function retrieveContracts() {
            this.clientContext = new SP.ClientContext.get_current();
            this.web = this.clientContext.get_web();
            this.contactsList = this.web.get_lists().getByTitle("WingtipContacts");
            this.clientContext.load(this.contactsList);
            this.clientContext.executeQueryAsync(
                Function.createDelegate(this, this.onQuerySucceeded),
                Function.createDelegate(this, this.onQueryFailed));
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <input type="button" onclick="retrieveContracts()" value="Click me to get this list!" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
JavaScript Demo Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
JavaScript Demo Page
</asp:Content>

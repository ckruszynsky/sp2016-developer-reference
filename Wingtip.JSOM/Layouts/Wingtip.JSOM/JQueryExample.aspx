<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JQueryExample.aspx.cs" Inherits="Wingtip.JSOM.Layouts.Wingtip.JSOM.JQueryExample" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:ScriptLink ID="SPScriptLink" runat="server" LoadAfterUI="true" Localizable="false" Name="SP.js" />
    <script type="text/javascript" src="Scripts/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />    
    
    <style type="text/css">

        #listOfContacts .ui-selecting
        {
            background: #FECA40;
        }
        #listOfContacts .ui-selected
        {
            background: #F39814;
            color: white;
        }
        #listOfContacts
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 60%;
        }
        #listOfContacts li
        {
            margin: 3px;
            padding: 0.4em;
            font-size: 1em;
            height: 15px;
            width: 600px;
        }
    </style>

    <script type="text/javascript">
        var clientContext,
            web,
            contactsList,
            listItems;

        _spBodyOnLoadFunctionNames.push("InitData");

        function onQuerySucceeded(sender, args) {
            dataBindList();
        }

        function onQueryFailed(sender, args) {
            alert('Request failed ' + args.get_message() + '\n' + args.get_stackTrace());
        }

        function InitData() {

            this.clientContext = new SP.ClientContext.get_current();
            this.web = this.clientContext.get_web();
            this.oContactsList = this.web.get_lists().getByTitle("WingtipContacts");

            var camlQuery = new SP.CamlQuery();
            var q = '<View><RowLimit>100</RowLimit></View>';
            camlQuery.set_viewXml(q);
            this.listItems = this.oContactsList.getItems(camlQuery);
            this.clientContext.load(this.listItems);

            this.clientContext.executeQueryAsync(
            Function.createDelegate(this, this.onQuerySucceeded),
            Function.createDelegate(this, this.onQueryFailed));
        }

        function dataBindList() {

            var listItemsEnumerator = this.listItems.getEnumerator();
            //iterate though all of the items
            while (listItemsEnumerator.moveNext()) {
                var item = listItemsEnumerator.get_current();

                var id = item.get_id();
                var title = item.get_item("Title");
                var contactId = item.get_item("WingtipContactID");
                var companyName = item.get_item("WingtipCompanyName");
                var country = item.get_item("WingtipCountry");

                $("#listOfContacts").append('<li class="ui-widget-content" id="item_' + id +
                    '">Title: ' + title +
                    ' - Contact ID: ' + contactId +
                    ' - Company Name: ' + companyName +
                    ' - Country: ' + country + '</li>');
            }

            $("#listOfContacts").selectable();
        }


    </script>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <div id="listOfContactsContainer">
        <ol id="listOfContacts"></ol>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
JQuery JS Example
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
JQuery JS Example
</asp:Content>

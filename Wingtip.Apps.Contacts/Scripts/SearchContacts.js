var clientContext;
var web;
var contactsList;
var listItems;
var queryArguments;

// This code runs when the DOM is ready and creates a context object which is needed to use the SharePoint object model
$(document).ready(function () {
    clientContext = SP.ClientContext.get_current();
    web = clientContext.get_web();

    queryArguments = {};
    var q = document.URL.split('?')[1];
    if (q != undefined) {
        q = q.split('&');
        for (var i = 0; i < q.length; i++) {
            var pair = q[i].split('=');
            queryArguments[pair[0]] = pair[1];
        }
    }
});

// This function prepares, loads, and then executes a SharePoint query to get the search query for app contacts results
function searchContacts() {
    contactsList = web.get_lists().getByTitle("WingtipAppContacts");

    var textToSearch = $("#textToSearch").val();
    var camlQuery = new SP.CamlQuery();
    var q = '<View><Query><Where><Contains><FieldRef Name="WingtipAppContactDescription" /><Value Type="Text">' + textToSearch + '</Value></Contains></Where></Query></View>';
    camlQuery.set_viewXml(q);
    listItems = contactsList.getItems(camlQuery);
    clientContext.load(listItems);

    clientContext.executeQueryAsync(onSearchQuerySucceeded, onSearchQueryFailed);
}

// Output the result
function onSearchQuerySucceeded(sender, args) {

    $("#searchOutput").empty();

    if (listItems.get_count() > 0) {
        var listItemsEnumerator = listItems.getEnumerator();

        //iterate though all of the items    
        while (listItemsEnumerator.moveNext()) {
            var item = listItemsEnumerator.get_current();

            var id = item.get_id();
            var title = item.get_item("Title");
            var contactDescription = item.get_item("WingtipAppContactDescription");
            var contactTelephone = item.get_item("WingtipAppContactTelephone");
            var contactEmail = item.get_item("WingtipAppContactEmail");
            var contactPhoto = item.get_item("WingtipAppContactPhoto").get_url();

            $("#searchOutput").append('<a href="mailto:' + contactEmail + '">' +
                '<img style="float: left; margin: 5px;" src="' +
                contactPhoto + '" align="left" alt="' + title + '"/></a>');

            if (queryArguments['Direction'] == 'Vertical') {
                // You should render the results vertically
            }
        }
    }
    else {
        $("#searchOutput").append('<div>No results matching the query. Try again ...</div>');
    }
}

function onSearchQueryFailed(sender, args) {
    alert('Request failed ' + args.get_message() + '\n' + args.get_stackTrace());
}
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wingtip.CSOM
{
    class Program
    {
        static string url = "http://intranet.wingtip.com";
        static void Main(string[] args)
        {

            BrowseForContacts();
            Console.Read();
        }

        static void BrowseForContacts()
        {
            // Open the current ClientContext
            ClientContext ctx = new ClientContext(url);
            //ctx.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
            //FormsAuthenticationLoginInfo loginInfo = new FormsAuthenticationLoginInfo { 
            //    LoginName = "UserLoginName",
            //    Password = "HereYourPassword",
            //};
            //ctx.FormsAuthenticationLoginInfo = loginInfo;

            // Prepare a reference to the current Site Collection
            Site site = ctx.Site;
            ctx.Load(site);

            // Prepare a reference to the current Web Site
            Web web = site.RootWeb;
            ctx.Load(web);

            // Prepare a reference to the list of "Wingtip Contacts"
            List list = web.Lists.GetByTitle("WingtipContacts");
            ctx.Load(list);

            // Execute the prepared commands against the target ClientContext
            ctx.ExecuteQuery();

            // Show the title of the list just retrieved
            Console.WriteLine(list.Title);

            // Prepare a query for all items in the list
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<View/>";
            ListItemCollection allContacts = list.GetItems(query);
            ctx.Load(allContacts,
                items => items.IncludeWithDefaultProperties(
                    item => item.DisplayName,
                    item => item.RoleAssignments
                    ));


            // Execute the prepared command against the target ClientContext
            ctx.ExecuteQuery();

            // Browse the result
            Console.WriteLine("\nContacts");
            foreach (ListItem listItem in allContacts)
            {               
                ctx.ExecuteQuery();

                Console.WriteLine("Id: {0} - Fullname: {1} - Company: {2} - Country: {3}",
                    listItem["WingtipContactID"],
                    listItem["Title"],
                    listItem["WingtipCompanyName"],
                    listItem["WingtipCountry"]
                    );

                Console.WriteLine(listItem.DisplayName);
            }
        }
    }
}

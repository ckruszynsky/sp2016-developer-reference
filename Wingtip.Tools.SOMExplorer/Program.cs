using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Linq;

namespace Wingtip.Tools.SOMExplorer
{
    class Program
    {

        static void Main(string[] args)
        {
            //ShowFarmConfiguration(true);
           // BrowseSiteCollectionsAndWebs();
            //OpenSiteWithCurrentUserImpersonation();
           // OpenWebSiteViaSiteCollection();
            //ChangeWebSiteTitle();
            //BrowseSingleListItems();
            //BrowseFiles();
            //BrowseRoleAssignmentsAndDefinitions();
            //UsingSample();
            //TryFinallySample();
            //TryCatchUsingSample();
            //WrongUsingSample();
            //GoodDisposingSample();
            //GoodDisposingCollectionSample();
            //LoggerSample();
            //AllowUnsafeUpdatesSample();
            //CreateNewSiteCollection();
            //CreateNewWebSite();
            //CreateNewList();
            //PopulateListWithItems();
            //UpdateListItem();
            //ConcurrencyListItem();
            //DeleteListItem(true);
            //QueryList();
            //QueryListWithPaging();
            //CreateDocumentLibrary();
            //UploadDocumentInLibrary();
            //DownloadDocumentFromLibrary();
            //CopyOrMoveDocumentBetweenLibraries(false);
            //ExtractSecondLastVersionOfDocument();
            //AddNewUserToWeb();
            //EnsureUserInWeb();
            //AddUserInGroup();
            //CreatePermissionSetAndAssignToPrincipal();
            Console.ReadLine();
        }

        // Listing 5-1
        static void ShowFarmConfiguration(Boolean useLocal)
        {
            SPFarm farm = null;

            if (!useLocal)
            {
                SecureString passphrase = new SecureString();

                Console.WriteLine("Enter Password:");
                while (true)
                {
                    ConsoleKeyInfo conKeyInfo = Console.ReadKey(true);
                    if (conKeyInfo.Key == ConsoleKey.Enter)
                        break;
                    else if (conKeyInfo.Key == ConsoleKey.Escape)
                        return;
                    else if (conKeyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (passphrase.Length != 0)
                            passphrase.RemoveAt(passphrase.Length - 1);
                    }
                    else
                        passphrase.AppendChar(conKeyInfo.KeyChar);
                }

                farm = SPFarm.Open(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SharePointConfig"].ConnectionString), passphrase);
            }
            else
            {
                farm = SPFarm.Local;
            }

            Console.WriteLine("Here are the servers of the Farm");
            foreach (SPServer server in farm.Servers)
            {
                Console.WriteLine("Server Name: {0}", server.Name);
                Console.WriteLine("Server Address: {0}", server.Address);
                Console.WriteLine("Server Role: {0}", server.Role);
            }

            foreach (SPService service in farm.Services)
            {
                Console.WriteLine("---------------------------------------");
                if (service is SPWindowsService)
                {
                    Console.WriteLine("Windows Service: {0}", service.DisplayName);
                    Console.WriteLine("Type: {0}", service.TypeName);
                    Console.WriteLine("Instances: {0}", service.Instances.Count);
                }
                else if (service is SPWebService)
                {
                    Console.WriteLine("Web Service: {0}", service.DisplayName);
                    Console.WriteLine("Type: {0}", service.TypeName);
                    Console.WriteLine("Instances: {0}", service.Instances.Count);

                    SPWebService webService = service as SPWebService;

                    if (webService != null)
                    {
                        foreach (SPWebApplication webApplication in webService.WebApplications)
                        {
                            Console.WriteLine("Web Application: {0}", webApplication.DisplayName);

                            Console.WriteLine("Content Databases");
                            foreach (SPContentDatabase db in webApplication.ContentDatabases)
                            {
                                Console.WriteLine("Content Database: {0}", db.Name);
                                Console.WriteLine("Connection String: {0}", db.DatabaseConnectionString);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Generic Service Name: {0}", service.DisplayName);
                    Console.WriteLine("Type Name: {0}", service.TypeName);
                    Console.WriteLine("Instances: {0}", service.Instances.Count);
                }
            }
        }

        // Listing 5-2
        static void BrowseSiteCollectionsAndWebs()
        {
            SPFarm farm = SPFarm.Local;

            foreach (SPService service in farm.Services)
            {
                if (service is SPWebService)
                {
                    Console.WriteLine("Web Service: {0}", service.DisplayName);
                    Console.WriteLine("Type: {0}", service.TypeName);
                    Console.WriteLine("Instances: {0}", service.Instances.Count);

                    SPWebService webService = service as SPWebService;

                    if (webService != null)
                    {
                        foreach (SPWebApplication webApplication in webService.WebApplications)
                        {
                            Console.WriteLine("Web Application: {0}", webApplication.DisplayName);

                            foreach (SPSite site in webApplication.Sites)
                            {
                                using (site)
                                {
                                    Console.WriteLine("Site Collection: {0}", site.Url);
                                    try
                                    {
                                        foreach (SPWeb web in site.AllWebs)
                                    {
                                      
                                            using (web)
                                            {
                                                Console.WriteLine("Web Site: {0}", web.Title);
                                            }                                       
                                    }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        // Listing 5-3
        static void OpenSiteWithCurrentUserImpersonation()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.RootWeb)
                {
                    Console.WriteLine("Current Site RootWeb Title: {0}", web.Title);
                }
            }
        }

        // Listing 5-4
        static void OpenWebSiteViaSiteCollection()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.OpenWeb("SampleSubSite"))
                {
                    Console.WriteLine(web.Title);
                }
            }
        }

        // Listing 5-5
        static void ChangeWebSiteTitle()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.OpenWeb("SampleSubSite"))
                {
                    web.Title = web.Title + " - Changed by code!";
                    web.Update();
                }
            }
        }

        // Listing 5-6
        static void BrowseSingleListItems()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["wingtip Customers"];

                    foreach (SPListItem item in list.Items)
                    {
                        Console.WriteLine(item.Title);
                    }
                }
            }
        }

        // Listing 5-7
        static void BrowseFiles()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary docLibrary = web.Lists["Documents"] as SPDocumentLibrary;

                    foreach (SPListItem item in docLibrary.Items)
                    {
                        Console.WriteLine("{0} - {1}",
                            item.File.Name,
                            item.File.Length);
                    }
                }
            }
        }

        // Listing 5-8
        static void BrowseRoleAssignmentsAndDefinitions()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                Console.WriteLine("Current Site URL: {0}", site.Url);

                using (SPWeb web = site.OpenWeb())
                {
                    foreach (SPRoleAssignment ra in web.RoleAssignments)
                    {
                        Console.WriteLine("=> Member Name: {0}", ra.Member.Name);

                        foreach (SPRoleDefinition rd in ra.RoleDefinitionBindings)
                        {
                            Console.WriteLine("Permissions: {0}", rd.BasePermissions);
                        }
                    }
                }
            }
        }

        // Listing 5-9
        static void UsingSample()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                // Work with the SPSite object
            }
        }

        // Listing 5-10
        static void TryFinallySample()
        {
            SPSite site = null;
            try
            {
                site = new SPSite("http://wingtipServer");
                // Work with the SPSite object
            }
            finally
            {
                if (site != null)
                    site.Dispose();
            }
        }

        // Listing 5-11
        static void TryCatchUsingSample()
        {
            try
            {
                using (SPSite site = new SPSite("http://wingtipServer"))
                {
                    // Work with the SPSite object
                }
            }
            catch (SPException ex)
            {
                // Handle exception
            }
        }

            

        // Listing 5-14
        static void GoodDisposingCollectionSample()
        {
            try
            {
                using (SPSite site = new SPSite("http://wingtipServer"))
                {
                    // Work with the SPSite object
                    foreach (SPWeb web in site.AllWebs)
                    {
                        using (web)
                        {
                            // Work with the SPWeb object
                        }
                    }
                }
            }
            catch (SPException ex)
            {
                // Handle exception
            }
        }

        // Listing 5-15
        static void LoggerSample()
        {
            try
            {
                using (SPSite site = new SPSite("http://wingtipServer"))
                {
                    // Work with the SPSite object
                    foreach (SPWeb web in site.AllWebs)
                    {
                        using (web)
                        {
                            // Work with the SPWeb object
                            throw new SPException("Sample Exception");
                        }
                    }
                }
            }
            catch (SPException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("My Category", TraceSeverity.Unexpected, EventSeverity.Error), TraceSeverity.Unexpected, ex.Message, ex.StackTrace);
            }
        }

        // Listing 5-16
        static void AllowUnsafeUpdatesSample()
        {
            using (SPWeb web = SPContext.Current.Web)
            {
                SPList list = web.Lists["Wingtip Customers"];

                try
                {
                    web.AllowUnsafeUpdates = true;

                    list.Title = list.Title + " - Changed!";
                    list.Update();
                }
                finally
                {
                    web.AllowUnsafeUpdates = false;
                }
            }
        }

        // Listing 5-17
        static void CreateNewSiteCollection()
        {
            using (SPSite rootSite = new SPSite("http://wingtipServer"))
            {
                SPWebApplication webApplication = rootSite.WebApplication;

                using (SPSite newSiteCollection = webApplication.Sites.Add(
                    "sites/CreatedByCode", // Site URL
                    "Created by Code", // Site Collection Title
                    "Sample Site Collection Created by Code", // Site Collection Description
                    1033, // LCID
                    15, // Compatibility level (can be 14 for 2010 or 15 for 2013)
                    "STS#0", // Site Template
                    "SHAREPOINT\\Paolo.Pialorsi", // Owner Login
                    "Paolo Pialorsi", // Owner DisplayName
                    "paolo@wingtip.com", // Owner EMail
                    "SHAREPOINT\\Marco.Russo", // Secondary Contact Login
                    "Marco Russo", // Secondary Contact DisplayName
                    "marco@wingtip.com", // Secondary Contact EMail
                    "SP2013SQL", // Database Server Name for Content Database
                    "SP2013_Farm_WSS_Content_CreatedByCode3", // Content Database Name
                    null, // Database Login Name
                    null // Database Login Password
                    ))
                {
                    Console.WriteLine("Created Site Collection: {0}",
                        newSiteCollection.Url);
                }
            }
        }

        // Listing 5-18
        static void CreateNewWebSite()
        {
            using (SPSite site = new SPSite("http://wingtipServer/sites/CreatedByCode/"))
            {
                using (SPWeb newWeb = site.AllWebs.Add(
                    "MyBlog", // Web Site Url
                    "Blog Created By Code", // Web Site Title
                    "Blogging Site Created By Code", // Web Site Description
                    1033, // LCID
                    "BLOG#0", // Site Template Name
                    true,  // Use Unique Permissions
                    false // Convert an existin folder
                    ))
                {
                    Console.WriteLine("New Web Site URL: {0}", newWeb.Url);
                }
            }
        }

        // Listing 5-19
        static void CreateNewList()
        {
            using (SPSite site = new SPSite("http://wingtipServer/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    Guid newListId = web.Lists.Add(
                        "Contacts", // List Title
                        "Company's Contacts", // List Description
                        SPListTemplateType.Contacts // List Template Type
                        );

                    SPList newList = web.Lists[newListId];
                    newList.OnQuickLaunch = true;
                    newList.ReadSecurity = 1; // All users have Read access to all items
                    newList.WriteSecurity = 2; // Users can modify only items that they created
                    newList.Update();

                    Console.WriteLine("Created list: {0}", newList.Title);
                }
            }
        }

        // Listing 5-20
        static void PopulateListWithItems()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        SPList list = web.Lists["Contacts"];

                        try
                        {
                            SPListItem newItem = list.Items.Add();
                            newItem["Title"] = "Pialorsi";
                            newItem["FirstName"] = "Paolo";
                            newItem["Email"] = "paolo@wingtip.it";
                            newItem.Update();
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid Field Name!");
                        }
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Invalid List Title!");
                    }
                }
            }
        }

        // Listing 5-21
        static void UpdateListItem()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        SPList list = web.Lists["Contacts"];
                        SPListItem itemToChange = list.GetItemByIdSelectedFields(1, "Last Name");

                        itemToChange["Title"] += " - Changed!";
                        itemToChange.Update();
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Invalid List Title or invalid List Item ID!");
                    }
                }
            }
        }

        // Listing 5-22
        static void ConcurrencyListItem()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    try
                    {
                        SPList list = web.Lists["Contacts"];
                        SPListItem itemToChange = list.GetItemById(1);

                        itemToChange["Title"] += " - Changed!";

                        // Before Update simulate a concurrent change
                        ChangeListItemConcurrently();
                        itemToChange.Update();
                    }
                    catch (SPException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        static void ChangeListItemConcurrently()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["Contacts"];
                    SPListItem itemToChange = list.GetItemById(1);

                    itemToChange["Title"] += " - Changed!";
                    itemToChange.Update();
                }
            }
        }

        // Listing 5-23
        static void DeleteListItem(Boolean recycle)
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["Contacts"];
                    SPListItem itemToDelete = list.GetItemById(1);

                    if (recycle)
                        itemToDelete.Recycle();
                    else
                        itemToDelete.Delete();
                }
            }
        }

        // Listing 5-24
        static void QueryList()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["Contacts"];

                    SPQuery query = new SPQuery();

                    // Define columns to retrieve
                    query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"FirstName\" /><FieldRef Name=\"Email\" />";
                    // Force retrieving only the selected columns
                    query.ViewFieldsOnly = true;
                    // Define the query
                    query.Query = "<Where><Contains><FieldRef Name=\"Email\" /><Value Type=\"Text\">@wingtip.it</Value></Contains></Where>";
                    // Define the maximum number of results for each (like a SELECT TOP)
                    query.RowLimit = 10;

                    // Query for items
                    SPListItemCollection items = list.GetItems(query);

                    foreach (SPListItem item in items)
                    {
                        Console.WriteLine("{0} {1} - {2}",
                            item["FirstName"],
                            item["Title"],
                            item["Email"]);
                    }
                }
            }
        }

        // Listing 5-25
        static void QueryListWithPaging()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists["Contacts"];

                    SPQuery query = new SPQuery();

                    // Define columns to retrieve
                    query.ViewFields = "<FieldRef Name=\"Title\" /><FieldRef Name=\"FirstName\" /><FieldRef Name=\"Email\" />";
                    // Force retrieving only the selected columns
                    query.ViewFieldsOnly = true;
                    // Define the query
                    query.Query = "<Where><Contains><FieldRef Name=\"Email\" /><Value Type=\"Text\">@domain.com</Value></Contains></Where>";
                    // Define the maximum number of results for each page (like a SELECT TOP)
                    query.RowLimit = 5;

                    Int32 pageIndex = 1;
                    Int32 itemIndex = 1;

                    do
                    {
                        Console.WriteLine("Current Page: {0}", pageIndex);

                        // Query for items
                        SPListItemCollection items = list.GetItems(query);

                        foreach (SPListItem item in items)
                        {
                            Console.WriteLine("{0} - {1} {2} - {3}",
                                itemIndex,
                                item["FirstName"],
                                item["Title"],
                                item["Email"]);
                            itemIndex++;
                        }
                        // Set current position to make SPQuery able
                        // to set the start item of the next page
                        query.ListItemCollectionPosition =
                          items.ListItemCollectionPosition;
                        pageIndex++;
                    } while (query.ListItemCollectionPosition != null);
                }
            }
        }

        // Listing 5-26
        static void CreateDocumentLibrary()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPListTemplate listTemplate = web.ListTemplates["Document Library"];
                    SPDocTemplate docTemplate =
                        (from SPDocTemplate dt in web.DocTemplates
                         where dt.Type == 122
                         select dt).FirstOrDefault();

                    Guid newListId = web.Lists.Add(
                        "Invoices", // List Title
                        "Excel Invoices", // List Description
                        listTemplate, // List Template
                        docTemplate // Document Template (i.e. Excel)
                        );

                    SPDocumentLibrary newLibrary = web.Lists[newListId] as SPDocumentLibrary;
                    newLibrary.OnQuickLaunch = true;
                    newLibrary.EnableVersioning = true;
                    newLibrary.Update();

                    Console.WriteLine("Created document library: {0}", newLibrary.Title);
                }
            }
        }

        // Listing 5-27
        static void UploadDocumentInLibrary()
        {
            using (SPSite site = new SPSite("http://wingtipServer/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary library = web.Lists["Invoices"] as SPDocumentLibrary;

                    using (FileStream fs = new FileStream(@"..\..\DemoInvoice.xlsx",
                        FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        SPFile fileUploaded = library.RootFolder.Files.Add(
                            "DemoInvoice.xlsx", fs, true);

                        Console.WriteLine("Uploaded file: {0}", fileUploaded.Url);
                    }
                }
            }
        }

        // Listing 5-28
        static void DownloadDocumentFromLibrary()
        {
            using (SPSite site = new SPSite("http://wingtipServer/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary library = web.Lists["Invoices"] as SPDocumentLibrary;

                    SPFile fileToDownload = web.GetFile(library.RootFolder.Url + "/DemoInvoice.xlsx");

                    Int32 bufferLenght = 4096;
                    Int32 readLenght = bufferLenght;
                    Byte[] buffer = new Byte[bufferLenght];

                    Stream inStream = fileToDownload.OpenBinaryStream();
                    using (FileStream outStream = new FileStream(
                        @"..\..\DemoInvoiceDownload.xlsx",
                        FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        while (readLenght == buffer.Length)
                        {
                            readLenght = inStream.Read(buffer, 0, bufferLenght);
                            outStream.Write(buffer, 0, readLenght);
                            if (readLenght < bufferLenght) break;
                        }
                    }
                }
            }
        }

        // Listing 5-29
        static void CheckOutInDocumentInLibrary()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary library = web.Lists["Invoices"] as SPDocumentLibrary;

                    SPFile file = web.GetFile(library.RootFolder.Url + "/DemoInvoice.xlsx");

                    if (file.CheckOutType == SPFile.SPCheckOutType.None)
                    {
                        // If the file is not already checked out ... check it out
                        file.CheckOut();
                    }
                    else
                    {
                        // Otherwise check it in leaving a comment
                        file.CheckIn("File Checked-In for demo purposes",
                            SPCheckinType.MajorCheckIn);
                    }
                }
            }
        }

        // Listing 5-30
        static void CopyOrMoveDocumentBetweenLibraries(Boolean move)
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary sourceLibrary = web.Lists["Invoices"] as SPDocumentLibrary;
                    SPDocumentLibrary destinationLibrary = web.Lists["Invoices History"] as SPDocumentLibrary;

                    SPFile file = web.GetFile(sourceLibrary.RootFolder.Url + "/DemoInvoice.xlsx");

                    if (move)
                    {
                        // It is a file moving action
                        file.MoveTo(destinationLibrary.RootFolder.Url + "/DemoInvoice_Moved.xlsx", true);
                    }
                    else
                    {
                        // It is a file copy action
                        file.CopyTo(destinationLibrary.RootFolder.Url + "/DemoInvoice_Copied.xlsx", true);
                    }
                }
            }
        }

        // Listing 5-31
        static void ExtractSecondLastVersionOfDocument()
        {
            using (SPSite site = new SPSite("http://wingtipserver/sites/CreatedByCode/"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPDocumentLibrary library = web.Lists["Invoices"] as SPDocumentLibrary;

                    SPFile file = web.GetFile(library.RootFolder.Url + "/DemoInvoice.xlsx");

                    Console.WriteLine("Available versions:");

                    foreach (SPFileVersion v in file.Versions)
                    {
                        Console.WriteLine("Version: {0} - URL: {1}", v.VersionLabel, v.Url);
                    }

                    SPFile fileOfSecondLastVersion = file.Versions[file.Versions.Count - 1].File;

                    Console.WriteLine(fileOfSecondLastVersion.Name);
                }
            }
        }

        // Listing 5-32
        static void AddNewUserToWeb()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.SiteUsers.Add("SHAREPOINT\\TestUser", "test@wingtip.com", "Test User", null);

                    SPUser userAdded = web.SiteUsers["SHAREPOINT\\TestUser"];

                    Console.WriteLine(userAdded.Xml);
                }
            }
        }

        // Listing 5-33
        static void EnsureUserInWeb()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPUser userAdded = web.EnsureUser("SHAREPOINT\\AnotherTestUser");

                    Console.WriteLine(userAdded.Xml);
                }
            }
        }

        // Listing 5-34
        static void AddUserInGroup()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPUser user = web.EnsureUser("SHAREPOINT\\AnotherTestUser");

                    web.Groups["Excel Services Viewers"].AddUser(user);
                }
            }
        }

        // Listing 5-35
        static void CreatePermissionSetAndAssignToPrincipal()
        {
            using (SPSite site = new SPSite("http://wingtipServer"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPUser user = web.EnsureUser("SHAREPOINT\\AnotherTestUser");

                    SPRoleDefinition newRoleDefinition = new SPRoleDefinition();
                    newRoleDefinition.Name = "Custom Permission Level";
                    newRoleDefinition.Description = "View Pages, Browse Directories, Update Personal Web Parts";
                    newRoleDefinition.BasePermissions = SPBasePermissions.ViewPages | SPBasePermissions.BrowseDirectories | SPBasePermissions.UpdatePersonalWebParts;
                    web.RoleDefinitions.Add(newRoleDefinition);

                    SPPrincipal principal = user;
                    SPRoleAssignment newRoleAssignment = new SPRoleAssignment(principal);
                    newRoleAssignment.RoleDefinitionBindings.Add(web.RoleDefinitions["Custom Permission Level"]);

                    web.RoleAssignments.Add(newRoleAssignment);
                }
            }
        }
    }
}

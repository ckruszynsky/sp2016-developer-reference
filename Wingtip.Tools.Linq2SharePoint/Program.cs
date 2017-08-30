using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Wingtip.Tools.Linq2SharePoint
{
    class Program
    {
        private const string url = "http://intranet.wingtip.com";

        static void Main(string[] args)
        {
            //InsertContactItem();
            //QueryInvoices();
            //QueryContactsJoinedWithInvoices();
           // UpdateContactItem();            
            //DeleteContactItem(true);
            //ConcurrencyManagement();
            ShowIdentityManagementBehavior();
            //EntitySerialization();
            Console.ReadLine();
        }

        static void InsertContactItem()
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                spContext.Log = Console.Out;

                WingtipCustomer newCustomer = new WingtipCustomer
                {
                    Title = "John Doe",
                    ContactID = "AP002",
                    CompanyName = "Wingtip",
                    Country = Country.Italy,
                    CustomerLevel = CustomerLevel.LevelA,
                };

                spContext.WingtipContacts.InsertOnSubmit(newCustomer);

                spContext.SubmitChanges();
            }
        }


        static void QueryInvoices()
        {
            using(WingtipDataContext spContext = new WingtipDataContext("http://intranet.wingtip.com"))
            {
                spContext.Log = Console.Out;
                var invoices = (from i in spContext.WingtipInvoices                            
                            select new
                            {
                                i.Title,                                                                                                
                            })
                            .ToList();

                foreach(var invoice in invoices)
                {
                    Console.WriteLine(invoice.Title);
                }            
            }
        }

        static void QueryContactsJoinedWithInvoices()
        {
            using (WingtipDataContext spContext = new WingtipDataContext("http://intranet.wingtip.com"))
            {
                // spContext.Log = Console.Out;

                var query = from c in spContext.WingtipContacts
                            join i in spContext.WingtipInvoices on c.ContactID equals i.ContactID
                            select c;

                foreach (var i in query)
                {
                    Console.WriteLine(i);
                }
            }
        }
      
        static void UpdateContactItem()
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                spContext.Log = Console.Out;

                var contact = (from c in spContext.WingtipContacts
                               where c.ContactID == "AP001"
                               select c).FirstOrDefault();

                // Let's see if we found the target contact
                if (contact != null)
                {
                    Console.WriteLine(((ITrackEntityState)contact).EntityState);
                    contact.Country = Country.USA;
                    Console.WriteLine(((ITrackEntityState)contact).EntityState);

                    spContext.SubmitChanges();
                    Console.WriteLine(((ITrackEntityState)contact).EntityState);
                }
            }
        }

    
        static void DeleteContactItem(Boolean recycle)
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                spContext.Log = Console.Out;

                var contact = (from c in spContext.WingtipContacts
                               where c.ContactID == "AP001"
                               select c).FirstOrDefault();

                // Let's see if we found the target contact
                if (contact != null)
                {
                    if (recycle)
                    {
                        spContext.WingtipContacts.RecycleOnSubmit(contact);
                    }
                    else
                    {
                        spContext.WingtipContacts.DeleteOnSubmit(contact);
                    }

                    spContext.SubmitChanges();
                }
            }
        }

        static void ConcurrencyManagement()
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                spContext.Log = Console.Out;

                var contacts = from c in spContext.WingtipContacts
                               where c.Country == Country.Italy
                               select c;

                String conflictingItemID = contacts.FirstOrDefault().ContactID;

                foreach (var item in contacts)
                {
                    item.CompanyName += String.Format(" - Changed on {0}", DateTime.Now);
                }

                // Before submitting changes, the code simulate concurrency
                // changing one of the items from another DataContext
                using (WingtipDataContext spContextOther =
                    new WingtipDataContext(url))
                {
                    var conflictingItem = (from c in spContextOther.WingtipContacts
                                           where c.ContactID == conflictingItemID
                                           select c).FirstOrDefault();

                    conflictingItem.Country = Country.USA;
                    spContextOther.SubmitChanges();
                }

                try
                {
                    spContext.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException ex)
                {
                    Console.WriteLine(ex.Message);

                    // Browse for conflicting items
                    foreach (var conflict in spContext.ChangeConflicts)
                    {
                        // Check if the item has been deleted by
                        // someone else
                        if (conflict.IsDeleted)
                        {
                            Console.WriteLine("Unfortunately the item has been deleted, so your changes cannot be submitted!");
                        }
                        else
                        {
                            // Retrieve a typed reference to the conflicting item
                            WingtipContact contact = conflict.Object as WingtipContact;

                            // If the item is a WingtipContact
                            if (contact != null)
                            {
                                Console.WriteLine("Contact with ID {0} is in conflict!", contact.ContactID);

                                // Browse for conflicting members
                                foreach (var member in conflict.MemberConflicts)
                                {
                                    Console.WriteLine("Member {0} is in conflict.\n\tCurrent Value: {1}\n\tOriginal Value: {2}\n\tDatabase Value: {3}",
                                        member.Member.Name,
                                        member.CurrentValue,
                                        member.OriginalValue,
                                        member.DatabaseValue);
                                }

                                Console.WriteLine("Make your choice: Override Database Value (Y) or Skip your Current Values (N)?");
                                String choice = Console.ReadLine().ToLower();

                                switch (choice)
                                {
                                    case "y":
                                    case "yes":
                                        conflict.Resolve(RefreshMode.KeepChanges, true);
                                        break;
                                    case "n":
                                    case "no":
                                        conflict.Resolve(RefreshMode.OverwriteCurrentValues, true);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    spContext.SubmitChanges();
                }
            }
        }
        
        static void ShowIdentityManagementBehavior()
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                // spContext.Log = Console.Out;

                var contacts = from c in spContext.WingtipContacts
                               where c.CompanyName.Contains("Wingtip")
                               select c;

                // Change the Country property of the first contact
                contacts.FirstOrDefault().Country = Country.USA;

                spContext.SubmitChanges();

                // Show all the retrieved contacts
                foreach (var c in contacts)
                {
                    Console.WriteLine("Customer with ID {0} has a Country value of {1}",
                        c.ContactID, c.Country);
                }

                Console.WriteLine("------------------");

                // Retrieve the same contacts with another LINQ query
                var otherContacts = from c in spContext.WingtipContacts
                                    where c.CompanyName.Contains("Wingtip")
                                    select c;

                // Show all the newly retrieved contacts
                foreach (var c in otherContacts)
                {
                    Console.WriteLine("Customer with ID {0} has a Country value of {1}",
                        c.ContactID, c.Country);
                }

                // Check if the two first contacts instances are the same contact
                Console.WriteLine("Do the contacts have the same HashCode? {0}",
                    contacts.FirstOrDefault().GetHashCode() ==
                        otherContacts.FirstOrDefault().GetHashCode());
            }
        }

        static void EntitySerialization()
        {
            using (WingtipDataContext spContext = new WingtipDataContext(url))
            {
                spContext.DeferredLoadingEnabled = false;

                var contact = (from c in spContext.WingtipContacts                               
                               where c.ContactID == "AP001"
                               select c).FirstOrDefault();

                // Let's see if we found the target contact
                if (contact != null)
                {
                    DataContractSerializer dcs = new DataContractSerializer(typeof(WingtipContact),
                        new Type[] { typeof(WingtipCustomer), typeof(WingtipSupplier) });

                    try
                    {
                        using (XmlWriter xw = XmlWriter.Create(Console.Out))
                        {
                            dcs.WriteObject(xw, contact);
                            xw.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

    }
}

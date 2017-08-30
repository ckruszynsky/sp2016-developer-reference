using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wingtip.BCS.CRM
{
    // This class has been defined to add an overloaded constructor
    // to the SampleCRMEntities type. Thus, you will be able to
    // create SampleCRMEntities context instances providing the
    // connection string by code
    public partial class SampleCRMEntities
    {
        public static SampleCRMEntities CreateContext()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SampleCRM"].ConnectionString;

                System.Data.SqlClient.SqlConnectionStringBuilder scsb = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

                EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder();
                ecsb.Metadata = "res://*/SampleCRM.csdl|res://*/SampleCRM.ssdl|res://*/SampleCRM.msl";
                ecsb.Provider = "System.Data.SqlClient";
                ecsb.ProviderConnectionString = scsb.ConnectionString;

                return (new SampleCRMEntities(ecsb.ToString()));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Invalid configuration! Error: {0}", ex.Message), ex);
            }
        }

        public SampleCRMEntities(String connectionString) :
            base(connectionString)
        {
        }
    }
}

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Wingtip.WebParts.Connected.CategoriesWebPart
{
    [ToolboxItemAttribute(false)]
    public class CategoriesWebPart : WebPart, ICategoriesProvider
    {
        protected GridView gridCategories;

        [WebBrowsable(true)]
        [Personalizable(true)]
        public string XmlDataSourceUri { get; set; }

        public string CategoryId
        {
            get
            {
                if (this.gridCategories.SelectedIndex >= 0)
                {
                    return (this.gridCategories.SelectedDataKey.Value as String);
                }
                else
                {
                    return (String.Empty);
                }
            }
        }

        [ConnectionProvider("Category")]
        public ICategoriesProvider GetCategoryProvider()
        {
            return (this);
        }

        protected override void CreateChildControls()
        {
            if (!string.IsNullOrEmpty(this.XmlDataSourceUri))
            {
                SPWeb web = SPControl.GetContextWeb(HttpContext.Current);
                SPFile file = web.GetFile(this.XmlDataSourceUri);

                XElement ds = XElement.Load(XmlReader.Create(file.OpenBinaryStream()));

                this.gridCategories = new GridView();
                this.gridCategories.DataSource =
                     from XElement x in ds.Descendants("category")
                     select new CategoryItem
                     {
                         Id = x.Attribute("id").Value.ToString(),
                         Description = x.Attribute("description").Value.ToString(),
                     };

                this.gridCategories.ShowHeader = true;
                this.gridCategories.AutoGenerateColumns = true;
                this.gridCategories.AutoGenerateSelectButton = true;
                this.gridCategories.DataKeyNames = new String[] { "Id" };
                this.gridCategories.SelectedRowStyle.BackColor = System.Drawing.Color.LightGray;
                this.Controls.Add(new LiteralControl("Products' Categories: <br/><br/>"));
                this.Controls.Add(this.gridCategories);
                this.gridCategories.DataBind();
            }
            else
            {
                this.Controls.Add(new LiteralControl("Please configure the Web Part data source URI"));
            }
        }


        private class CategoryItem
        {
            public String Id { get; set; }
            public String Description { get; set; }
        }
    }
}

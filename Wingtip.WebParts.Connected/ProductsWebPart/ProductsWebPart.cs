using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace Wingtip.WebParts.Connected.ProductsWebPart
{
    [ToolboxItemAttribute(false)]
    public class ProductsWebPart : WebPart
    {
        [WebBrowsable(true)]
        [Personalizable(true)]
        public String XmlDataSourceUri { get; set; }

        protected ICategoriesProvider _provider;
        protected GridView gridProducts;
        protected string categoryId;

        [ConnectionConsumer("Products of Category")]
        public void SetCategoryProvider(ICategoriesProvider categoriesProvider)
        {
            this._provider = categoriesProvider;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this._provider != null)
            {
                this.categoryId = this._provider.CategoryId;

                if (!String.IsNullOrEmpty(this.categoryId))
                {
                    this.EnsureChildControls();

                    SPWeb web = SPControl.GetContextWeb(HttpContext.Current);
                    SPFile file = web.GetFile(this.XmlDataSourceUri);

                    XElement ds = XElement.Load(XmlReader.Create(file.OpenBinaryStream()));

                    this.gridProducts.DataSource =
                        from XElement x in ds.Descendants("product")
                        where x.Attribute("categoryId").Value == this.categoryId
                        select new ProductItem
                        {
                            Code = x.Attribute("code").Value.ToString(),
                            Description = x.Attribute("description").Value.ToString(),
                            CategoryId = x.Attribute("categoryId").Value.ToString(),
                            Price = Decimal.Parse(x.Attribute("price").Value),
                        };
                    this.gridProducts.DataBind();
                }
                else
                {
                    this.Controls.Add(new LiteralControl("Please select a Product Category"));
                }
            }
            else
            {
                this.Controls.Add(new LiteralControl("Please connect this Web Part to a Categories Data Provider"));
            }

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            if (!String.IsNullOrEmpty(this.XmlDataSourceUri))
            {
                this.gridProducts = new GridView();
                this.gridProducts.ShowHeader = true;
                this.gridProducts.AutoGenerateColumns = true;
                this.Controls.Add(new LiteralControl("Products in current Category"));
                this.Controls.Add(this.gridProducts);
            }
            else
            {
                this.Controls.Add(new LiteralControl("Please configure the Web Part data source URI"));
            }
        }

        private class ProductItem
        {
            public String Code { get; set; }
            public String Description { get; set; }
            public String CategoryId { get; set; }
            public Decimal Price { get; set; }
        }

    }
}

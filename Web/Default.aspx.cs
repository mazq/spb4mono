using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Web
{
    public partial class _Default : Page
    {
        protected void Page_PreInit(object sender, System.EventArgs e)
        {
            int iisVersion = 6;
            int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion);
            if (iisVersion >= 7)
                Response.Redirect(Request.ApplicationPath);
            else
                Response.Redirect("~/Home.aspx");
        }
    }
}

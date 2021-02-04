using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Cookies.Get("ASP.NET_SessionId").Expires = DateTime.Now.AddDays(-7);
            Response.Cookies.Get("sessionGUID").Expires = DateTime.Now.AddDays(-7);
            Response.Cookies.Get("loggedIn").Expires = DateTime.Now.AddDays(-7);
            Response.Redirect("/");
        }
    }
}
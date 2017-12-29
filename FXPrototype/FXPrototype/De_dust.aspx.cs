using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class De_dust : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("Session ID is:" + Session.SessionID.ToString() + "<br/>");
        Response.Write("Session value is:" + Session["sid"].ToString());
    }
}

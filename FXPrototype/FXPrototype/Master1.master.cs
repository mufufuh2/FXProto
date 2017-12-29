using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master1 : System.Web.UI.MasterPage
{
    public String CompanyName
    {
        get { return (String)ViewState["companyName"]; }
        set { ViewState["companyName"] = value;  }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CompanyName = "TestCase";
    }
}

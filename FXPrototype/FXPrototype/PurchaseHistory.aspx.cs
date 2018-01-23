using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

public partial class PurchaseHistory : System.Web.UI.Page
{
    string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["xenmarketConnection"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        /*
         Wrap creating the table into a function so it
         can be called with arguments later on
         also allows us to change the trigger that activates
         table creation.
        */
        Create_Table();
    }

    protected void Create_Table()
    {
        MySqlConnection con = new MySqlConnection(conStr);


    }
}
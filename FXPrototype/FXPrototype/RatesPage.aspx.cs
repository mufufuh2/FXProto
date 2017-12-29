using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

public partial class RatesPage : System.Web.UI.Page
{
    string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["xenmarketConnection"].ConnectionString;
    string selection;

    protected void Page_Load(object sender, EventArgs e)
    {
        MySqlConnection con = new MySqlConnection(conStr);
        //RatesList.Items.Clear();
        //RatesList.Items.Add("Rates");
        try
        {
            System.Diagnostics.Debug.WriteLine("Connecting to MySQL");
            con.Open();

            string sqlSelect = "SELECT CurrencyCode,CurrencyName FROM Currency;";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sqlSelect;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr[0].ToString() != "IDR")
                {
                    string toAdd = rdr[0] + " - " + rdr[1];
                    RatesList.Items.Add(toAdd);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

        con.Close();
    }

    protected void ValidateSelect_Click(object sender, EventArgs e)
    {
        // Learn how to do Custom Validation for the real product
        // http://www.adamtibi.net/09-2008/the-three-steps-of-building-an-asp-net-validator-control
        // https://msdn.microsoft.com/en-us/library/bwd43d0x.aspx
        // https://msdn.microsoft.com/en-us/library/7kh55542.aspx
        selection = RatesList.SelectedItem.ToString();
        /*
        if (RatesList.SelectedItem.ToString() == "Rates")
        {
            //SelectRate.Text = "Please select a valid rate";
            this.valid = false;
            if (valid == false)
            {
                SelectRate.Text = "Please select a valid rate";
            }
        }*/
        /*
        else
        {
            // Reset the label for the next button press
            SelectRate.Text = "";
        }
        */

    }

    protected void RatesList_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(RatesList.SelectedItem.ToString());
        if (RatesList.SelectedItem.ToString() == "Rates")
        {
            SelectRate.Text = "Please select a valid rate";
        }
        else
        {
            SelectRate.Text = "";
        }
    }
}
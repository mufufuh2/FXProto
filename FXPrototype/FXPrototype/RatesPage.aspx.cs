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

    protected void Page_Load(object sender, EventArgs e)
    {
        MySqlConnection con = new MySqlConnection(conStr);
        //RatesList.Items.Clear();
        //RatesList.Items.Add("Rates");
        
        if (RatesList.Items.Count <= 1)
        {
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

    }

    /*
            <div class ="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="ValidateSelect_Click" Text="Check Rates" CssClass="btn btn-default" />
            </div>
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
        }
        /*
        else
        {
            // Reset the label for the next button press
            SelectRate.Text = "";
        }

    }
    */

    protected void RatesList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RatesList.SelectedItem.ToString() == "Rates")
        {
            SelectRate.Text = "Please select a valid rate";
            return;
        }
        else
        {
           // Reset the label for the next button press
           SelectRate.Text = "";
        }

        string[] selection = (RatesList.SelectedItem.ToString()).Split(new[] { " - " }, StringSplitOptions.None);

        MySqlConnection con = new MySqlConnection(conStr);

        TableHeaderRow th = new TableHeaderRow();
        TableHeaderCell th1 = new TableHeaderCell();
        th1.Text = "Company Name";
        TableHeaderCell th2 = new TableHeaderCell();
        th2.Text = "Rate";
        TableHeaderCell th3 = new TableHeaderCell();
        th3.Text = "Amount";
        th.Cells.Add(th1);
        th.Cells.Add(th2);
        th.Cells.Add(th3);
        RatesTable.Rows.Add(th);
        try
        {
            System.Diagnostics.Debug.WriteLine("Connecting to MySQL");
            con.Open();

            string sqlSelect = 
                "SELECT CompanyName, Rate, Quantity FROM Company " +
                "LEFT JOIN SellRates sr ON Company.CompanyID = sr.CompanyID " +
                "LEFT JOIN Currency cc ON sr.CurrencyID = cc.CurrencyID " +
                "LEFT JOIN Quantity ON Company.CompanyID = Quantity.CompanyID AND cc.CurrencyID = Quantity.CurrencyID " +
                "WHERE cc.CurrencyCode = '" + selection[0] + "' " +
                "ORDER BY cc.CurrencyID ASC, Company.CompanyID ASC;";
            //System.Diagnostics.Debug.WriteLine(sqlSelect);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sqlSelect;
            MySqlDataReader rdr = cmd.ExecuteReader();
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            while (rdr.Read())
            {
                tr = new TableRow();
                for (int i=0; i < rdr.VisibleFieldCount; i++)
                {
                    td.Text = rdr[i].ToString();
                    tr.Cells.Add(td);
                    td = new TableCell();
                }
                RatesTable.Rows.Add(tr);
            }


            con.Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
    
}
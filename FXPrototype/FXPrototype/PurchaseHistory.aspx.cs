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
        //string[] headersList = new string[6];
        List<string> headerList = new List<string>
        {
            "PurchaseID",
            "BaseCurrency",
            "Rate",
            "TradeCurrency",
            "Amount",
            "PurchasedOn"
        };
        Create_Table(headerList, headerList.Count);
    }

    protected void Create_Table(List<string> headerList, int numHeaders)
    {
        MySqlConnection con = new MySqlConnection(conStr);

        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thc = new TableHeaderCell();
        for (int i=0; i < numHeaders; i++)
        {
            thc.Text = headerList[i];
            thr.Cells.Add(thc);
            thc = new TableHeaderCell();
        }
        HistoryTable.Rows.Add(thr);

        try
        {
            con.Open();

            string sqlSelect =
                "SELECT PurchaseID, cc.CurrencyCode, Rate, c2.CurrencyCode, TotalAmount, PurchasedOn " +
                "FROM Purchase p LEFT JOIN Currency cc ON p.BaseCurrency = cc.CurrencyID " +
                                "LEFT JOIN Currency c2 ON p.TradeCurrency = c2.CurrencyID " +
                "ORDER BY PurchasedOn DESC;";

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sqlSelect;
            MySqlDataReader rdr = cmd.ExecuteReader();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            while (rdr.Read())
            {
                tr = new TableRow();
                for (int i = 0; i < rdr.VisibleFieldCount; i++)
                {
                    tc.Text = rdr[i].ToString();
                    tr.Cells.Add(tc);
                    tc = new TableCell();
                }
                HistoryTable.Rows.Add(tr);
            }

            con.Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("SQL ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
}
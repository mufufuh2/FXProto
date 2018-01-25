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

    // This Function is for testing only
    // Performs a similar Query call to AmountSelect_Click below
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
            //System.Diagnostics.Debug.WriteLine("Connecting to MySQL");
            con.Open();

            string sqlSelect = 
                "SELECT CompanyName, Rate, Quantity FROM Company " +
                "LEFT JOIN SellRates sr ON Company.CompanyID = sr.CompanyID " +
                "LEFT JOIN Currency cc ON sr.CurrencyID = cc.CurrencyID " +
                "LEFT JOIN Quantity ON Company.CompanyID = Quantity.CompanyID AND cc.CurrencyID = Quantity.CurrencyID " +
                "WHERE cc.CurrencyCode = '" + selection[0] + "' " +
                "ORDER BY Rate ASC, Quantity DESC;";

            /*
            Considerations for ordering the priority of company selections
            - Lowest Rates
            - Amount Available
            - Location/Address (Lat,Lon)
            - Company Ratings
            All of these encompass risk and cost of the total package
            */

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
            System.Diagnostics.Debug.WriteLine("SQL ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }


    /*  TO-DO:
     *  - Proper BuyerID system (maybe as a property of the page?)
     */
    protected void AmountSelect_Click(object sender, EventArgs e)
    {
        // Learn how to do Custom Validation for the real product
        // http://www.adamtibi.net/09-2008/the-three-steps-of-building-an-asp-net-validator-control
        // https://msdn.microsoft.com/en-us/library/bwd43d0x.aspx
        // https://msdn.microsoft.com/en-us/library/7kh55542.aspx

        int purchaseAmount = 0;
        bool converted = int.TryParse(Amount.Text, out purchaseAmount);

        if (purchaseAmount <= 0 || converted == false)
        {
            SelectRate.Text = "Invalid amount entered!";
            return;
        }

        if (RatesList.SelectedItem.ToString() == "Rates")
        {
            SelectRate.Text = "Please select a valid rate ";
            return;
        }

        string[] selection = (RatesList.SelectedItem.ToString()).Split(new[] { " - " }, StringSplitOptions.None);

        MySqlConnection con = new MySqlConnection(conStr);

        try
        {
            con.Open();

            string sqlSelect =
                "SELECT CompanyID, Rate, Quantity, cc.CurrencyID FROM Company " +
                "LEFT JOIN SellRates sr ON Company.CompanyID = sr.CompanyID " +
                "LEFT JOIN Currency cc ON sr.CurrencyID = cc.CurrencyID " +
                "LEFT JOIN Quantity ON Company.CompanyID = Quantity.CompanyID AND cc.CurrencyID = Quantity.CurrencyID " +
                "WHERE cc.CurrencyCode = '" + selection[0] + "' " +
                "ORDER BY Rate ASC, Quantity DESC;";

            /*
            Considerations for ordering the priority of company selections
            - Lowest Rates
            - Amount Available
            - Location/Address (Lat,Lon)
            - Company Ratings
            All of these encompass risk and cost of the total package
            */

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sqlSelect;
            MySqlDataReader rdr = cmd.ExecuteReader();

            int countVendors = 0;
            int currentTotal = 0;
            int intermediate = 0;
            int tradeID = 0;
            List<int> vendorIDs = new List<int>();
            List<double> rates = new List<double>();
            List<int> amounts = new List<int>();

            while (rdr.Read())
            {
                countVendors++;
                vendorIDs.Add(int.Parse(rdr[0].ToString()));
                rates.Add(double.Parse(rdr[1].ToString()));
                intermediate = int.Parse(rdr[2].ToString());

                if (intermediate >= (purchaseAmount-currentTotal))
                {
                    intermediate = purchaseAmount - currentTotal;
                    amounts.Add(intermediate);
                    tradeID = int.Parse(rdr[3].ToString());
                    break;
                }
                amounts.Add(intermediate);
            }
            con.Close();

            double avgRate = AverageRate_Calc(rates, amounts, countVendors);

            // Temporary IDR currency ID for use as the base currency
            int idrCurrencyID = 11;

            /* Currently, InsertPurchases is inserting both to purchases and to sale
             * if necessary or if there is a bug caused by this, it will be split
             * into individual purchase and sale functions
             */
            InsertPurchases(vendorIDs, rates, amounts, avgRate, countVendors, idrCurrencyID, tradeID);
            // INSERT rates and amounts to database here
            // UPDATE individual vendor quantities here
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("SQL ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }

    }

    public static double AverageRate_Calc(List<double> rates, List<int> quantities, int count)
    {
        int total = quantities.Sum();
        double average = 0.0;

        for (int i=count; i>0; i--)
        {
            average += (rates[i] * quantities[i]);
        }

        average = average / total;
        
        return average;
    }

    protected void InsertPurchases (List<int> IDs, List<double> rates, List<int> quantities, double average, int vendorCount, int baseID, int tradeID)
    {
        MySqlConnection con = new MySqlConnection(conStr);
        MySqlTransaction tr = null;
        try
        {
            con.Open();
            tr = con.BeginTransaction();

            MySqlCommand cmd = new MySqlCommand()
            {
                Connection = con,
                Transaction = tr,
            };

            string[] selection = (RatesList.SelectedItem.ToString()).Split(new[] { " - " }, StringSplitOptions.None);
            // Find a way to get the CurrencyID from here...
            // Slightly annoying but you might need to do another query here

            // DONE, queried the currency ID in previous SQL statement
            // and passed as parameter in this function
            // baseCurrency is currently set to IDR's CurrencyID by default

            // Once buyer ID can be passed properly to the page, we will use it here
            // for now buyerID is temporarily set to user 3
            int buyerID = 3;

            int total = quantities.Sum();
            string sqlInsertPurchase = GenerateSQLPurchaseString(buyerID, baseID, average, tradeID, total);

            cmd.CommandText = sqlInsertPurchase;
            cmd.ExecuteNonQuery();

            //Find most recent purchase ID here
            int purchaseID = 0;
            string getPurchaseID = "SELECT MAX(PurchaseID) FROM `Purchase`;";
            MySqlCommand getIDcmd = new MySqlCommand(getPurchaseID, con);
            object result = getIDcmd.ExecuteScalar();
            if (result != null)
            {
                purchaseID = Convert.ToInt32(result);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to execute Scalar calculation in SQL");
            }

            string sqlInsertSale = GenerateSQLSaleString(vendorCount, IDs, purchaseID, baseID, rates, tradeID, quantities);

            cmd.CommandText = sqlInsertSale;
            cmd.ExecuteNonQuery();

            //tr.Commit();
        }
        catch (Exception ex)
        {
            try
            {
                tr.Rollback();
            }
            catch (MySqlException ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1.ToString());
            }
            System.Diagnostics.Debug.WriteLine("SQL ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        con.Close();
    }

    private string GenerateSQLPurchaseString(int buyerID, int baseCurrID, double rate, int tradeCurrID, int quantity)
    {
        string sqlInsertPurchase = "INSERT INTO `Purchase` (BuyerID, BaseCurrency, Rate, TradeCurrency, TotalAmount, PurchasedOn) VALUES";
        sqlInsertPurchase += "(";
        sqlInsertPurchase += "`" + buyerID + "`,";
        sqlInsertPurchase += "`" + baseCurrID + "`,";
        sqlInsertPurchase += "`" + rate + "`,";
        sqlInsertPurchase += "`" + tradeCurrID + "`,";
        sqlInsertPurchase += "`" + quantity + "`,";
        sqlInsertPurchase += "NOW()";
        sqlInsertPurchase += ");";

        return sqlInsertPurchase;
    }

    private string GenerateSQLSaleString(int vendorCount, List<int> companyID, int purchaseID, int baseCurrency, List<double> ratesList, int tradeCurrency, List<int> quantities)
    {
        string sqlInsertSale = "INSERT INTO `Sale` (SellerID, PurchaseID, BaseCurrency, Rate, TradeCurrency, Amount, PurchasedOn) VALUES";

        for (int i = vendorCount; i > 0; i--)
        {
            sqlInsertSale += "(";
            sqlInsertSale += "`" + companyID[i] + "`,";
            sqlInsertSale += "`" + purchaseID + "`,";
            sqlInsertSale += "`" + baseCurrency + "`,";
            sqlInsertSale += "`" + ratesList[i] + "`,";
            sqlInsertSale += "`" + tradeCurrency + "`,";
            sqlInsertSale += "`" + quantities[i] + "`,";
            sqlInsertSale += "NOW()";

            if (i == 1) { sqlInsertSale += ");"; }
            else { sqlInsertSale += "),"; }
        }

        return sqlInsertSale;
    }

}
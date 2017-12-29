using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using WebSite1;

using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

public partial class Account_Register : Page
{
    string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["xenmarketConnection"].ConnectionString;

    protected void CreateUser_Click(object sender, EventArgs e)
    {
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = UserName.Text };
        IdentityResult result = manager.Create(user, Password.Text);
        if (result.Succeeded)
        {
            IdentityHelper.SignIn(manager, user, isPersistent: false);
            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        }
        else
        {
            ErrorMessage.Text = result.Errors.FirstOrDefault();
        }
    }
    protected void Test_Click(object sender, EventArgs e)
    {
        MySqlConnection con = new MySqlConnection(conStr);
        try
        {
            //System.Diagnostics.Debug.WriteLine("Connecting to MySQL");
            con.Open();

            string sqlSelect = "SELECT * FROM Account";
            MySqlCommand cmd = new MySqlCommand(sqlSelect, con);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                
                for (int i = 1; i < rdr.VisibleFieldCount; i++)
                {
                    System.Diagnostics.Debug.Write(rdr[i] + " ");
                }
                
                System.Diagnostics.Debug.WriteLine("");
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        con.Close();
    }
    protected void NewUser_Click(object sender, EventArgs e)
    {
        MySqlConnection con = new MySqlConnection(conStr);
        MySqlTransaction tr = null;
        try
        {
            //System.Diagnostics.Debug.WriteLine("Connecting to MySQL");
            con.Open();
            tr = con.BeginTransaction();

            MySqlCommand cmd = new MySqlCommand()
            {
                Connection = con,
                Transaction = tr
            };
            string sqlInsertUser = "INSERT INTO `Account`(Username,Password,FirstName,LastName,CompanyID) VALUES (";
            sqlInsertUser += "'"+UserName.Text + "', ";
            sqlInsertUser += "'"+Password.Text + "', ";
            sqlInsertUser += "'"+FirstName.Text + "', ";
            sqlInsertUser += "'"+LastName.Text + "', NULL);";

            cmd.CommandText = sqlInsertUser;

            cmd.ExecuteNonQuery();

            tr.Commit();
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
            System.Diagnostics.Debug.WriteLine("ERROR");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
        con.Close();
    }
}

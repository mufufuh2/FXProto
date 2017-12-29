<%@ Page Language="C#" %>

<!DOCTYPE html>

<script runat="server">
/*
    protected void Signup_Click(object sender, EventArgs e)
    {

    }
*/
</script>
<style>
    input[type=text]{
        color:white;
        background-color:rgba(57, 57, 57, 0.92);
        margin-top:5px;
        margin-bottom:5px;
    }
</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hello World</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Hello...
            <br />
            <br />
            <br />
            Enter your username:<br />
            <asp:TextBox ID="Username" runat="server"></asp:TextBox><br />
            Enter desired password:<br />
            <asp:TextBox ID="Password" runat="server"></asp:TextBox><br />
            Repeat password:<br />
            <asp:TextBox ID="Password2" runat="server"></asp:TextBox><br />
            First Name:<br />
            <asp:TextBox ID="FirstName" runat="server"></asp:TextBox><br />
            Last Name:<br />
            <asp:TextBox ID="LastName" runat="server"></asp:TextBox><br />
            Company Name (if present):<br />
            <asp:TextBox ID="CompanyName" runat="server"></asp:TextBox><br />
            <br />
            <br />
            <a runat="server" href="~/">Home</a>
            <br />
            <a runat="server" href="~/Tefault.aspx">Test</a>
        </div>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="FXPrototype" AutoGenerateColumns="False" DataKeyNames="AccountID">
            <Columns>
                <asp:BoundField DataField="AccountID" HeaderText="AccountID" InsertVisible="False" ReadOnly="True" SortExpression="AccountID" />
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" />
                <asp:BoundField DataField="CompanyID" HeaderText="CompanyID" SortExpression="CompanyID" />
                <asp:BoundField DataField="AddressName" HeaderText="AddressName" SortExpression="AddressName" />
                <asp:BoundField DataField="Street1" HeaderText="Street1" SortExpression="Street1" />
                <asp:BoundField DataField="PostCode" HeaderText="PostCode" SortExpression="PostCode" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="FXPrototype" runat="server" ConnectionString="<%$ ConnectionStrings:xenmarketConnection %>" ProviderName="<%$ ConnectionStrings:xenmarketConnection.ProviderName %>" SelectCommand="SELECT ac.AccountID, Username, FirstName, LastName, CompanyID, AddressName, Street1, PostCode
	FROM Account ac	LEFT JOIN UserAddress ud ON ac.AccountID = ud.AccountID
					LEFT JOIN Address ad ON ad.AddressID = ud.AddressID;"></asp:SqlDataSource>
    </form>
</body>
</html>

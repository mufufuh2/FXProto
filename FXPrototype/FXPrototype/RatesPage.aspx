<%@ Page Title="RatesPage" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RatesPage.aspx.cs" Inherits="RatesPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">
        select {
            width: 200px;
            border: 2px solid #7d6754;
            border-radius: 5px;
            padding: 3px;
            -webkit-appearance: none;
            background-image: url(http://localhost:60649/Nggfj.png);
            background-position: 160px;
            background-repeat: no-repeat;
            text-indent: 0.01px;
            text-overflow: '';
        }
        .warning{
            color: red;
        }

    </style>
    <h2>Check our available Rates here</h2>
    <h5>Our rates are calculated against IDR only (for now...)</h5>
    <hr />
    <div class ="form-horizontal">
        <h4>Please select a rate</h4>
            <asp:DropDownList ID="RatesList" runat="server" OnSelectedIndexChanged="RatesList_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem>Rates</asp:ListItem>
            </asp:DropDownList>
        <div class="form-group">
            <asp:Label runat="server" ID="SelectRate" CssClass="col-md-2 control-label warning"></asp:Label>
        </div>
    </div>
    <asp:Table ID="RatesTable" runat="server" CssClass="table table-hover table-bordered">
    </asp:Table>
</asp:Content>


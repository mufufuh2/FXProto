<%@ Page Title="PurchaseHistory" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PurchaseHistory.aspx.cs" Inherits="PurchaseHistory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">

    </style>
    <h2>Check your purchase history</h2>
    <h5>Each purchase comes from several vendors</h5>
    <hr />
    <div class="row">
        <div class="col-sm-4">
            <h4>Might add purchase/order status at a later date</h4>
        </div>
        <div class="col-sm-4">
            <h4>Also add filters such as date, amount, currency</h4>
        </div>
    </div>
    <br />
    <asp:Table ID="HistoryTable" runat="server" CssClass="table table-hover table-bordered">
    </asp:Table>
</asp:Content>


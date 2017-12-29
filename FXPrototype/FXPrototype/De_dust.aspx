<%@ Page Title="De_dust" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="De_dust.aspx.cs" Inherits="De_dust" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">
        .jumbotron {
            background:url(/crypto-currency-exchange.jpg);
        }
        .textBox {
            color: white;
            background: rgba(0, 0, 0, 0.55);
        }
        .btn-lg {
            background-color: #7a9469 !important;
        }
        .btn-default {
            background: #7a9469 !important;
            color: white;
        }
    </style>
    <div class="jumbotron">
        <div class="textBox">
            <h1>Sound FX rates,</h1><h1>all in one place!</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <h3>Discover our rates</h3>
            <p>We pool rates from all our partners to provide you with the best</p>
            <a class="btn btn-default" runat="server" href="~/RatesPage.aspx" >Learn more &raquo;</a>
        </div>
        <div class="col-md-4">
            <h3>Become a Partner</h3>
            <p>Join our growing list of partner vendors and reach out to a wider audience</p>
            <a class="btn btn-default" runat="server" href="~/CompanyRate.aspx">Learn more &raquo;</a>
        </div>
    </div>

</asp:Content>
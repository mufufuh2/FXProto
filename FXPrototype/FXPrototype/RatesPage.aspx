<%@ Page Title="RatesPage" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RatesPage.aspx.cs" Inherits="RatesPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">
        select {
            width: 200px;
            border: 2px solid #7d6754;
            border-radius: 5px;
            padding: 3px;
            -webkit-appearance: none;
            background-image: url(http://localhost:52082/Nggfj.png);
            background-position: 160px;
            background-repeat: no-repeat;
            text-indent: 0.01px;
            text-overflow: '';
        }
        input.ratestext {
             border: 2px solid #7d6754;
             border-radius: 5px;
             padding: 3px;
             margin-top: 5px;
        }
        .buttontext{
            margin-top: 13px;
        }
        .btn-default {
            background-color: #7a9469 !important;
            color: white;
        }
    </style>
    <h2>Check our available rates here</h2>
    <h5>Our rates are calculated against IDR only (for now...)</h5>
    <hr />
    <div class="row">
        <div class="col-sm-4">
        <h4>Please select a rate</h4>
        <asp:DropDownList ID="RatesList" runat="server" OnSelectedIndexChanged="RatesList_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem>Rates</asp:ListItem>
        </asp:DropDownList> &nbsp;
        <asp:Label runat="server" ID="SelectRate" CssClass="text-danger"></asp:Label>
        </div>
        <div class="col-sm-4">
            <div class ="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="Amount" CssClass="control-label">Amount</asp:Label>
                    <asp:TextBox runat="server" ID="Amount" CssClass="form-control ratestext" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Amount"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Please enter an amount to purchase" />
                </div>
            </div>
        </div>
        <div class ="col-sm-4">
            <br />
            <div class="form-group buttontext">
                <asp:Button runat="server" OnClick="AmountSelect_Click" Text="Purchase" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
    <br />
    <asp:Table ID="RatesTable" runat="server" CssClass="table table-hover table-bordered">
    </asp:Table>
</asp:Content>


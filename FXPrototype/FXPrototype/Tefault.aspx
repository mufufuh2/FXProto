<%@ Page Title="TestHomePage" Language="C#" MasterPageFile="~/Master1.master" AutoEventWireup="true" CodeFile="Tefault.aspx.cs" Inherits="Tefault" %>
<%@ MasterType virtualpath="~/Master1.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <h1>Welcome to the site of
        <asp:Label ID="CompanyName" runat="server" Text=""></asp:Label>
    </h1>
    <p>Thank you</p>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneArticoli.aspx.cs" Inherits="VideoSystemWeb.Articoli.GestioneArticoli" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HF_TIPO_ARTICOLO" Value="" runat="server" />
    <asp:PlaceHolder runat="server" ID="PH"></asp:PlaceHolder>
</asp:Content>
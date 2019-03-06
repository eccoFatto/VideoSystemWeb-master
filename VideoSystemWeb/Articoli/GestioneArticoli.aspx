<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneArticoli.aspx.cs" Inherits="VideoSystemWeb.Articoli.GestioneArticoli" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="tipo" TagName="ArtTipo" Src="~/Articoli/userControl/ArtTipologie.ascx" %>
<%@ Register TagPrefix="art" TagName="ArtArt" Src="~/Articoli/userControl/ArtArticoli.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HF_TIPO_ARTICOLO" Value="" runat="server" />
    <asp:PlaceHolder runat="server" ID="PH">

        <tipo:ArtTipo id="controlTipo" runat="server" Visible="false"></tipo:ArtTipo>
        <art:ArtArt id="controlArt" runat="server" Visible="false"></art:ArtArt>

    </asp:PlaceHolder>
</asp:Content>
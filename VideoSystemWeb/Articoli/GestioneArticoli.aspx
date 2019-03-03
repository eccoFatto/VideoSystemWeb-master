<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneArticoli.aspx.cs" Inherits="VideoSystemWeb.Articoli.GestioneArticoli" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="articoli" TagName="gestArticoli" Src="~/Articoli/userControl/ArtTipologie.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HF_TIPO_ARTICOLO" Value="" runat="server" />
<articoli:gestArticoli id="controlAziende" runat="server"></articoli:gestArticoli>
</asp:Content>
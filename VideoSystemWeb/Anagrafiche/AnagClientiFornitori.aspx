<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnagClientiFornitori.aspx.cs" Inherits="VideoSystemWeb.Anagrafiche.AnagClientiFornitori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="aziende" TagName="AnagAziende" Src="~/Anagrafiche/userControl/AnagClientiFornitori.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HF_TIPO_AZIENDA" Value="" runat="server" />
<aziende:AnagAziende id="controlAziende" runat="server"></aziende:AnagAziende>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnagCollaboratori.aspx.cs" Inherits="VideoSystemWeb.Anagrafiche.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="coll" TagName="Anagcollaboratori" Src="~/Anagrafiche/userControl/AnagCollaboratori.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<coll:Anagcollaboratori id="controlCollab" runat="server"></coll:Anagcollaboratori>
</asp:Content>
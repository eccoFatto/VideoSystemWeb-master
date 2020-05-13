<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReportCollaboratoriPerGiornataNew.aspx.cs" Inherits="VideoSystemWeb.REPORT.ReportCollaboratoriPerGiornataNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="repcoll" TagName="ReportCollaboratori" Src="~/REPORT/userControl/collaboratoriPerGiornata.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<repcoll:ReportCollaboratori id="controlCollab" runat="server"></repcoll:ReportCollaboratori>
</asp:Content>
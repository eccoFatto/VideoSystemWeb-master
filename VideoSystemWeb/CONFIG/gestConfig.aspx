<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="gestConfig.aspx.cs" Inherits="VideoSystemWeb.CONFIG.gestConfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <label>
        <asp:Label ID="lblProtocolli" runat="server" Text="CONFIGURAZIONE" ForeColor="Teal"></asp:Label>
    </label>
    <div class="w3-card-4">
        <asp:PlaceHolder runat="server" ID="phCampiConfig">

        </asp:PlaceHolder>
    </div>
</asp:Content>
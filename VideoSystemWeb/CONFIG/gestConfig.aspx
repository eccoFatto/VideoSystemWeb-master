<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="gestConfig.aspx.cs" Inherits="VideoSystemWeb.CONFIG.gestConfig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <label>
        <asp:Label ID="lblConfigurazione" runat="server" Text="CONFIGURAZIONE" ForeColor="Teal"></asp:Label>
    </label>
    <div class="w3-panel w3-teal w3-center w3-round">
        <h5 class="w3-text-white" style="text-shadow:1px 1px 0 #444"> <b>GESTIONE CONFIGURAZIONE</b> </h5>
    </div>                        
    
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <div class="w3-card-4">
            <asp:PlaceHolder runat="server" ID="phCampiConfig">

            </asp:PlaceHolder>
        </div>
        <div style="text-align: center;">
            <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnModifica_Click" />
            <asp:Button ID="btnSalva" runat="server" Text="Salva" class="w3-panel w3-green w3-border w3-round" OnClick="btnSalva_Click" OnClientClick="return confirm('Confermi salvataggio modifiche?')" Visible="false"/>
            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false"/>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
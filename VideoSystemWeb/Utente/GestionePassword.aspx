<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionePassword.aspx.cs" Inherits="VideoSystemWeb.Utente.GestionePassword" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="/Css/spectre.min.css" />
    <link rel="stylesheet" href="/Css/spectre-exp.min.css" />
    <link rel="stylesheet" href="/Css/spectre-icons.min.css" />
    <script>
        $(document).ready(function () {
            $('.loader').hide();
        });

    </script>
    <Label><asp:Label ID="lbliInfoPagina" runat="server" Text="GESTIONE PASSWORD"></asp:Label></Label>
    <asp:UpdatePanel ID="UpdatePanelGestionePassword" runat="server">
        <ContentTemplate> 
            <div class="container">
                <div class="columns">
                    <div class="column col-2"></div>
                    <div class="column col-8">
                        <div class="w3-0 mydiv">
                            <div id="panelErrore" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
                                <span onclick="this.parentElement.style.display='none'"
                                class="w3-button w3-large w3-display-topright">&times;</span>
                                <p><asp:Label ID="lbl_MessaggioErrore" runat="server" ></asp:Label></p>
                            </div>
                            <div class="w3-container w3-card-4 w3-margin-bottom w3-padding-32">
                                <p>
                                    <label>Vecchia Password</label>
                                    <asp:TextBox ID="tbOldPassword" runat="server" TextMode="Password" class="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                                </p>
                                <p>
                                    <label>Nuova Password</label>
                                    <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" class="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                                </p>
                                <p>
                                    <label>Conferma Nuova Password</label>
                                    <asp:TextBox ID="tbConfirmNewPassword" runat="server" TextMode="Password" class="w3-input w3-border w3-round" Style="width: 99%" required="required"></asp:TextBox>
                                </p>
                                <p>
                                    <asp:Button ID="btnConfermaCambioPwd" runat="server" OnClick="btnConfermaCambioPwd_Click" class="w3-btn w3-section w3-teal w3-ripple" Text="Conferma Cambio"></asp:Button>
                                </p>
                                <p>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="column col-2"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
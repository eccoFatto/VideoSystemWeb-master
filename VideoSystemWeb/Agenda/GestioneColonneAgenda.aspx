<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="GestioneColonneAgenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.GestioneColonneAgenda" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script>
    $(document).ready(function () {
        $('.loader').hide();
    });

</script>
<Label><asp:Label ID="lblColonneAgenda" runat="server" Text="COLONNE AGENDA"></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <div id="panelErrore" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
            <span onclick="this.parentElement.style.display='none'"
            class="w3-button w3-large w3-display-topright">&times;</span>
            <p><asp:Label ID="lbl_MessaggioErrore" runat="server" ></asp:Label></p>
        </div>
        <div class="w3-container w3-center" style="width:75%;">
            <div class="w3-row-padding w3-center w3-text-center">
                <div class="w3-threequarter">
                    <asp:DropDownList ID="lbMod_Tipologia" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" ></asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <asp:Button ID="btnSeleziona" runat="server" Text="Seleziona" class="w3-panel w3-green w3-border w3-round" OnClick="btnSeleziona_Click" />
                </div>
            </div>
        </div>
        <div class="w3-panel w3-center">
            <div class="w3-row-padding w3-center w3-text-center">
                <div class="w3-half">
                    <label>Nome Colonna</label>
                    <asp:TextBox ID="tbInsNomeTipologia" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                </div>
                <div class="w3-half">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbInsDescrizioneTipologia" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding w3-center w3-text-center">
                <div class="w3-third">
                    <label>Tipo Colonna</label>
                    <asp:DropDownList ID="cmbInsSottotipoTipologia" runat="server" class="w3-input w3-border">
                        <asp:ListItem Value="dipendenti">Dipendenti</asp:ListItem>
                        <asp:ListItem Value="regie">Regie</asp:ListItem>
                        <asp:ListItem Value="extra">Extra</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="w3-third">
                    <label>Colore Colonna</label>
                    <asp:DropDownList ID="cmbInsParametriTipologia" runat="server" class="w3-input w3-border">
                        <asp:ListItem Value="COLOR=#ffa500">ARANCIONE</asp:ListItem>
                        <asp:ListItem Value="COLOR=#add8e6">AZZURRO</asp:ListItem>
                        <asp:ListItem Value="COLOR=#ffffff">BIANCO</asp:ListItem>
                        <asp:ListItem Value="COLOR=#9991CC">BLU</asp:ListItem>
                        <asp:ListItem Value="COLOR=#ffff00">GIALLO</asp:ListItem>
                        <asp:ListItem Value="COLOR=#800000">MARRONE</asp:ListItem>
                        <asp:ListItem Value="COLOR=#D7868E">ROSSO</asp:ListItem>
                        <asp:ListItem Value="COLOR=#89D38D">VERDE</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID ="tbIdTipologiaDaModificare"  runat="server" Visible ="false"></asp:TextBox>
                </div>
                <div class="w3-third">
                    <label>Ordinamento</label>
                    <asp:TextBox ID="tbInsOrdinamento" class="w3-input w3-border" runat="server" MaxLength="2" Text="1"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="tbInsOrdinamento_MaskedEditExtender" runat="server" TargetControlID="tbInsOrdinamento" MaskType="None" Mask="99" ></ajaxToolkit:MaskedEditExtender>
                </div>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnInserisciTipologia" runat="server" Text="Inserisci Colonna" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoTipologia_Click" OnClientClick="return confirm('Confermi inserimento Tipologia?')" />
                <asp:Button ID="btnModificaTipologia" runat="server" Text="Modifica Colonna" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaTipologia_Click" OnClientClick="return confirm('Confermi modifica Tipologia?')" Visible="false" />
                <asp:Button ID="btnEliminaTipologia" runat="server" Text="Elimina Colonna" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaTipologia_Click" OnClientClick="return confirm('Confermi eliminazione Tipologia?')" Visible="false" />
                <asp:Button ID="btnAnnullaTipologia" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaTipologia_Click" />
            </div>
            
            
        </div>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArtSottogruppi.ascx.cs" Inherits="VideoSystemWeb.Articoli.userControl.ArtSottogruppi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script>
    $(document).ready(function () {
        $('.loader').hide();
    });

</script>
<Label><asp:Label ID="lblTipoArticolo" runat="server" Text=""></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 

        <div class="w3-container w3-center" style="width:50%;">
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
                <div class="w3-col" style="width:20%">
                    <label>Nome</label>
                    <asp:TextBox ID="tbInsNomeTipologia" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                </div>
                <div class="w3-col" style="width:20%">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbInsDescrizioneTipologia" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                </div>
                <div class="w3-col" style="width:20%">
                    <label>Gruppo</label>
                    <asp:DropDownList ID="ddl_Gruppo" runat="server" class="w3-input w3-border " ReadOnly="true" Width="99%" ></asp:DropDownList>
                </div>
                <div class="w3-col" style="width:20%">
                    <label>Sottotipo</label>
                    <asp:TextBox ID="tbInsSottotipoTipologia" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                </div>
                <div class="w3-col" style="width:20%">
                    <label>Parametri</label>
                    <asp:TextBox ID="tbInsParametriTipologia" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                    <asp:TextBox ID ="tbIdTipologiaDaModificare"  runat="server" Visible ="false"></asp:TextBox>
                </div>
                
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnInserisciTipologia" runat="server" Text="Inserisci Tipologia" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoTipologia_Click" OnClientClick="return confirm('Confermi inserimento Tipologia?')" />
                <asp:Button ID="btnModificaTipologia" runat="server" Text="Modifica Tipologia" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaTipologia_Click" OnClientClick="return confirm('Confermi modifica Tipologia?')" Visible="false" />
                <asp:Button ID="btnEliminaTipologia" runat="server" Text="Elimina Tipologia" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaTipologia_Click" OnClientClick="return confirm('Confermi eliminazione Tipologia?')" Visible="false" />
                <asp:Button ID="btnAnnullaTipologia" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaTipologia_Click" />
            </div>
            
            
        </div>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>

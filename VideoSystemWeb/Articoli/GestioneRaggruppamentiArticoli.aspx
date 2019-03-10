<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="GestioneRaggruppamentiArticoli.aspx.cs" Inherits="VideoSystemWeb.Articoli.GestioneRaggruppamentiArticoli" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script>
    $(document).ready(function () {
        $('.loader').hide();
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA RAGGRUPPAMENTO
    function mostraRaggruppamento() {
        $('.loader').show();
        //$("#<%=hf_idRagg.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditRaggruppamento.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO RAGGRUPPAMENTO
    function inserisciRaggruppamento() {
        $('.loader').show();
        $("#<%=hf_idRagg.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsRaggruppamento.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO RAGGRUPPAMENTO
    function openDettaglioRaggruppamento(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            var i;
            var x = document.getElementsByClassName("ragg");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";  
            }
            document.getElementById(tipoName).style.display = "block";  
        }
    }

    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA RAGGRUPPAMENTO E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Raggruppamento');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        pannelloPopup.style.display = "none";
    }
</script>
<Label><asp:Label ID="lblRaggruppamenti" runat="server" Text="RAGGRUPPAMENTI ARTICOLI" ForeColor="Teal"></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <div id="panelErrore" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
            <span onclick="this.parentElement.style.display='none'"
            class="w3-button w3-large w3-display-topright">&times;</span>
            <p><asp:Label ID="lbl_MessaggioErrore" runat="server" ></asp:Label></p>
        </div>
        <div class="w3-cell-row" style="width:75%">
            <div class="w3-container w3-cell w3-cell-middle">
                <asp:ListBox ID="lbMod_Raggruppamenti" runat="server" class="w3-input w3-border w3-margin" Width="99%" ></asp:ListBox>
            </div>
            <div class="w3-container w3-cell w3-cell-middle">
                <div id="divSelRaggruppamento" runat="server"> 
                    <div id="selRaggruppamento" class="w3-btn w3-border w3-green w3-round w3-margin" onclick="mostraRaggruppamento();" style="width:100px">Seleziona</div>
                </div>
                <div id="divBtnInserisciRaggruppamento" runat="server"> 
                    <div id="insRaggruppamento" class="w3-btn w3-border w3-green w3-round w3-margin" onclick="inserisciRaggruppamento();" style="width:100px">Inserisci</div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>

<asp:Button runat="server" ID="btnEditRaggruppamento" Style="display: none" OnClick="btnEditRaggruppamento_Click"/>
<asp:Button runat="server" ID="btnInsRaggruppamento" Style="display: none" OnClick="btnInsRaggruppamento_Click"/>

<asp:HiddenField ID="hf_idRagg" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Raggruppamento" />


<asp:UpdatePanel ID="upColl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-container w3-center w3-xlarge">
                    GESTIONE RAGGRUPPAMENTI
                </div>
                <br />
                
                <!-- DIV MESSAGGI DI ERRORE -->        
                <div id="Div1" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
                  <span onclick="this.parentElement.style.display='none'"
                  class="w3-button w3-large w3-display-topright">&times;</span>
                  <p><asp:Label ID="Label1" runat="server" ></asp:Label></p>
                </div>                             
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI COLLABORATORE -->
                    <div class="w3-bar w3-yellow w3-round">
                        <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioRaggruppamento('Raggruppamento')">Raggruppamento</div>
                        <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioRaggruppamento('Articoli')">Articoli</div>
                        <div class="w3-bar-item w3-button w3-yellow w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
                    </div>
                </div>
                    <!-- TAB RAGGRUPPAMENTI -->
                    <div id="Raggruppamento" class="w3-container w3-border ragg" style="display:block" >
                        <label>Raggruppamento</label>
                        <div class="w3-container w3-center">
                            <p>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-half">
                                        <label>Nome Raggruppamento</label>
                                        <asp:TextBox ID="tbInsNomeRaggruppamento" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-half">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbInsDescrizioneRaggruppamento" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID ="tbIdRaggruppamentoDaModificare"  runat="server" Visible ="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnInserisciRaggruppamento" runat="server" Text="Inserisci Ragruppamento" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoRaggruppamento_Click" OnClientClick="return confirm('Confermi inserimento Raggruppamento?')" />
                                    <asp:Button ID="btnModificaRaggruppamento" runat="server" Text="Modifica Raggruppamento" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaRaggruppamento_Click" OnClientClick="return confirm('Confermi modifica Raggruppamento?')" Visible="false" />
                                    <asp:Button ID="btnEliminaRaggruppamento" runat="server" Text="Elimina Raggruppamento" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaRaggruppamento_Click" OnClientClick="return confirm('Confermi eliminazione Raggruppamento?')" Visible="false" />
                                    <%--<asp:Button ID="btnAnnullaRaggruppamento" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaRaggruppamento_Click" />--%>
                                </div>
                            </p>
                        </div>
                    </div>
                <!-- TAB ARTICOLI -->
                    <div id="Articoli" class="w3-container w3-border ragg" style="display:none">
                        <label>Articoli</label>
                        <asp:ListBox ID="lbMod_Articoli" runat="server" class="w3-input w3-border " Rows="3" ></asp:ListBox>
                        <div class="w3-container w3-center">
                            <asp:Button ID="btnApriArticoli" runat="server" OnClick="btnApriArticoli_Click" Text="Gestione Articoli" class="w3-panel w3-green w3-border w3-round" />
                            <asp:PlaceHolder ID="phArticoli" runat="server" Visible="false">                                
                                <div class="w3-row-padding w3-center w3-text-center" style="width:50%;">
                                    <label>Selezione Gruppi da aggiungere</label>
                                    <asp:DropDownList ID="ddlArticoliDaAggiungere" runat="server" AutoPostBack="false" Width="100%" class="w3-input w3-border">
                                    </asp:DropDownList>                                                
                                </div>
                                <asp:Button ID="btnInserisciArticolo" runat="server" Text="Inserisci Articolo" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciArticolo_Click" OnClientClick="return confirm('Confermi inserimento Articolo?')" />
                                <asp:Button ID="btnEliminaArticolo" runat="server" Text="Elimina Articolo" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaArticolo_Click" OnClientClick="return confirm('Confermi eliminazione Articolo?')" />
                            </asp:PlaceHolder>
                        </div>
                    </div>

            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

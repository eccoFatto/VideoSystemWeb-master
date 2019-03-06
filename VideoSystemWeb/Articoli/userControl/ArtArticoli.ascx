<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArtArticoli.ascx.cs" Inherits="VideoSystemWeb.Articoli.userControl.ArtArticoli" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script>
    $(document).ready(function () {
        $('.loader').hide();

        $(window).keydown(function(e){
            if(e.keyCode == 13) {
                $("#<%=btnRicercaArticoli.ClientID%>").click();
            }
        }); 
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA ARTICOLO
    function mostraArticolo(row) {
        $('.loader').show();
        $("#<%=hf_idArticolo.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditArticolo.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO ARTICOLO
    function inserisciArticoli() {
        $('.loader').show();
        $("#<%=hf_idArticolo.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsArticolo.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO ARTICOLO
    function openDettaglioArticolo(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            var i;
            var x = document.getElementsByClassName("articolo");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";  
            }
            document.getElementById(tipoName).style.display = "block";  
        }
    }

    // APRO/CHIUDO TAB
    function openTab(id) {
        var x = document.getElementById(id);
        if (x.className.indexOf("w3-show") == -1) {
            x.className += " w3-show";
        } else { 
            x.className = x.className.replace(" w3-show", "");
        }
    }
    // AZZERO TUTTI I CAMPI RICERCA
    function azzeraCampiRicerca() {
        $("#<%=tbDescrizione.ClientID%>").val('');
        $("#<%=tbDescrizioneBreve.ClientID%>").val('');
        $("#<%=TbCosto.ClientID%>").val('');
        $("#<%=tbIva.ClientID%>").val('');
        $("#<%=tbPrezzo.ClientID%>").val('');
    }


    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA ARTICOLO E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Articolo');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        pannelloPopup.style.display = "none";
    }

</script>
<Label><asp:Label ID="lblIntestazionePagina" runat="server" Text=""></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        
        <div class="w3-row-padding">
            <div class="w3-half">
                <label>Descrizione</label>
                <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-half">
                <label>Descrizione Breve</label>
                <asp:TextBox ID="tbDescrizioneBreve" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                <label>IVA</label>
                    <asp:TextBox ID="tbIva" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Prezzo</label>
                <asp:TextBox ID="tbPrezzo" runat="server" MaxLength="16" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Costo</label>
                <asp:TextBox ID="TbCosto" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaArticoli" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaArticoli_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciArticoli" runat="server"> 
                                <div id="btnInserisciArticoli" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciArticoli();">Inserisci</div>
                            </div>

                        </td>
                        <td style="width:20%;">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;"  OnClientClick="azzeraCampiRicerca();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>   

        <div class="round">
            <asp:GridView ID="gv_articoli" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_articoli_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_articoli_PageIndexChanging" PageSize="20">
            </asp:GridView>
        </div>
        
    </ContentTemplate>
    <Triggers>
        
        <asp:AsyncPostBackTrigger ControlID="btnRicercaArticoli" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="BtnPulisciCampiRicerca" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<asp:Button runat="server" ID="btnEditArticolo" Style="display: none" OnClick="EditArticolo_Click"/>
<asp:Button runat="server" ID="btnInsArticolo" Style="display: none" OnClick="InserisciArticoli_Click"/>

<asp:HiddenField ID="hf_idArticolo" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Articolo" />

<asp:UpdatePanel ID="upArticolo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-container w3-center w3-xlarge">
                    GESTIONE ARTICOLI
                </div>
                <br />
                
                <!-- DIV MESSAGGI DI ERRORE -->        

                <div id="panelErrore" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
                  <span onclick="this.parentElement.style.display='none'"
                  class="w3-button w3-large w3-display-topright">&times;</span>
                  <p><asp:Label ID="lbl_MessaggioErrore" runat="server" ></asp:Label></p>
                </div>                
                
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI ARTICOLO -->
                    <div class="w3-bar w3-green w3-round">
                        <div class="w3-bar-item w3-button w3-green" onclick="openDettaglioArticolo('Azienda')">Articolo</div>
                        <div class="w3-bar-item w3-button w3-green" onclick="openDettaglioArticolo('Referenti')" style="display:none">Referenti</div>
                        <div class="w3-bar-item w3-button w3-green w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-green w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
                    </div>
                    <!-- TAB ARTICOLI -->
                    <div id="Articolo" class="w3-container w3-border articolo"  style="display:block">

                        <div class="w3-row-padding">
                            <div class="w3-half">
                                <label>Descrizione</label>
                                <asp:TextBox ID="tbMod_Descrizione" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"  ></asp:TextBox>
                            </div>
                            <div class="w3-half">
                                <label>Desc. breve</label>
                                <asp:TextBox ID="tbMod_DescrizioneBreve" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                                        
                        <div class="w3-row-padding">
                            <div class="w3-third">
                                <label>Prezzo</label>
                                <asp:TextBox ID="tbMod_Prezzo" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="18"></asp:TextBox>
                            </div>
                            <div class="w3-third">
                                <label>Costo</label>
                                <asp:TextBox ID="tbMod_Costo" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="18" ></asp:TextBox>
                            </div>
                            <div class="w3-third">
                                <label>IVA</label>
                                <asp:TextBox ID="tbMod_IVA" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>

                        <div class="w3-row-padding">
                            <div class="w3-third">
                                <label>Genere</label>
                                <asp:DropDownList ID="cmbMod_Genere" runat="server" class="w3-input w3-border" disabled></asp:DropDownList>
                            </div>
                            <div class="w3-third">
                                <label>Gruppo</label><br />
                                <asp:DropDownList ID="cmbMod_Gruppo" runat="server" class="w3-input w3-border" disabled></asp:DropDownList>
                            </div>
                            <div class="w3-third">
                                <label>Sottogruppo</label><br />
                                <asp:DropDownList ID="cmbMod_Sottogruppo" runat="server" class="w3-input w3-border" disabled></asp:DropDownList>
                            </div>
                        </div>
                        <div class="w3-container">
                            <label>Gruppi</label>
                            <asp:ListBox ID="lbMod_Gruppi" runat="server" class="w3-input w3-border " ReadOnly="true" Rows="3" disabled></asp:ListBox>
                        </div>

                        <div class="w3-container">
                            <label>Note</label>
                            <asp:TextBox ID="tbMod_Note" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>

                        <div class="w3-row-padding">
                            <div class="w3-half">
                                <label>Stampa</label>
                                <asp:CheckBox ID="cbMod_Stampa" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                            </div>
                            <div class="w3-half">
                                <label>Attivo</label>
                                <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                            </div>
                        </div>

                        <div style="text-align: center;">
                            <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnModifica_Click" />
                            <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-panel w3-green w3-border w3-round" OnClick="btnElimina_Click"  OnClientClick="return confirm('Confermi eliminazione Azienda?')" Visible="false" />
                            <asp:Button ID="btnSalva" runat="server" Text="Salva" class="w3-panel w3-green w3-border w3-round" OnClick="btnSalva_Click" OnClientClick="return confirm('Confermi salvataggio modifiche?')" Visible="false"/>
                            <asp:Button ID="btnConfermaInserimento" runat="server" Text="Inserisci" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimento_Click" OnClientClick="return confirm('Confermi inserimento Azienda?')" Visible="false"/>
                            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false"/>
                        </div>
                    </div>
                    <!-- TAB REFERENTI -->
                    <div id="Referenti" class="w3-container w3-border azienda" style="display:none">
                        <label>Referenti</label>
                        <div class="round">
                            <asp:GridView ID="gvMod_Referenti" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvMod_Referenti_RigaSelezionata">
                            </asp:GridView>
                        </div>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriReferenti" runat="server" OnClick="btnApriReferenti_Click" Text="Gestione Referenti" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phReferenti" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Cognome</label>
                                            <asp:TextBox ID="tbInsCognomeReferente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Nome</label>
                                            <asp:TextBox ID="tbInsNomeReferente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Settore</label>
                                            <asp:TextBox ID="tbInsSettoreReferente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Email</label>
                                            <asp:TextBox ID="tbInsEmailReferente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                            <asp:TextBox ID="tbIdReferenteDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w3-row-padding">
                                        <div class="w3-quarter">
                                            <label>Telefono 1</label>
                                            <asp:TextBox ID="tbInsTelefono1Referente" runat="server" MaxLength="25" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Telefono 2</label>
                                            <asp:TextBox ID="tbInsTelefono2Referente" runat="server" MaxLength="25" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Cellulare</label>
                                            <asp:TextBox ID="tbInsCellulareReferente" runat="server" MaxLength="25" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Attivo</label><br />
                                            <asp:CheckBox ID="cbInsAttivoReferente" runat="server" class="w3-check" Checked="true"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="w3-container">
                                        <label>Note</label>
                                        <asp:TextBox ID="tbInsNoteReferente" runat="server" class="w3-input w3-border" placeholder="" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>

                                    <asp:Button ID="btnInserisciReferente" runat="server" Text="Inserisci Referente" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoReferente_Click" OnClientClick="return confirm('Confermi inserimento Referente?')" />
                                    <asp:Button ID="btnModificaReferente" runat="server" Text="Modifica Referente" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaModificaReferente_Click" OnClientClick="return confirm('Confermi modifica Referente?')" Visible="false" />
                                    <asp:Button ID="btnEliminaReferente" runat="server" Text="Elimina Referente" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaReferente_Click" OnClientClick="return confirm('Confermi eliminazione Referente?')" />
                                    <asp:Button ID="btnAnnullaReferente" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round"  OnClick="btnAnnullaReferente_Click" />
                                </asp:PlaceHolder>
                            </p>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnEditArticolo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInsArticolo" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnSalva" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnElimina" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAnnulla" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnEliminaReferente" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInserisciReferente" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagCollaboratori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script>

    // APRO POPUP VISUALIZZAZIONE/MODIFICA COLLABORATORE
    function mostraCollaboratore(row) {
        $("#<%=hf_idColl.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditCollaboratore.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO COLLABORATORE
    function inserisciCollaboratore() {
        $("#<%=hf_idColl.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsCollaboratore.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO COLLABORATORE
    function openDettaglioAnagrafica(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        //alert(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            //alert(document.getElementById(tipoName).id);
            //alert(document.getElementById(tipoName).style.display);
            var i;
            var x = document.getElementsByClassName("collab");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";  
            }
            document.getElementById(tipoName).style.display = "block";  
        }
    }

    // APRO/CHIUDO GESTIONE UPLOAD IMMAGINI COLLABORATORE
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
        $("#<%=tbCF.ClientID%>").val('');
        $("#<%=tbCitta.ClientID%>").val('');
        $("#<%=tbCognome.ClientID%>").val('');
        $("#<%=tbNome.ClientID%>").val('');
        $("#<%=TbPiva.ClientID%>").val('');
        $("#<%=TbSocieta.ClientID%>").val('');
        $("#<%=ddlQualifiche.ClientID%>").val('');

    }
</script>

<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        
        <div class="w3-row-padding">
            <div class="w3-quarter">
                <label>Cognome</label>
                <asp:TextBox ID="tbCognome" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Nome</label>
                <asp:TextBox ID="tbNome" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Codice Fiscale</label>
                <asp:TextBox ID="tbCF" runat="server" MaxLength="16" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Città</label>
                <asp:TextBox ID="tbCitta" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                <label>Qualifica</label>
                <asp:DropDownList ID="ddlQualifiche" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlQualifiche_SelectedIndexChanged" class="w3-input w3-border">
                </asp:DropDownList>
            </div>
            <div class="w3-quarter">
                <label>Società</label>
                <asp:TextBox ID="TbSocieta" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Partita Iva</label>
                <asp:TextBox ID="TbPiva" runat="server" MaxLength="11" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaCollaboratori_Click" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnInserisciCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-red w3-round-large" Text="Inserisci" OnClientClick="inserisciCollaboratore();" />
                        </td>
                        <td style="width:20%;">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;"  OnClientClick="azzeraCampiRicerca();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>   

        <div class="round">
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_collaboratori_RowDataBound">
            </asp:GridView>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRicercaCollaboratori" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="BtnPulisciCampiRicerca" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlQualifiche" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>


<asp:Button runat="server" ID="btnEditCollaboratore" Style="display: none" OnClick="EditCollaboratore_Click"/>
<asp:Button runat="server" ID="btnInsCollaboratore" Style="display: none" OnClick="InserisciCollaboratori_Click"/>

<asp:HiddenField ID="hf_idColl" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Anagrafica" />

<asp:UpdatePanel ID="upColl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-container w3-center w3-xlarge">
                    GESTIONE COLLABORATORI
                </div>
                <br />
                
                <!-- DIV MESSAGGI DI ERRORE -->        
                <div class="alert alert-danger alert-dismissible fade in" role="alert" runat="server" id="panelErrore" style="display: none">
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                    <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
                </div>
                            
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI COLLABORATORE -->
                    <div class="w3-bar w3-red w3-round">
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Anagrafica')">Anagrafica</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Qualifiche')">Qualifiche</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Indirizzi')">Indirizzi</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Telefoni')">Telefoni</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Email')">Email</div>
                        <div class="w3-bar-item w3-button w3-red w3-right"><asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-button w3-green w3-small w3-round" OnClick="btn_chiudi_Click" OnClientClick="return confirm('Confermi chiusura pagina?')"/></div>
                    </div>
                    <!-- TAB ANAGRAFICA -->
                    <div id="Anagrafica" class="w3-container w3-border collab"  style="display:block">
                    <table style="width:100%">
                        <tr>
                            <td style="width:75%">
                                <div class="w3-row-padding">
                                    <div class="w3-half">
                                        <label>Cognome</label>
                                        <asp:TextBox ID="tbMod_Cognome" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" ></asp:TextBox>
                                    </div>
                                    <div class="w3-half">
                                        <label>Nome</label>
                                        <asp:TextBox ID="tbMod_Nome" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                        
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Codice Fiscale</label>
                                        <asp:TextBox ID="tbMod_CF" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="16"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Nazione</label>
                                        <asp:TextBox ID="tbMod_Nazione" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Comune Nascita</label>
                                        <asp:TextBox ID="tbMod_ComuneNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Provincia Nascita</label>
                                        <asp:TextBox ID="tbMod_ProvinciaNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="2"></asp:TextBox>
                                    </div>
                                </div>
                                        
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Data Nascita</label>
                                        <asp:TextBox ID="tbMod_DataNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" TextMode="Date" MaxLength="10"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Comune Riferimento</label>
                                        <asp:TextBox ID="tbMod_ComuneRiferimento" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Partita Iva</label>
                                        <asp:TextBox ID="tbMod_PartitaIva" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="11"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Società</label>
                                        <asp:TextBox ID="tbMod_NomeSocieta" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                        
                                <div class="w3-row-padding">
                                    <div class="w3-half">
                                        <label>Assunto</label>
                                        <asp:CheckBox ID="cbMod_Assunto" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                                    </div>
                                    <div class="w3-half">
                                        <label>Attivo</label>
                                        <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                                    </div>
                                </div>

                                <div class="w3-container">
                                    <label>Note</label>
                                    <asp:TextBox ID="tbMod_Note" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>

                            </td>
                            <td style="width:25%; vertical-align:top">
                                <div class="w3-container">
                                    <h2></h2>
                                    <div class="w3-card-4" style="width:90%">
                                    <asp:Image ID="imgCollaboratore" runat="server" Width="100%" />
                                    <div class="w3-container w3-center">
                                        <p>
                                            <div onclick="openTab('divUploadImg')" class="w3-button w3-block w3-center-align">
                                                Carica Immagine</div>
                                            <div id="divUploadImg" class="w3-container w3-hide">
                                                <asp:FileUpload ID="fuImg" runat="server" Font-Size="X-Small" class="w3-button w3-yellow w3-round w3-margin" />
                                                <asp:Button ID="uploadButton" runat="server" class="w3-button w3-yellow w3-round w3-margin" OnClick="CaricaImmagine" Text="Carica Immagine" />
                                                <p>
                                                    <asp:Label ID="lblImage" runat="server" Font-Size="XX-Small"></asp:Label>
                                                </p>
                                            </div>
                                        </p>
                                    </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: center;">
                        <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnModifica_Click" />
                        <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-panel w3-green w3-border w3-round" OnClick="btnElimina_Click"  OnClientClick="return confirm('Confermi eliminazione Collaboratore?')" Visible="false" />
                        <asp:Button ID="btnSalva" runat="server" Text="Salva" class="w3-panel w3-green w3-border w3-round" OnClick="btnSalva_Click" OnClientClick="return confirm('Confermi salvataggio modifiche?')" Visible="false"/>
                        <asp:Button ID="btnConfermaInserimento" runat="server" Text="Inserisci" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimento_Click" OnClientClick="return confirm('Confermi inserimento Collaboratore?')" Visible="false"/>
                        <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false"/>
                    </div>
                    </div>
                    <!-- TAB QUALIFICHE -->
                    <div id="Qualifiche" class="w3-container w3-border collab" style="display:none">
                        <label>Qualifiche</label>
                        <asp:ListBox ID="lbMod_Qualifiche" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriQualifiche" runat="server" OnClick="btnApriQualifiche_Click" Text="Gestione Qualifiche" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phQualifiche" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center" style="width:50%;">
                                        <div class="w3-half">
                                            <label>Qualifiche</label>
                                            <asp:DropDownList ID="ddlQualificheDaAggiungere" runat="server" AutoPostBack="false" Width="100%" class="w3-input w3-border">
                                            </asp:DropDownList>                                                
                                        </div>
                                        <div class="w3-half">
                                            <label>Priorità</label>
                                            <asp:TextBox ID="tbInsPrioritaQualifica" runat="server" MaxLength="1" class="w3-input w3-border" placeholder="" Text="1" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnInserisciQualifica" runat="server" Text="Inserisci Qualifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoQualifica_Click" OnClientClick="return confirm('Confermi inserimento Qualifica?')" />
                                    <asp:Button ID="btnEliminaQualifica" runat="server" Text="Elimina Qualifica" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaQualifica_Click" OnClientClick="confirm('Confermi eliminazione Qualifica?')" />
                                </asp:PlaceHolder>
                            </p>
                        </div>
                    </div>
                    <!-- TAB INDIRIZZI -->
                    <div id="Indirizzi" class="w3-container w3-border collab" style="display:none">
                        <label>Indirizzi</label>
                        <asp:ListBox ID="lbMod_Indirizzi" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriIndirizzi" runat="server" OnClick="btnApriIndirizzi_Click" Text="Gestione Indirizzi" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phIndirizzi" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Tipo</label>
                                            <asp:TextBox ID="tbInsTipoIndirizzo" runat="server" class="w3-input w3-border" MaxLength="10" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Indirizzo</label>
                                            <asp:TextBox ID="tbInsIndirizzoIndirizzo" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Civico</label>
                                            <asp:TextBox ID="tbInsCivicoIndirizzo" runat="server" MaxLength="10" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Cap</label>
                                            <asp:TextBox ID="tbInsCapIndirizzo" runat="server" MaxLength="5" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Comune</label>
                                            <asp:TextBox ID="tbInsComuneIndirizzo" runat="server" class="w3-input w3-border" MaxLength="50" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Provincia</label>
                                            <asp:TextBox ID="tbInsProvinciaIndirizzo" runat="server" MaxLength="2" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Descrizione</label>
                                            <asp:TextBox ID="tbInsDescrizioneIndirizzo" runat="server" MaxLength="60" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Priorità</label>
                                            <asp:TextBox ID="tbInsPrioritaIndirizzo" runat="server" MaxLength="1" class="w3-input w3-border" placeholder="" Text="1" ></asp:TextBox>
                                            <asp:TextBox ID="tbIdIndirizzoDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnInserisciIndirizzo" runat="server" Text="Inserisci Indirizzo" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoIndirizzo_Click" OnClientClick="return confirm('Confermi inserimento Indirizzo?')" />
                                    <asp:Button ID="btnModificaIndirizzo" runat="server" Text="Modifica Indirizzo" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaModificaIndirizzo_Click" OnClientClick="return confirm('Confermi modifica Indirizzo?')" Visible="false" />
                                    <asp:Button ID="btnEliminaIndirizzo" runat="server" Text="Elimina Indirizzo" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaIndirizzo_Click" OnClientClick="confirm('Confermi eliminazione Indirizzo?')" />
                                </asp:PlaceHolder>
                            </p>
                        </div>
                    </div>
                    <!-- TAB TELEFONI -->
                    <div id="Telefoni" class="w3-container w3-border collab" style="display:none">
                        <label>Telefoni</label>
                        <asp:ListBox ID="lbMod_Telefoni" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                    </div>
                    <!-- TAB EMAIL -->
                    <div id="Email" class="w3-container  w3-border collab" style="display:none">
                        <label>E-Mail</label>
                        <asp:ListBox ID="lbMod_Email" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriEmail" runat="server" OnClick="btnApriEmail_Click" Text="Gestione Email" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phEmail" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Email</label>
                                            <asp:TextBox ID="tbInsEmail" runat="server" class="w3-input w3-border" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Priorità</label>
                                            <asp:TextBox ID="tbInsPrioritaEmail" runat="server" MaxLength="1" class="w3-input w3-border" placeholder="" Text="1"></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Tipo</label>
                                            <asp:TextBox ID="tbInsTipoEmail" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <asp:TextBox ID="tbIdEmailDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnInserisciEmail" runat="server" Text="Inserisci Email" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoEmail_Click" OnClientClick="return confirm('Confermi inserimento Email?')" />
                                    <asp:Button ID="btnModificaEmail" runat="server" Text="Modifica Email" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaModificaEmail_Click" OnClientClick="return confirm('Confermi modifica Email?')" Visible="false" />
                                    <asp:Button ID="btnEliminaEmail" runat="server" Text="Elimina Email" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaEmail_Click" OnClientClick="confirm('Confermi eliminazione Email?')" />
                                </asp:PlaceHolder>
                            </p>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        
        <asp:PostBackTrigger ControlID="uploadButton" />
        <asp:AsyncPostBackTrigger ControlID="btnEditCollaboratore" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnSalva" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnElimina" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAnnulla" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnEliminaQualifica" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInserisciQualifica" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagCollaboratori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script>

    $(document).ready(function () {
        $('.loader').hide();
        
        $(window).keydown(function(e){
            if(e.keyCode == 13) {
                $("#<%=btnRicercaCollaboratori.ClientID%>").click();
            }
        }); 
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA COLLABORATORE
    function mostraCollaboratore(row) {
        $('.loader').show();
        $("#<%=hf_idColl.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditCollaboratore.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO COLLABORATORE
    function inserisciCollaboratore() {
        $('.loader').show();
        $("#<%=hf_idColl.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsCollaboratore.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO COLLABORATORE
    function openDettaglioAnagrafica(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
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
        $("#<%=tbRegione.ClientID%>").val('');

    }

    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA ANAGRAFICA E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Anagrafica');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        //alert(pannelloPopup.id);
        pannelloPopup.style.display = "none";
    }
    


</script>
<Label class="w3-text-blue"><asp:Label ID="lblTipoAzienda" runat="server" Text="COLLABORATORI"></asp:Label></Label>

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
                <label>Regione</label>
                <asp:TextBox ID="tbRegione" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
        </div>   
        <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                &nbsp;
            </div>
            <div class="w3-quarter">
                &nbsp;
            </div>
            <div class="w3-quarter">
                &nbsp;
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaCollaboratori_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciCollaboratori" runat="server"> 
                                <%--<button id="clientBtnInserisciCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-red w3-round-large" value="Inserisci" onclick="inserisciCollaboratore();">Inserisci</button>--%>
                                <div id="BtnInserisciCollaboratori" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciCollaboratore();">Inserisci</div>
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
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_collaboratori_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_collaboratori_PageIndexChanging" PageSize="20" >
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
<%--                <div class="alert alert-danger alert-dismissible fade in" role="alert" runat="server" id="panelErrore" style="display: none">
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                    <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
                </div>--%>
                <div id="panelErrore" class="w3-panel w3-red w3-display-container" runat="server" style="display:none;">
                  <span onclick="this.parentElement.style.display='none'"
                  class="w3-button w3-large w3-display-topright">&times;</span>
                  <p><asp:Label ID="lbl_MessaggioErrore" runat="server" ></asp:Label></p>
                </div>                             
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI COLLABORATORE -->
                    <div class="w3-bar w3-red w3-round">
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Anagrafica')">Anagrafica</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Qualifiche')">Qualifiche</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Indirizzi')">Indirizzi</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Telefoni')">Telefoni</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Email')">Email</div>
                        <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Documenti')">Documenti</div>
                        <div class="w3-bar-item w3-button w3-red w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-green w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
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
                                        <asp:TextBox ID="tbMod_DataNascita" runat="server" class="w3-input w3-border" placeholder="gg/mm/aaaa" MaxLength="10" ReadOnly="true"></asp:TextBox>
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
                                     <div class="w3-third">
<%--                                        <label>Attivo</label>
                                        <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>--%>
                                         <label>Regione Riferimento</label>
                                        <asp:DropDownList runat="server" ID="cmbMod_RegioneRiferimento" class="w3-input w3-border" disabled>
                                            <asp:ListItem Text="" Value="" />
                                            <asp:ListItem Text="Abruzzo" Value="Abruzzo" />
                                            <asp:ListItem Text="Basilicata" Value="Basilicata" />
                                            <asp:ListItem Text="Calabria" Value="Calabria" />
                                            <asp:ListItem Text="Campania" Value="Campania" />
                                            <asp:ListItem Text="Emilia Romagna" Value="Emilia Romagna" />
                                            <asp:ListItem Text="Friuli Venezia Giulia" Value="Friuli Venezia Giulia" />
                                            <asp:ListItem Text="Lazio" Value="Lazio" />
                                            <asp:ListItem Text="Liguria" Value="Liguria" />
                                            <asp:ListItem Text="Lombardia" Value="Lombardia" />
                                            <asp:ListItem Text="Marche" Value="Marche" />
                                            <asp:ListItem Text="Molise" Value="Molise" />
                                            <asp:ListItem Text="Piemonte" Value="Piemonte" />
                                            <asp:ListItem Text="Puglia" Value="Puglia" />
                                            <asp:ListItem Text="Sardegna" Value="Sardegna" />
                                            <asp:ListItem Text="Sicilia" Value="Sicilia" />
                                            <asp:ListItem Text="Toscana" Value="Toscana" />
                                            <asp:ListItem Text="Trentino Alto Adige" Value="Trentino Alto Adige" />
                                            <asp:ListItem Text="Umbria" Value="Umbria" />
                                            <asp:ListItem Text="Val d'Aosta" Value="Val d'Aosta" />
                                            <asp:ListItem Text="Veneto" Value="Veneto" />
                                        </asp:DropDownList>
                                    </div>                                   
                                    <div class="w3-third">
                                        <label>Iban</label><br />
                                        <asp:TextBox ID="tbMod_Iban" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="27" ></asp:TextBox>
                                    </div>
                                    <div class="w3-third">
                                        <label>Assunto</label><br />
                                        <asp:CheckBox ID="cbMod_Assunto" runat="server" class="w3-check" disabled></asp:CheckBox>
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
                                            <p>
                                            </p>
                                            <p>
                                            </p>
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
                                    <asp:Button ID="btnEliminaQualifica" runat="server" Text="Elimina Qualifica" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaQualifica_Click" OnClientClick="return confirm('Confermi eliminazione Qualifica?')" />
                                </asp:PlaceHolder>
                                <p>
                                </p>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                    <!-- TAB INDIRIZZI -->
                    <div id="Indirizzi" class="w3-container w3-border collab" style="display:none">
                        <label>Indirizzi</label>
                        <%--<asp:ListBox ID="lbMod_Indirizzi" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>--%>
                        <div class="round">
                            <asp:GridView ID="gvMod_Indirizzi" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvMod_Indirizzi_RigaSelezionata" >
                            </asp:GridView>
                        </div>                        
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriIndirizzi" runat="server" OnClick="btnApriIndirizzi_Click" Text="Gestione Indirizzi" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phIndirizzi" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Tipo</label>
                                            <asp:DropDownList ID="cmbInsTipoIndirizzo" runat="server"  class="w3-input w3-border">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="Via">Via</asp:ListItem>
                                                <asp:ListItem Value="Viale">Viale</asp:ListItem>
                                                <asp:ListItem Value="Corso">Corso</asp:ListItem>
                                                <asp:ListItem Value="Piazza">Piazza</asp:ListItem>
                                                <asp:ListItem Value="Piazzale">Piazzale</asp:ListItem>
                                                <asp:ListItem Value="Largo">Largo</asp:ListItem>
                                                <asp:ListItem Value ="Vicolo">Vicolo</asp:ListItem>
                                                <asp:ListItem Value ="Altro">Altro</asp:ListItem>                                            
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="tbInsTipoIndirizzo" runat="server" class="w3-input w3-border" MaxLength="10" ></asp:TextBox>--%>
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
                                    <asp:Button ID="btnEliminaIndirizzo" runat="server" Text="Elimina Indirizzo" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaIndirizzo_Click" OnClientClick="return confirm('Confermi eliminazione Indirizzo?')" />
                                    <asp:Button ID="btnAnnullaIndirizzo" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round"  OnClick="btnAnnullaIndirizzo_Click" />
                                </asp:PlaceHolder>
                                <p>
                                </p>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                    <!-- TAB TELEFONI -->
                    <div id="Telefoni" class="w3-container w3-border collab" style="display:none">
                        <label>Telefoni</label>
                        <%--<asp:ListBox ID="lbMod_Telefoni" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>--%>
                        <div class="round">
                            <asp:GridView ID="gvMod_Telefoni" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvMod_Telefoni_RigaSelezionata" >
                            </asp:GridView>
                        </div>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriTelefoni" runat="server" OnClick="btnApriTelefoni_Click" Text="Gestione Telefoni" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phTelefoni" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                            <label>Prefisso Int.</label>
                                            <asp:TextBox ID="tbInsPrefIntTelefono" runat="server" class="w3-input w3-border" MaxLength="5" Text="+39" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Prefisso Naz.</label>
                                            <asp:TextBox ID="tbInsPrefNazTelefono" runat="server" MaxLength="5" class="w3-input w3-border" placeholder=""></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Numero</label>
                                            <asp:TextBox ID="tbInsNumeroTelefono" runat="server" MaxLength="15" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Tipo</label>
                                            <asp:DropDownList ID="cmbInsTipoTelefono" runat="server" class="w3-input w3-border">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="Cellulare">Cellulare</asp:ListItem>
                                                <asp:ListItem Value="Fisso">Fisso</asp:ListItem>
                                                <asp:ListItem Value="Centralino">Centralino</asp:ListItem>
                                                <asp:ListItem Value ="Altro">Altro</asp:ListItem>                                            
                                            </asp:DropDownList>

                                            <%--<asp:TextBox ID="tbInsTipoTelefono" runat="server" MaxLength="30" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>--%>
                                        </div>
                                    </div>
                                    <div class="w3-row-padding w3-center w3-text-center">
                                        <div class="w3-quarter">
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Whatsapp</label>
                                            <asp:CheckBox ID="cbInsWhatsappTelefono" runat="server" class="w3-input w3-border"></asp:CheckBox>
                                        </div>
                                        <div class="w3-quarter">
                                            <label>Priorità</label>
                                            <asp:TextBox ID="tbInsPrioritaTelefono" runat="server" MaxLength="1" class="w3-input w3-border" placeholder="" Text="1" ></asp:TextBox>
                                        </div>
                                        <div class="w3-rest">
                                            <label>Descrizione</label>
                                            <asp:TextBox ID="tbInsDescrizioneTelefono" runat="server" MaxLength="60" class="w3-input w3-border" placeholder="" ></asp:TextBox>
                                            <asp:TextBox ID="tbIdTelefonoDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnInserisciTelefono" runat="server" Text="Inserisci Telefono" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoTelefono_Click" OnClientClick="return confirm('Confermi inserimento Telefono?')" />
                                    <asp:Button ID="btnModificaTelefono" runat="server" Text="Modifica Telefono" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaModificaTelefono_Click" OnClientClick="return confirm('Confermi modifica Telefono?')" Visible="false" />
                                    <asp:Button ID="btnEliminaTelefono" runat="server" Text="Elimina Telefono" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaTelefono_Click" OnClientClick="return confirm('Confermi eliminazione Telefono?')" />
                                    <asp:Button ID="btnAnnullaTelefono" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round"  OnClick="btnAnnullaTelefono_Click" />
                                </asp:PlaceHolder>
                                <p>
                                </p>
                                <p>
                                </p>
                            </p>
                        </div>

                    </div>
                    <!-- TAB EMAIL -->
                    <div id="Email" class="w3-container  w3-border collab" style="display:none">
                        <label>E-Mail</label>
                        <%--<asp:ListBox ID="lbMod_Email" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3" Visible="false"></asp:ListBox>--%>
                        <div class="round">
                            <asp:GridView ID="gvMod_Email" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvMod_Email_RigaSelezionata" >
                            </asp:GridView>
                        </div>
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
                                    <asp:Button ID="btnEliminaEmail" runat="server" Text="Elimina Email" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaEmail_Click" OnClientClick="return confirm('Confermi eliminazione Email?')" />
                                    <asp:Button ID="btnAnnullaEmail" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round"  OnClick="btnAnnullaEmail_Click" />
                                </asp:PlaceHolder>
                                <p>
                                </p>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                    <!-- TAB DOCUMENTI -->
                    <div id="Documenti" class="w3-container  w3-border collab" style="display:none">
                        <label>Documenti</label>
                        <%--<asp:ListBox ID="lbMod_Documenti" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3" Visible="false"></asp:ListBox>--%>
                        <div class="round">
                            <asp:GridView ID="gvMod_Documenti" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" AutoGenerateSelectButton="True" OnSelectedIndexChanged="gvMod_Documenti_RigaSelezionata" OnRowDataBound="gvMod_Documenti_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false"  Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="30px" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="30px" />
                                    </asp:TemplateField>                                
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="w3-container w3-center">
                            <p>
                                <asp:Button ID="btnApriDocumenti" runat="server" OnClick="btnApriDocumenti_Click" Text="Gestione Documenti" class="w3-panel w3-green w3-border w3-round" />
                                <asp:PlaceHolder ID="phDocumenti" runat="server" Visible="false">                                
                                    <div class="w3-row-padding w3-center">
                                        <div class="w3-half">
                                            <label>TipoDocumento</label>
                                            <asp:DropDownList ID="cmbInsTipoDocumento" runat="server" class="w3-input w3-border">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="Carta di Identità">Carta di Identità</asp:ListItem>
                                                <asp:ListItem Value="Patente">Patente</asp:ListItem>
                                                <asp:ListItem Value="Passaporto">Passaporto</asp:ListItem>
                                                <asp:ListItem Value ="Altro">Altro</asp:ListItem>                                            
                                            </asp:DropDownList>
                                        </div>
                                        <div class="w3-half">
                                            <label>Numero Documento</label>
                                            <asp:TextBox ID="tbInsNumeroDocumento" runat="server" MaxLength="20" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                            <asp:TextBox ID="tbIdDocumentoDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="w3-rest">
                                            <label>Sfoglia</label>
                                            
                                        </div>
                                    </div>
                                    <div class="w3-row-padding w3-center">
                                        <div class="w3-threequarter"">
                                            <asp:FileUpload ID="fuDoc" runat="server" Font-Size="X-Small" class="w3-input w3-border" Visible="false" />
                                        </div>
                                        <div class="w3-quarter">
                                            <asp:Button ID="btnCaricaDocumento" runat="server" class="w3-btn w3-circle w3-green w3-left" ToolTip="Upload Documento" OnClick="CaricaDocumento" Text="+" Visible="false" />
                                        </div>
                                    </div>
                                    <div class="w3-panel w3-center w3-text-center">
                                        <Label class="w3-text-green">
                                            <asp:Label ID="lblDoc" runat="server" ></asp:Label>
                                        </Label>
                                    </div>
                                    <asp:Button ID="btnInserisciDocumento" runat="server" Text="Inserisci Documento" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoDocumento_Click" OnClientClick="return confirm('Confermi inserimento Documento?')" />
                                    <asp:Button ID="btnModificaDocumento" runat="server" Text="Modifica Documento" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaModificaDocumento_Click" OnClientClick="return confirm('Confermi modifica Documento?')" Visible="false" />
                                    <asp:Button ID="btnEliminaDocumento" runat="server" Text="Elimina Documento" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaDocumento_Click" OnClientClick="return confirm('Confermi eliminazione Documento?')" />
                                    <asp:Button ID="btnAnnullaDocumento" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round"  OnClick="btnAnnullaDocumento_Click" />
                                </asp:PlaceHolder>
                                <p>
                                </p>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>

                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        
        <asp:PostBackTrigger ControlID="uploadButton" />
        <asp:PostBackTrigger ControlID="btnCaricaDocumento" />
        <asp:AsyncPostBackTrigger ControlID="btnEditCollaboratore" EventName="Click" />

        <asp:AsyncPostBackTrigger ControlID="btnSalva" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnElimina" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAnnulla" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnEliminaQualifica" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInserisciQualifica" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>



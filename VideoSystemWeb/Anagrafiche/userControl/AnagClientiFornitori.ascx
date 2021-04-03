<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagClientiFornitori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagClientiFornitori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script>
    $(document).ready(function () {
        $('.loader').hide();

        $(window).keydown(function(e){
            if(e.keyCode == 13) {
                $("#<%=btnRicercaAziende.ClientID%>").click();
            }
        }); 
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA AZIENDA
    function mostraAzienda(row) {
        $('.loader').show();
        $("#<%=hf_idAzienda.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditAzienda.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO COLLABORATORE
    function inserisciAzienda() {
        $('.loader').show();
        $("#<%=hf_idAzienda.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsAzienda.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO AZIENDA
    function openDettaglioAzienda(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            var i;
            var x = document.getElementsByClassName("azienda");
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
        $("#<%=tbComune.ClientID%>").val('');
        $("#<%=tbRagioneSociale.ClientID%>").val('');
        $("#<%=tbProvincia.ClientID%>").val('');
        $("#<%=TbPiva.ClientID%>").val('');
        $("#<%=tbCodiceIdentificativo.ClientID%>").val('');
        $("#<%=ddlTipoAzienda.ClientID%>").val('');
    }
    function copiaLegaleSuOperativo() {

        var x = $("#<%=tbMod_CapLegale.ClientID%>").val();
        $("#<%=tbMod_CapOperativo.ClientID%>").val(x);

        x = $("#<%=tbMod_CivicoLegale.ClientID%>").val();
        $("#<%=tbMod_CivicoOperativo.ClientID%>").val(x);

        x = $("#<%=tbMod_IndirizzoLegale.ClientID%>").val();
        $("#<%=tbMod_IndirizzoOperativo.ClientID%>").val(x);

        x = $("#<%=tbMod_ComuneLegale.ClientID%>").val();
        $("#<%=tbMod_ComuneOperativo.ClientID%>").val(x);

        x = $("#<%=tbMod_ProvinciaLegale.ClientID%>").val();
        $("#<%=tbMod_ProvinciaOperativo.ClientID%>").val(x);

        x = $("#<%=tbMod_NazioneLegale.ClientID%>").val();
        $("#<%=tbMod_NazioneOperativo.ClientID%>").val(x);

        x = $("#<%=cmbMod_TipoIndirizzoLegale.ClientID%>").val();
        $("#<%=cmbMod_TipoIndirizzoOperativo.ClientID%>").val(x);

    }

    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA AZIENDA E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Azienda');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        pannelloPopup.style.display = "none";
    }

</script>
<Label><asp:Label ID="lblTipoAzienda" runat="server" Text=""></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        
        <div class="w3-row-padding">
            <div class="w3-quarter">
                <label>Ragione Sociale</label>
                <asp:TextBox ID="tbRagioneSociale" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Partita Iva</label>
                <asp:TextBox ID="TbPiva" runat="server" MaxLength="11" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Comune</label>
                <%--<asp:TextBox ID="tbCF" runat="server" MaxLength="16" class="w3-input w3-border" placeholder=""></asp:TextBox>--%>
                <asp:TextBox ID="tbComune" runat="server" MaxLength="16" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Provincia</label>
                <%--<asp:TextBox ID="TbReferente" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>--%>
                <asp:TextBox ID="tbProvincia" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                <label>Codice</label>
                    <asp:TextBox ID="tbCodiceIdentificativo" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Tipo</label>
                <asp:DropDownList ID="ddlTipoAzienda" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlTipoAzienda_SelectedIndexChanged" class="w3-input w3-border">
                </asp:DropDownList>
            </div>
            <div class="w3-quarter">
                &nbsp;
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaAziende" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaAziende_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciAzienda" runat="server"> 
                                <div id="btnInserisciAzienda" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciAzienda();">Inserisci</div>
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
            <div class="w3-container w3-center">
                <table class="w3-table w3-small" style="width:200px">
                    <tr>
                        <th>Tot.Elementi</th>
                        <th><asp:TextBox runat="server" class="w3-input w3-border" ID="tbTotElementiGriglia" Text="" ReadOnly="true" Height="15px" /></th>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="gv_aziende" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_aziende_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_aziende_PageIndexChanging" PageSize="20">
                <Columns>
                    <asp:TemplateField HeaderText="whatsapp">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" id="cbwhatsapp" AutoPostBack="true" Checked='false' Enabled='false' OnCheckedChanged="cbwhatsapp_CheckedChanged"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        
    </ContentTemplate>
    <Triggers>
        
        <asp:AsyncPostBackTrigger ControlID="btnRicercaAziende" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="BtnPulisciCampiRicerca" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlTipoAzienda" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<asp:Button runat="server" ID="btnEditAzienda" Style="display: none" OnClick="EditAzienda_Click"/>
<asp:Button runat="server" ID="btnInsAzienda" Style="display: none" OnClick="InserisciAzienda_Click"/>

<asp:HiddenField ID="hf_idAzienda" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Azienda" />

<asp:UpdatePanel ID="upAzienda" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-row-padding w3-margin w3-center w3-large w3-green w3-round" >
                    <div class="w3-quarter">
                        GESTIONE AZIENDE
                    </div>
                    <div class="w3-rest">
                        <asp:Label runat="server" ID="lblDettaglioModifica" Text=""></asp:Label>
                    </div>
                </div>                
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI AZIENDA -->
                    <div class="w3-bar w3-green w3-round">
                        <div class="w3-bar-item w3-button w3-green" onclick="openDettaglioAzienda('Azienda')">Azienda</div>
                        <div class="w3-bar-item w3-button w3-green" onclick="openDettaglioAzienda('Referenti')">Referenti</div>
                        <div class="w3-bar-item w3-button w3-green w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-green w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
                    </div>
                    <!-- TAB AZIENDA -->
                    <div id="Azienda" class="w3-container w3-border azienda"  style="display:block">

                        <div class="w3-row-padding">
                            <div class="w3-half">
                                <label>Ragione Sociale</label>
                                <asp:TextBox ID="tbMod_RagioneSociale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  ></asp:TextBox>
                            </div>
                            <div class="w3-half">
                                <label>Partita Iva</label>
                                <asp:TextBox ID="tbMod_PartitaIva" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                                        
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Codice Fiscale</label>
                                <asp:TextBox ID="tbMod_CF" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="16"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Codice Identificativo</label>
                                <asp:TextBox ID="tbMod_CodiceIdentificativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Iban</label>
                                <asp:TextBox ID="tbMod_Iban" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Pagamento</label><br />
                                <asp:DropDownList ID="cmbMod_Pagamento" runat="server" CssClass="w3-input w3-border" disabled Visible="false" />
                                <asp:TextBox ID="tbMod_NotaPagamento" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="w3-panel w3-blue w3-center w3-round">
                          <h5 class="w3-text-white" style="text-shadow:1px 1px 0 #444"> <b>Indirizzo Legale</b> </h5>
                        </div>
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Tipo Indirizzo</label><br />
                                <asp:DropDownList ID="cmbMod_TipoIndirizzoLegale" runat="server" CssClass="w3-input w3-border" disabled>
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value="Via">Via</asp:ListItem>
                                    <asp:ListItem Value="Viale">Viale</asp:ListItem>
                                    <asp:ListItem Value="Corso">Corso</asp:ListItem>
                                    <asp:ListItem Value="Piazza">Piazza</asp:ListItem>
                                    <asp:ListItem Value="Piazzale">Piazzale</asp:ListItem>
                                    <asp:ListItem Value="Largo">Largo</asp:ListItem>
                                    <asp:ListItem Value="Vicolo">Vicolo</asp:ListItem>
                                    <asp:ListItem Value="Circ.ne">Circ.ne</asp:ListItem>
                                    <asp:ListItem Value="Altro">Altro</asp:ListItem>                                            
                                </asp:DropDownList>
                            </div>
                            <div class="w3-quarter">
                                <label>Indirizzo</label>
                                <asp:TextBox ID="tbMod_IndirizzoLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Civico</label>
                                <asp:TextBox ID="tbMod_CivicoLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Cap</label>
                                <asp:TextBox ID="tbMod_CapLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                                        
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Comune</label>
                                <asp:TextBox ID="tbMod_ComuneLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Provincia</label>
                                <asp:TextBox ID="tbMod_ProvinciaLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Nazione</label>
                                <asp:TextBox ID="tbMod_NazioneLegale" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="w3-panel w3-amber w3-center w3-round">
                          <h5 class="w3-text-white" style="text-shadow:1px 1px 0 #444"> <b>Indirizzo Operativo</b> </h5>
                        </div>                        
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Tipo Indirizzo</label><br />
                                <asp:DropDownList ID="cmbMod_TipoIndirizzoOperativo" runat="server" CssClass="w3-input w3-border" disabled>
                                    <asp:ListItem Value=""></asp:ListItem>
                                    <asp:ListItem Value="Via">Via</asp:ListItem>
                                    <asp:ListItem Value="Viale">Viale</asp:ListItem>
                                    <asp:ListItem Value="Corso">Corso</asp:ListItem>
                                    <asp:ListItem Value="Piazza">Piazza</asp:ListItem>
                                    <asp:ListItem Value="Piazzale">Piazzale</asp:ListItem>
                                    <asp:ListItem Value="Largo">Largo</asp:ListItem>
                                    <asp:ListItem Value ="Vicolo">Vicolo</asp:ListItem>
                                    <asp:ListItem Value="Circ.ne">Circ.ne</asp:ListItem>
                                    <asp:ListItem Value ="Altro">Altro</asp:ListItem>                                            
                                </asp:DropDownList>
                            </div>
                            <div class="w3-quarter">
                                <label>Indirizzo</label>
                                <asp:TextBox ID="tbMod_IndirizzoOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Civico</label>
                                <asp:TextBox ID="tbMod_CivicoOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Cap</label>
                                <asp:TextBox ID="tbMod_CapOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="5"></asp:TextBox>                                    
                            </div>
                        </div>
                                        
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Comune</label>
                                <asp:TextBox ID="tbMod_ComuneOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Provincia</label>
                                <asp:TextBox ID="tbMod_ProvinciaOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="2"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Nazione</label>
                                <asp:TextBox ID="tbMod_NazioneOperativo" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="w3-quarter w3-center">
                                <div id="clientBtnCopiaLegaleSuOperativo" class="w3-btn w3-white w3-border w3-border-red w3-round-large w3-margin-top" onclick="copiaLegaleSuOperativo();">Copia da Legale</div>
                            </div>
                        </div>
                        <hr />
                        <div class="w3-row-padding">
                            <div class="w3-quarter">
                                <label>Telefono</label>
                                <asp:TextBox ID="tbMod_Telefono" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="25"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Fax</label>
                                <asp:TextBox ID="tbMod_Fax" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="25"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Email</label>
                                <asp:TextBox ID="tbMod_Email" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="w3-quarter">
                                <label>Pec</label>
                                <asp:TextBox ID="tbMod_Pec" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="60"></asp:TextBox>
                            </div>
                        </div>

                        <div class="w3-row-padding">
                            <div class="w3-half">
                                <label>Sito Web</label>
                                <asp:TextBox ID="tbMod_WebSite" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true"  MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="w3-half">
                                <label>Tipologia</label><br />
                                <asp:DropDownList ID="cmbMod_TipoAzienda" runat="server" CssClass="w3-input w3-border" disabled></asp:DropDownList>
                            </div>
                        </div>

                        <div class="w3-row-padding">
                            <div class="w3-half">
                                <label>Cliente</label>
                                <asp:CheckBox ID="cbMod_Cliente" runat="server" Enabled="false" CssClass="w3-check"></asp:CheckBox>
                            </div>
                            <div class="w3-half">
                                <label>Fornitore</label>
                                <asp:CheckBox ID="cbMod_Fornitore" runat="server" Enabled="false" CssClass="w3-check"></asp:CheckBox>
                            </div>
<%--                            <div class="w3-quarter">
                                <label>Attivo</label>
                                <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false" CssClass="w3-check"></asp:CheckBox>
                            </div>--%>
                        </div>

                        <div class="w3-container">
                            <label>Note</label>
                            <asp:TextBox ID="tbMod_Note" runat="server" CssClass="w3-input w3-border" placeholder="" ReadOnly="true" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox>
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
                        <asp:ListBox ID="lbMod_Referenti" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3" Visible="false"></asp:ListBox>
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
        <asp:AsyncPostBackTrigger ControlID="btnEditAzienda" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInsAzienda" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnSalva" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnElimina" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnAnnulla" EventName="Click" />
        
        <asp:AsyncPostBackTrigger ControlID="btnEliminaReferente" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnInserisciReferente" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

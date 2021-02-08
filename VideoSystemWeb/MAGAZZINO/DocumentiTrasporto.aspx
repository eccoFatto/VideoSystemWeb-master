<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentiTrasporto.aspx.cs" ValidateRequest="false" Inherits="VideoSystemWeb.Magazzino.DocumentiTrasporto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaDocumentoTrasporto.ClientID%>").click();
                    }
                }
            });

            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
                });

            });
        });

        // APRO POPUP VISUALIZZAZIONE/MODIFICA DOCUMENTO TRASPORTO
        function mostraDocumentoTrasporto(row) {
            $('.loader').show();
            $("#<%=hf_idDocTras.ClientID%>").val(row);
            $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
            $("#<%=btnEditDocumentoTrasporto.ClientID%>").click();
        }
        // APRO POPUP DI INSERIMENTO DOCUMENTO TRASPORTO
        function inserisciDocumentoTrasporto() {
            $('.loader').show();
            $("#<%=hf_idDocTras.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
            $("#<%=btnInsDocumentoTrasporto.ClientID%>").click();
        }
        // APRO POPUP DI INSERIMENTO ATTREZZATURA MAGAZZINO
        function inserisciAttrezzaturaMagazzino() {
            $('.loader').show();
            $("#<%=btnInsAttrezzaturaMagazzino.ClientID%>").click();
        }

        

        // APRO POPUP RICERCA CLIENTE
        function cercaCliente() {
            $('.loader').show();
            $("#<%=btnCercaCliente.ClientID%>").click();
        }

        // APRO LE TAB DETTAGLIO DocumentoTrasporto
        function openDettaglioDocumentoTrasporto(tipoName) {
            $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
            if (document.getElementById(tipoName) != undefined) {
                var i;
                var x = document.getElementsByClassName("prot");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(tipoName).style.display = "block";
            }
        }

        function chiudiPopup() {
            // QUANDO APRO IL POPUP RIPARTE SEMPRE DA DocumentoTrasporto E NON DALL'ULTIMA TAB APERTA
            $("#<%=hf_tabChiamata.ClientID%>").val('DocumentoTrasporto');
            $("#<%=hf_idDocTras.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('VISUALIZZAZIONE');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();

        }

        function chiudiPopupClienti() {
            $("#<%=btnChiudiPopupClientiServer.ClientID%>").click();
        }

        function chiudiPopupMagazzino() {
            $("#<%=btnChiudiPopupMagazzinoServer.ClientID%>").click();
        }
        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCausale.ClientID%>").val('');
            $("#<%=tbComune.ClientID%>").val('');
            $("#<%=tbDataTrasporto.ClientID%>").val('');
            $("#<%=tbDataTrasportoA.ClientID%>").val('');
            $("#<%=tbDestinatario.ClientID%>").val('');
            $("#<%=tbIndirizzo.ClientID%>").val('');
            $("#<%=tbPartitaIva.ClientID%>").val('');
            $("#<%=tbNumeroDocTrasporto.ClientID%>").val('');
            $("#<%=tbTrasportatore.ClientID%>").val('');
        }

        function azzeraCampiRicercaMagazzino() {
            $("#<%=tbSearch_CodiceVideosystem.ClientID%>").val('');
            $("#<%=tbSearch_DescMagazzino.ClientID%>").val('');
            $("#<%=tbSearch_Seriale.ClientID%>").val('');
            $("#<%=tbIns_Quantita.ClientID%>").val('1');
        }

        function associaCliente(idCli, cliente) {
            var txt1 = $("#<%=tbMod_destinatario.ClientID%>");
            var txt2 = $("#<%=tbMod_IdCliente.ClientID%>");
            txt1.val(cliente);
            txt2.val(idCli);
            txt1.text = cliente;
            txt2.text = idCli;
            $("#<%=btnAssociaClienteServer.ClientID%>").click();
        }

        function inserisciCliente() {
            if (document.getElementById('<%=tbSearch_RagioneSociale.ClientID%>').value != '') {
                associaCliente('', document.getElementById('<%=tbSearch_RagioneSociale.ClientID%>').value);
            }
        }

        function popupProt(messaggio) {
            document.getElementById('popMessage').style.display = 'block';
            $('#textPopMess').html(messaggio);
        }
    </script>

    <div id="popMessage" class="w3-modal" style="z-index:10000">
        <div class="w3-modal-content w3-animate-opacity w3-card-4">
            <header class="w3-container w3-green">
                <span onclick="document.getElementById('popMessage').style.display='none'"
                    class="w3-button w3-display-topright">&times;</span>
                <h2>Operazione completata</h2>
            </header>
            <div class="w3-container">
                <p id="textPopMess"></p>
            </div>
        </div>
    </div>
    <label><asp:Label ID="lblDocumentiDiTrasporto" runat="server" Text="DOCUMENTI DI TRASPORTO" ForeColor="Teal"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Destinatario</label>
                    <asp:TextBox ID="tbDestinatario" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>                
                <div class="w3-quarter">
                    <label>Causale</label>
                    <asp:TextBox ID="tbCausale" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Trasporto (Da-A)</label>
                    <table style="width:100%;">
                        <tr>
                            <td style="position:relative;"><asp:TextBox ID="tbDataTrasporto" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                            <td style="position:relative;"><asp:TextBox ID="tbDataTrasportoA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>            
                <div class="w3-quarter">
                    <label>Numero DocumentoTrasporto</label>
                    <asp:TextBox ID="tbNumeroDocTrasporto" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-half">
                    <label>Indirizzo</label>
                    <asp:TextBox ID="tbIndirizzo" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-half">
                    <label>Comune</label>
                    <asp:TextBox ID="tbComune" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-quarter">
                    <label>Partita Iva</label>
                    <asp:TextBox ID="tbPartitaIva" runat="server" MaxLength="11" class="w3-input w3-border" placeholder=""></asp:TextBox>                </div>  
                <div class="w3-half">
                    <label>Trasportatore</label>
                    <asp:TextBox ID="tbTrasportatore" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>               
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Button ID="btnRicercaDocumentoTrasporto" runat="server" CssClass="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaDocumentoTrasporto_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 40%;">
                                <div id="divBtnInserisciDocumentoTrasporto" runat="server">
                                    <div id="clbtnInserisciDocumentoTrasporto" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciDocumentoTrasporto();" >Nuovo Documento</div>
                                </div>

                            </td>
                            <td style="width: 20%;">
                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" CssClass="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                            </td>
                        </tr>
                    </table>
                </div>

            </div>

            <div class="round">
                <asp:GridView ID="gv_documenti_trasporto" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_documenti_trasporto_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_documenti_trasporto_PageIndexChanging" PageSize="20"  AllowSorting="true" OnSorting="gv_documenti_trasporto_Sorting" AutoGenerateColumns="false" DataKeyNames="id">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                    <Columns>

                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Allegato" ImageAlign="AbsMiddle" Height="30px" OnClick="btnOpenDoc_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="numeroDocumentoTrasporto" HeaderText="Numero Doc. Trasporto" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="dataTrasporto" HeaderText="Data Trasporto" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="numero_protocollo" HeaderText="Protocollo" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="causale" HeaderText="Causale" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="destinatario" HeaderText="Destinatario"  HeaderStyle-Width="22%" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="partitaIva" HeaderText="Partita Iva"  HeaderStyle-Width="11%" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="trasportatore" HeaderText="Trasportatore" HeaderStyle-Width="22%" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza DocumentoTrasporto" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaDocumentoTrasporto" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button runat="server" ID="btnEditDocumentoTrasporto" Style="display: none" OnClick="btnEditDocumentoTrasporto_Click" />
    <asp:Button runat="server" ID="btnInsDocumentoTrasporto" Style="display: none" OnClick="btnInsDocumentoTrasporto_Click" />
    <asp:Button runat="server" ID="btnInsAttrezzaturaMagazzino" Style="display: none" OnClick="btnInsAttrezzaturaMagazzino_Click" />
    

    <asp:Button runat="server" ID="btnCercaCliente" Style="display: none" OnClick="btnCercaCliente_Click" />

    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopup_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupClientiServer" Style="display: none" OnClick="btnChiudiPopupClientiServer_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupMagazzinoServer" Style="display: none" OnClick="btnChiudiPopupMagazzinoServer_Click" />


    <asp:Button runat="server" ID="btnAssociaClienteServer" Style="display: none" OnClick="btnAssociaClienteServer_Click" />
    <asp:Button runat="server" ID="btnAssociaMagazzinoServer" Style="display: none" OnClick="btnAssociaMagazzinoServer_Click" />

    <asp:HiddenField ID="hf_idDocTras" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="DocumentoTrasporto" />

    <asp:UpdatePanel ID="upColl" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE DOCUMENTI TRASPORTO
                    </div>
                    <br />

                    <div class="w3-container">
                        <!-- ELENCO TAB DETTAGLI DocumentoTrasporto -->
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioDocumentoTrasporto('DocumentoTrasporto')">DocumentoTrasporto</div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <asp:ImageButton ID="btnViewAttachement" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Allegato" ImageAlign="AbsMiddle" Height="30px" OnClick="btnViewAttachement_Click" />
                            </div>
                        </div>
                    </div>
                    <!-- TAB DOCUMENTO TRASPORTO -->
                    <div id="DocumentoTrasporto" class="w3-container w3-border prot" style="display: block">
                        <div class="w3-container">
                            <p>
                                <div class="w3-row-padding">
                                     <div class="w3-quarter">
                                        <label>Destinatario</label>
                                        <div class="w3-row">
                                            <div class="w3-threequarter">
                                                <asp:TextBox ID="tbMod_destinatario" runat="server" MaxLength="60" CssClass="w3-input w3-border" placeholder="" Text="" ReadOnly="true"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="tbMod_IdCliente" />
                                            </div>
                                            <div class="w3-quarter">
                                                <asp:ImageButton ID="imgbtnSelectCliente" ImageUrl="~/Images/Search.ico" runat="server" class="w3-input w3-round w3-margin-left" Height="40px" Width="40px" ToolTip="Cerca Destinatario" OnClientClick="cercaCliente()" />
                                            </div>
                                        </div>
                                    </div>                                   
                                    <div class="w3-quarter">
                                        <label>Data Trasporto</label>
                                        <asp:TextBox ID="tbMod_DataTrasporto" runat="server" MaxLength="10" CssClass="w3-input w3-border calendar" placeholder="GG/MM/AAAA" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Partita iva</label>
                                        <asp:TextBox ID="tbMod_PartitaIva" runat="server" MaxLength="11" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>                                    
                                    <div class="w3-quarter">
                                        <label>Numero Documento Trasporto</label>
                                        <asp:TextBox ID="tbMod_NumeroDocumentoTrasporto" runat="server" MaxLength="30" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbIdDocumentoTrasportoDaModificare" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="tbMod_NumeroProtocollo" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Tipo</label>
                                        <asp:DropDownList ID="cmbMod_TipoIndirizzo" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                            <asp:ListItem Value="Via">Via</asp:ListItem>
                                            <asp:ListItem Value="Viale">Viale</asp:ListItem>
                                            <asp:ListItem Value="Corso">Corso</asp:ListItem>
                                            <asp:ListItem Value="Piazza">Piazza</asp:ListItem>
                                            <asp:ListItem Value="Piazzale">Piazzale</asp:ListItem>
                                            <asp:ListItem Value="Largo">Largo</asp:ListItem>
                                            <asp:ListItem Value ="Vicolo">Vicolo</asp:ListItem>
                                            <asp:ListItem Value ="Altro">Altro</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="w3-half">
                                        <label>Indirizzo</label>
                                        <asp:TextBox ID="tbMod_Indirizzo" runat="server" MaxLength="60" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Numero Civico</label>
                                        <asp:TextBox ID="tbMod_NumeroCivico" runat="server" MaxLength="10" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Cap</label>
                                        <asp:TextBox ID="tbMod_Cap" runat="server" MaxLength="5" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Comune</label>
                                        <asp:TextBox ID="tbMod_Comune" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Provincia</label>
                                        <asp:TextBox ID="tbMod_Provincia" runat="server" MaxLength="2" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Nazione</label>
                                        <asp:TextBox ID="tbMod_Nazione" runat="server" MaxLength="20" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>

                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Causale</label>
                                        <asp:TextBox ID="tbMod_Causale" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Numero Colli</label>
                                        <div class="w3-row">
                                            <div class="w3-half">
                                                <asp:TextBox ID="tbMod_NumeroColli" runat="server" MaxLength="2" placeholder="" Text="1" CssClass="w3-input w3-border" ></asp:TextBox>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" TargetControlID="tbMod_NumeroColli" Minimum="1" Maximum="99" Width="100" TargetButtonDownID="btnGiu" TargetButtonUpID="btnSu" ></ajaxToolkit:NumericUpDownExtender>
                                            </div>
                                            <div class="w3-half">
                                                &nbsp;<asp:Button runat="server" ID="btnSu" Text="+" Width="40px" Height="40px" CssClass="w3-btn w3-white w3-border w3-border-green w3-round-large" />&nbsp;<asp:Button runat="server" ID="btnGiu" Text="-" Width="40px" Height="40px" CssClass="w3-btn w3-white w3-border w3-border-green w3-round-large" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Peso</label>
                                        <asp:TextBox ID="tbMod_Peso" runat="server" MaxLength="10" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Trasportatore</label>
                                        <asp:TextBox ID="tbMod_Trasportatore" runat="server" MaxLength="60" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>

                                <br />
                                <div id="divBtnInserisciAttrezzatura" runat="server">
                                    <div id="clbtnInserisciAttrezzatura" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciAttrezzaturaMagazzino();" >Inserisci Attrezzatura</div>
                                </div>
                                <br />
                                <div class="round">
                
                                    <asp:GridView ID="gv_attrezzature" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_attrezzature_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_attrezzature_PageIndexChanging" OnRowCommand="gv_attrezzature_RowCommand" PageSize="20" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False" HeaderStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/delete.png" ToolTip="Cancella Riga" ImageAlign="AbsMiddle" Height="30px" CommandName="ELIMINA_RIGA" CommandArgument='<%#Eval("id")%>' OnClientClick="return confirm('Confermi cancellazione attrezzatura?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                    
                                            <%--COLONNE CON ID NON VISIBILI--%>
                                            <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" Visible="false"/>
                        
                                            <asp:BoundField DataField="cod_vs" HeaderText="Codice VS" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="seriale" HeaderText="Seriale" HeaderStyle-Width="30%" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="descrizione" HeaderText="Descrizione" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="quantita" HeaderText="Quantita" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"/>
                    
                                        </Columns>
                                    </asp:GridView>
                                </div>

                                <br />
                                <asp:Label ID="lblStatus" runat="server" Style="font-family: Arial; font-size: small;"></asp:Label>
                                <br />
                                <div style="text-align: center;">
                                    <asp:Button ID="btnGestisciDocumentoTrasporto" runat="server" Text="Gestisci DocumentoTrasporto" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciDocumentoTrasporto_Click" />
                                    <asp:Button ID="btnInserisciDocumentoTrasporto" runat="server" Text="Inserisci DocumentoTrasporto" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciDocumentoTrasporto_Click" OnClientClick="return confirm('Confermi inserimento DocumentoTrasporto?')" />
                                    <asp:Button ID="btnModificaDocumentoTrasporto" runat="server" Text="Modifica DocumentoTrasporto" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaDocumentoTrasporto_Click" OnClientClick="return confirm('Confermi modifica DocumentoTrasporto?')" Visible="false" />
                                    <asp:Button ID="btnEliminaDocumentoTrasporto" runat="server" Text="Elimina DocumentoTrasporto" class="w3-panel w3-green w3-border w3-round" OnClick="btnEliminaDocumentoTrasporto_Click" OnClientClick="return confirm('Confermi eliminazione DocumentoTrasporto?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaDocumentoTrasporto" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaDocumentoTrasporto_Click" />
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>

            <!-- POPUP RICERCA CLIENTI -->
            <asp:Panel runat="server" ID="PanelClienti" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="PanelContLavorazioni" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <br />
                    <div class="w3-container w3-padding w3-margin">
                        <!-- RICERCA CLIENTI -->
                        <div class="w3-bar w3-orange w3-round">
                            <div class="w3-bar-item w3-button w3-orange">Ricerca Clienti\Fornitori - Collaboratori
                            </div>
                            <div class="w3-bar-item w3-button w3-orange w3-right">
                                <div id="btnChiudiPopupClienti" class="w3-button w3-orange w3-small w3-round" onclick="chiudiPopupClienti();">Chiudi</div>
                            </div>
                        </div>
                    </div>
                    <div class="w3-row-padding w3-padding w3-margin">
                        <div class="w3-half">
                            <label>Cliente\Fornitore - Collaboratore</label>
                            <asp:DropDownList ID="ddlSceltaClienteCollaboratore" runat="server" Width="100%" class="w3-input w3-border">
                                <asp:ListItem Value="Cliente" Text="Cliente\Fornitore"></asp:ListItem>
                                <asp:ListItem Value="Collaboratore" Text="Collaboratore"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="w3-half">
                            &nbsp;
                        </div>
                    </div>  
                    <div class="w3-row-padding w3-padding w3-margin">
                        <div class="w3-threequarter">
                            <label>Ragione Sociale</label>
                            <asp:TextBox ID="tbSearch_RagioneSociale" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>&nbsp;</label>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnInsertCliente" runat="server" class="w3-btn w3-white w3-border w3-border-blue w3-round-large" OnClientClick="inserisciCliente();" Text="Inserisci" />
                                    </td>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnRicercaClienti" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaClienti_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                                    </td>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnAzzeraCampiRicercaClienti" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicercaClienti();" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="round w3-padding w3-margin">
                        <asp:GridView ID="gvClienti" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gvClienti_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvClienti_PageIndexChanging" PageSize="20">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Seleziona Destinatario" ImageAlign="AbsMiddle" Height="30px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
            </asp:Panel>
            <!-- FINE POPUP RICERCA CLIENTI -->

            <!-- POPUP RICERCA MAGAZZINO -->
            <asp:Panel runat="server" ID="PanelMagazzino" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="Panel2" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <br />
                    <div class="w3-container w3-padding w3-margin">
                        <!-- RICERCA MAGAZZINO -->
                        <div class="w3-bar w3-orange w3-round">
                            <div class="w3-bar-item w3-button w3-orange">Ricerca Magazzino
                            </div>
                            <div class="w3-bar-item w3-button w3-orange w3-right">
                                <div id="btnChiudiPopupMagazzino" class="w3-button w3-orange w3-small w3-round" onclick="chiudiPopupMagazzino();">Chiudi</div>
                            </div>
                        </div>
                    </div>
                    <div class="w3-row-padding w3-padding w3-margin">
                        <div class="w3-quarter">
                            <label>Codice Videosystem</label>
                            <asp:TextBox ID="tbSearch_CodiceVideosystem" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Seriale</label>
                            <asp:TextBox ID="tbSearch_Seriale" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div> 
                        <div class="w3-quarter">
                            <label>Descrizione</label>
                            <asp:TextBox ID="tbSearch_DescMagazzino" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div> 
                        <div class="w3-quarter">
                            <label>Quantità</label>
                            <asp:TextBox ID="tbIns_Quantita" runat="server" MaxLength="3" class="w3-input w3-border" placeholder="" Text="1"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="tbIns_Quantita_MaskedEditExtender" runat="server" TargetControlID="tbIns_Quantita" MaskType="None" Mask="999" ></ajaxToolkit:MaskedEditExtender>
                        </div>   
                    </div>
                    <div class="w3-row-padding w3-padding w3-margin">
                        <div class="w3-quarter">
                            <label>Categoria</label>
                            <asp:DropDownList ID="ddlTipoCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoCategoria_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="w3-quarter">
                            <label>Sub Categoria</label>
                            <asp:DropDownList ID="ddlTipoSubCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoSubCategoria_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="w3-quarter">
                            <label>Gruppo</label>
                            <asp:DropDownList ID="ddlTipoGruppoMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                            </asp:DropDownList>   
                        </div>
                        <div class="w3-quarter">
                            <label>&nbsp;</label>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnInsertMagazzino" runat="server" class="w3-btn w3-white w3-border w3-border-blue w3-round-large" OnClick="btnInsertMagazzino_Click" Text="Inserisci" />
                                    </td>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnRicercaMagazzino" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaMagazzino_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                                    </td>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnAzzeraCampiRicercaMagazzino" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicercaMagazzino();" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    


                    <div class="round w3-padding w3-margin">
                        <asp:GridView ID="gvMagazzino" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gvMagazzino_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvMagazzino_PageIndexChanging" PageSize="20" DataKeyNames="id">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelectMagazzino" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Seleziona Magazzino" ImageAlign="AbsMiddle" Height="30px" OnClick="imgSelectMagazzino_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
            </asp:Panel>
            <!-- FINE POPUP RICERCA MAGAZZINO -->



        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnInserisciDocumentoTrasporto" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnModificaDocumentoTrasporto" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEliminaDocumentoTrasporto" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Protocollo.aspx.cs" Inherits="VideoSystemWeb.Protocollo.Protocollo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaProtocollo.ClientID%>").click();
                    }
                    else {
                        $("#<%=btnRicercaLavorazioni.ClientID%>").click();
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

        // APRO POPUP VISUALIZZAZIONE/MODIFICA PROTOCOLLO
        function mostraProtocollo(row) {
            $('.loader').show();
            $("#<%=hf_idProt.ClientID%>").val(row);
            $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
            $("#<%=btnEditProtocollo.ClientID%>").click();
        }
        // APRO POPUP DI INSERIMENTO PROTOCOLLO
        function inserisciProtocollo() {
            $('.loader').show();
            $("#<%=hf_idProt.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
            $("#<%=btnInsProtocollo.ClientID%>").click();
        }

        // APRO POPUP RICERCA CLIENTE
        function cercaCliente() {
            $('.loader').show();
            $("#<%=btnCercaCliente.ClientID%>").click();
        }

        // APRO POPUP RICERCA LAVORAZIONE
        function cercaLavorazione() {
            $('.loader').show();
            $("#<%=btnCercaLavorazione.ClientID%>").click();
        }

        // APRO LE TAB DETTAGLIO PROTOCOLLO
        function openDettaglioProtocollo(tipoName) {
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
            // QUANDO APRO IL POPUP RIPARTE SEMPRE DA PROTOCOLLO E NON DALL'ULTIMA TAB APERTA
            $("#<%=hf_tabChiamata.ClientID%>").val('Protocollo');
            $("#<%=hf_idProt.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('VISUALIZZAZIONE');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();

        }

        function chiudiPopupClienti() {
            $("#<%=btnChiudiPopupClientiServer.ClientID%>").click();
        }

        function chiudiPopupLavorazioni() {
            $("#<%=btnChiudiPopupLavorazioniServer.ClientID%>").click();
        }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCodiceLavoro.ClientID%>").val('');
            $("#<%=tbRagioneSociale.ClientID%>").val('');
            $("#<%=tbNumeroProtocollo.ClientID%>").val('');
            $("#<%=tbDataProtocollo.ClientID%>").val('');
            $("#<%=tbDataProtocolloA.ClientID%>").val('');
            $("#<%=tbDataLavorazione.ClientID%>").val('');
            $("#<%=tbDataLavorazioneA.ClientID%>").val('');
            $("#<%=tbProduzione.ClientID%>").val('');
            $("#<%=tbProtocolloRiferimento.ClientID%>").val('');
            $("#<%=tbLavorazione.ClientID%>").val('');
            $("#<%=tbDescrizione.ClientID%>").val('');
            $("#<%=ddlTipoProtocollo.ClientID%>").val('');
            $("#<%=ddlDestinatario.ClientID%>").val('');
        }

        function azzeraCampiRicercaLavorazione() {
            $("#<%=tbSearch_Cliente.ClientID%>").val('');
            $("#<%=tbSearch_CodiceLavoro.ClientID%>").val('');
            $("#<%=tbSearch_DataFine.ClientID%>").val('');
            $("#<%=tbSearch_DataInizio.ClientID%>").val('');
            $("#<%=tbSearch_Lavorazione.ClientID%>").val('');
            $("#<%=tbSearch_Luogo.ClientID%>").val('');
            $("#<%=tbSearch_Produzione.ClientID%>").val('');

        }

        function uploadError(sender, args) {
            //alert('errore ');
            $('.loader').hide();
            document.getElementById('<%=lblStatus.ClientID%>').innerText = 'Errore sul file ' + args.get_fileName() + ' - ' + args.get_errorMessage();
        }

        function StartUpload(sender, args) {
            //alert('start upload');
            $('.loader').show();
            document.getElementById('<%=lblStatus.ClientID%>').innerText = 'Caricamento Iniziato.';
        }

        function UploadComplete(sender, args) {
            $('.loader').hide();
            //alert('Caricamento Completato');
            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            var text = "La dimensione del file " + filename + " è " + args.get_length() + " bytes";
            if (contentType.length > 0) {
                text += " e la tipologia è '" + contentType + "'.";
            }
            document.getElementById('<%=lblStatus.ClientID%>').innerText = '';
            document.getElementById('<%=tbMod_NomeFile.ClientID%>').innerText = filename;
        }

        function associaCodiceLavorazione(codLav, cliente, produzione, lavorazione) {
            document.getElementById('<%=tbMod_CodiceLavoro.ClientID%>').value = codLav;
            //document.getElementById('<%=tbMod_Cliente.ClientID%>').value = cliente;
            document.getElementById('<%=tbMod_Produzione.ClientID%>').value = produzione;
            document.getElementById('<%=tbMod_Lavorazione.ClientID%>').value = lavorazione;
            chiudiPopupLavorazioni();
        }

        function associaCliente(idCli, cliente) {
            //alert('ASSOCIA CLIENTE prima - CLIENTE ' +  cliente);
            //alert('ASSOCIA CLIENTE prima - ID CLIENTE ' +  idCli);

            var txt1 = $("#<%=tbMod_Cliente.ClientID%>");
            var txt2 = $("#<%=tbMod_IdCliente.ClientID%>");
            txt1.val(cliente);
            txt2.val(idCli);
            txt1.text = cliente;
            txt2.text = idCli;

            //alert('ASSOCIA CLIENTE dopo - CLIENTE ' + txt1.text);
            //alert('ASSOCIA CLIENTE dopo - ID CLIENTE ' + txt2.text);
            $("#<%=btnAssociaClienteServer.ClientID%>").click();
        }

        function inserisciCliente() {
            //alert('' + document.getElementById('<%=tbSearch_RagioneSociale.ClientID%>').value);
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
    <label><asp:Label ID="lblProtocolli" runat="server" Text="PROTOCOLLI" ForeColor="Teal"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Codice Lavoro</label>
                    <asp:TextBox ID="tbCodiceLavoro" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Numero Protocollo</label>
                    <asp:TextBox ID="tbNumeroProtocollo" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Riferimento Documento</label>
                    <asp:TextBox ID="tbProtocolloRiferimento" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Tipo</label>
                    <asp:DropDownList ID="ddlTipoProtocollo" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Produzione</label>
                    <asp:TextBox ID="tbProduzione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Lavorazione</label>
                    <asp:TextBox ID="tbLavorazione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Lav. (Da-A)</label>
                    <table style="width:100%;">
                        <tr>
                            <td style="position:relative;"><asp:TextBox ID="tbDataLavorazione" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                            <td style="position:relative;"><asp:TextBox ID="tbDataLavorazioneA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="w3-quarter">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="200" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-quarter">
                    <label>Cliente/Fornitore</label>
                    <asp:TextBox ID="tbRagioneSociale" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Destinatario</label>
                    <asp:DropDownList ID="ddlDestinatario" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                        <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Prot. (Da-A)</label>
                    <table style="width:100%;">
                        <tr>
                            <td style="position:relative;"><asp:TextBox ID="tbDataProtocollo" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                            <td style="position:relative;"><asp:TextBox ID="tbDataProtocolloA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Button ID="btnRicercaProtocollo" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaProtocollo_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 40%;">
                                <div id="divBtnInserisciProtocollo" runat="server">
                                    <div id="clbtnInserisciProtocollo" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciProtocollo();" >Inserisci</div>
                                </div>

                            </td>
                            <td style="width: 20%;">
                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" OnClick="BtnPulisciCampiRicerca_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="round">
                <asp:GridView ID="gv_protocolli" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_protocolli_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_protocolli_PageIndexChanging" PageSize="20"  AllowSorting="true" OnSorting="gv_protocolli_Sorting" AutoGenerateColumns="false">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last"/>
                    <Columns>

                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Allegato" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Cod. Lav." HeaderText="Cod. Lav." HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Num. Prot." HeaderText="Num. Prot."  HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Data Prot." HeaderText="Data Prot." DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Cliente/Fornitore" HeaderText="Cliente/Fornitore"  HeaderStyle-Width="16%" />
                        <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="13%" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" HeaderStyle-Width="15%" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Nome File" HeaderText="Nome File" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Destinatario" HeaderText="Destinatario" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Pregresso" HeaderText="Pregresso" HeaderStyle-Width="6%" />

                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Protocollo" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaProtocollo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button runat="server" ID="btnEditProtocollo" Style="display: none" OnClick="btnEditProtocollo_Click" />
    <asp:Button runat="server" ID="btnInsProtocollo" Style="display: none" OnClick="btnInsProtocollo_Click" />
    <asp:Button runat="server" ID="btnCercaCliente" Style="display: none" OnClick="btnCercaCliente_Click" />
    <asp:Button runat="server" ID="btnCercaLavorazione" Style="display: none" OnClick="btnCercaLavorazione_Click" />

    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopup_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupClientiServer" Style="display: none" OnClick="btnChiudiPopupClientiServer_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupLavorazioniServer" Style="display: none" OnClick="btnChiudiPopupLavorazioniServer_Click" />


    <asp:Button runat="server" ID="btnAssociaClienteServer" Style="display: none" OnClick="btnAssociaClienteServer_Click" />

    <asp:HiddenField ID="hf_idProt" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Protocollo" />

    <asp:UpdatePanel ID="upColl" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE PROTOCOLLI
                    </div>
                    <br />

                    <div class="w3-container">
                        <!-- ELENCO TAB DETTAGLI PROTOCOLLO -->
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioProtocollo('Protocollo')">Protocollo</div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <asp:ImageButton ID="btnViewAttachement" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Allegato" ImageAlign="AbsMiddle" Height="30px" />
                            </div>
                        </div>
                    </div>
                    <!-- TAB PROTOCOLLI -->
                    <div id="Protocollo" class="w3-container w3-border prot" style="display: block">
                        <%--<label>Protocolli</label>--%>
                        <div class="w3-container">
                            <p>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Codice Lavorazione</label>
                                        <div class="w3-row">
                                            <div class="w3-threequarter">
                                                <asp:TextBox ID="tbMod_CodiceLavoro" runat="server" MaxLength="30" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <asp:ImageButton ID="imgbtnSelectCodLav" ImageUrl="~/Images/Search.ico" runat="server" class="w3-input w3-round w3-margin-left" Height="40px" Width="40px" ToolTip="Cerca Codice Lavorazione" OnClick="imgbtnSelectCodLav_Click" OnClientClick="cercaLavorazione()" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Numero Protocollo</label>
                                        <asp:TextBox ID="tbMod_NumeroProtocollo" runat="server" MaxLength="30" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbIdProtocolloDaModificare" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Riferimento Documento</label>
                                        <asp:TextBox ID="tbMod_ProtocolloRiferimento" runat="server" MaxLength="20" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Tipo</label>
                                        <asp:DropDownList ID="cmbMod_Tipologia" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Produzione</label>
                                        <asp:TextBox ID="tbMod_Produzione" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Lavorazione</label>
                                        <asp:TextBox ID="tbMod_Lavorazione" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter" style="position: relative">
                                        <label>Data Lavorazione</label>
                                        <asp:TextBox ID="tbMod_DataLavorazione" runat="server" MaxLength="10" CssClass="w3-input w3-border calendar" placeholder="GG/MM/AAAA" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbMod_Descrizione" runat="server" MaxLength="200" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Cliente/Fornitore</label>
                                        <div class="w3-row">
                                            <div class="w3-threequarter">
                                                <asp:TextBox ID="tbMod_Cliente" runat="server" CssClass="w3-input w3-border" Text=""></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="tbMod_IdCliente" />
                                            </div>
                                            <div class="w3-quarter">
                                                <asp:ImageButton ID="imgbtnSelectCliente" ImageUrl="~/Images/Search.ico" runat="server" class="w3-input w3-round w3-margin-left" Height="40px" Width="40px" ToolTip="Cerca Cliente" OnClientClick="cercaCliente()" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Destinatario</label>
                                        <asp:DropDownList ID="cmbMod_Destinatario" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                            <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                                            <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="cbMod_Pregresso" runat="server" Visible="false" Checked="false" />
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Data Prot</label>
                                        <asp:TextBox ID="tbMod_DataProtocollo" runat="server" MaxLength="10" CssClass="w3-input w3-border" placeholder="GG/MM/AAAA" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>&nbsp;</label>
                                    </div>
                                </div>

                                <div class="w3-row-padding">
                                    <div class="w3-threequarter">
                                        <label>Nome File</label>
                                        <asp:TextBox ID="tbMod_NomeFile" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>&nbsp;</label>
                                    </div>
                                </div>
                                <div class="w3-row-padding">
                                    <div class="w3-threequarter">
                                        <label>&nbsp;</label>
                                        <ajaxToolkit:AsyncFileUpload ID="fuFileProt"
                                            runat="server"
                                            OnClientUploadError="uploadError"
                                            OnClientUploadStarted="StartUpload"
                                            OnClientUploadComplete="UploadComplete"
                                            UploaderStyle="Traditional"
                                            OnUploadedComplete="AsyncFileUpload1_UploadedComplete"
                                            OnUploadedFileError="AsyncFileUpload1_UploadedFileError"
                                            ClientIDMode="AutoID"
                                            Width="99%"
                                            class="w3-input w3-border w3-round" />
                                    </div>
                                    <div class="w3-quarter">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnAnnullaCaricamento" runat="server" class="w3-input w3-border w3-circle w3-red  w3-center" Width="50px" Text="&times;" ToolTip="Annulla Caricamento File" OnClick="btnAnnullaCaricamento_Click" />
                                    </div>

                                </div>
                                <br />
                                <asp:Label ID="lblStatus" runat="server" Style="font-family: Arial; font-size: small;"></asp:Label>
                                <br />
                                <div style="text-align: center;">
                                    <asp:Button ID="btnGestisciProtocollo" runat="server" Text="Gestisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciProtocollo_Click" />
                                    <asp:Button ID="btnInserisciProtocollo" runat="server" Text="Inserisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciProtocollo_Click" OnClientClick="return confirm('Confermi inserimento Protocollo?')" />
                                    <asp:Button ID="btnModificaProtocollo" runat="server" Text="Modifica Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaProtocollo_Click" OnClientClick="return confirm('Confermi modifica Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnEliminaProtocollo" runat="server" Text="Elimina Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnEliminaProtocollo_Click" OnClientClick="return confirm('Confermi eliminazione Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaProtocollo" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaProtocollo_Click" />
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <!-- POPUP RICERCA LAVORAZIONI -->
            <asp:Panel runat="server" ID="PanelLavorazioni" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="Panel3" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container">
                        <!-- RICERCA LAVORAZIONI -->
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow">Ricerca Lavorazioni</div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <div id="btnChiudiPopupLavorazioni" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopupLavorazioni();">Chiudi</div>
                            </div>
                        </div>
                    </div>

                    <div class="w3-row-padding">
                        <div class="w3-quarter">
                            <label>Codice Lavoro</label>
                            <asp:TextBox ID="tbSearch_CodiceLavoro" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Cliente/Fornitore</label>
                            <asp:TextBox ID="tbSearch_Cliente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Luogo</label>
                            <asp:TextBox ID="tbSearch_Luogo" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Produzione</label>
                            <asp:TextBox ID="tbSearch_Produzione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="w3-row-padding">
                        <div class="w3-quarter">
                            <label>Lavorazione</label>
                            <asp:TextBox ID="tbSearch_Lavorazione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                        </div>
                        <div class="w3-quarter" style="position: relative">
                            <label>Data da</label>
                            <asp:TextBox ID="tbSearch_DataInizio" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                        <div class="w3-quarter" style="position: relative">
                            <label>Data a</label>
                            <asp:TextBox ID="tbSearch_DataFine" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>&nbsp;</label>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnRicercaLavorazioni" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaLavorazioni_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                                    </td>
                                    <td style="width: 50%;">
                                        <asp:Button ID="btnAzzeraCampiRicercaLavorazioni" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicercaLavorazione();" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="round">
                        <asp:GridView ID="gvLavorazioni" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gvLavorazioni_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvLavorazioni_PageIndexChanging" PageSize="20">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Seleziona Lavorazione" ImageAlign="AbsMiddle" Height="30px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <!-- FINE POPUP RICERCA LAVORAZIONI -->

            <!-- POPUP RICERCA CLIENTI -->
            <asp:Panel runat="server" ID="PanelClienti" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="PanelContLavorazioni" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container">
                        <!-- RICERCA CLIENTI -->
                        <div class="w3-bar w3-orange w3-round">
                            <div class="w3-bar-item w3-button w3-orange">Ricerca Clienti</div>
                            <div class="w3-bar-item w3-button w3-orange w3-right">
                                <div id="btnChiudiPopupClienti" class="w3-button w3-orange w3-small w3-round" onclick="chiudiPopupClienti();">Chiudi</div>
                            </div>
                        </div>
                    </div>

                    <div class="w3-row-padding">
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
                    <div class="round">
                        <asp:GridView ID="gvClienti" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gvClienti_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvClienti_PageIndexChanging" PageSize="20">
                            <Columns>
                                <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSelect" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Seleziona Cliente" ImageAlign="AbsMiddle" Height="30px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <!-- FINE POPUP RICERCA CLIENTI -->


        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnInserisciProtocollo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnModificaProtocollo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEliminaProtocollo" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

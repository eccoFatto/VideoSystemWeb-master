<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scadenzario.aspx.cs" Inherits="VideoSystemWeb.Scadenzario.userControl.Scadenzario" EnableViewState="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaScadenza.ClientID%>").click();
                    }
                    else {
                        $("#<%=btnRicercaScadenza.ClientID%>").click();
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

            $('.calendarAggiungiPagamento').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY',
                useCurrent: false
            });
            
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendarAggiungiPagamento').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY',
                    useCurrent: false
                });

                $('.calendarAggiungiPagamento').on('dp.change', function (e) {
                    modificaLabelValoriAcconto();
                });
            });



        });

        // APRO POPUP VISUALIZZAZIONE/MODIFICA SCADENZA
        function mostraScadenza(row) {
            
            $('.loader').show();
            $("#<%=hf_idScadenza.ClientID%>").val(row);
            $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
            $("#<%=btnEditScadenza.ClientID%>").click();
        }

        // APRO POPUP DI INSERIMENTO SCADENZA
        function inserisciScadenza() {
            $('.loader').show();
            $("#<%=hf_idScadenza.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
            $("#<%=btnInsScadenza.ClientID%>").click();
        }

        function chiudiPopup() {
            $("#<%=hf_tipoOperazione.ClientID%>").val('VISUALIZZAZIONE');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();

        }

        function popupScadenzario(messaggio) {
            document.getElementById('popMessage').style.display = 'block';
            $('#textPopMess').html(messaggio);
        }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=ddl_TipoAnagrafica.ClientID%>").val('');
            <%--$("#<%=hf_RagioneSociale.ClientID%>").val('');--%>
            $("#<%=txt_RagioneSociale.ClientID%>").val('');
            $("#<%=txt_NumeroFattura.ClientID%>").val('');
            <%--$("#<%=ddlFatturaPagata.ClientID%>").val('');--%>
            $("#<%=txt_DataFatturaDa.ClientID%>").val('');
            $("#<%=txt_DataFatturaA.ClientID%>").val('');
            $("#<%=txt_DataScadenzaDa.ClientID%>").val('');
            $("#<%=txt_DataScadenzaA.ClientID%>").val('');
            $("#<%=ddlFatturaPagata.ClientID%>").val('0');
            $("#<%=ddl_FiltroBanca.ClientID%>").val('');
        }

        function confermaEliminazioneScadenza() {
            return confirm("Verranno eliminate tutte le voci dello scadenzario riguardanti la fattura cui si riferisce la voce selezionata. Confermare?");
        }

        function calcolaScorporoIva() {
            var importoIva = $('#<%=txt_ImportoIva.ClientID%>').val().replace(".", "").replace(",", ".");
            var iva = $('#<%=txt_Iva.ClientID%>').val().replace(".", "").replace(",", ".");;
            var importo = importoIva / (1 + (iva / 100));

            $('#<%=txt_ImportoDocumento.ClientID%>').val(importo.toFixed(2).replace(".", ","));
        }

        function calcolaScorporoModifica() {
            var importoIva = $('#<%=txt_VersatoIva.ClientID%>').val().replace(".", "").replace(",", ".");            
            var iva = $('#<%=txt_IvaModifica.ClientID%>').val().replace(".", "").replace(",", ".");
            var importo = importoIva / (1 + (iva / 100));
            
            var imponibile = $('#<%=txt_Imponibile.ClientID%>').val().replace(".", "").replace(",", ".");
            var imponibileIva = $('#<%=txt_ImponibileIva.ClientID%>').val().replace(".", "").replace(",", ".");
        
            $('#<%=txt_Versato.ClientID%>').val(importo.toFixed(2).replace(".", ","));
           
            $('#<%=txt_Totale.ClientID%>').val((imponibile - importo).toFixed(2).replace(".", ","));
            $('#<%=txt_TotaleIva.ClientID%>').val((imponibileIva - importoIva).toFixed(2).replace(".", ","));
        }

        <%--Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        // GESTIONE DROPDOWN RAGIONE SOCIALE
            $("#filtroRagioneSociale").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divRagioneSociale .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoRagioneSociale.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_RagioneSociale.ClientID%>").val($(e.target).text());
                $("#<%=ddl_RagioneSociale.ClientID%>").val($(e.target).text());
                $("#<%=ddl_RagioneSociale.ClientID%>").attr("title", $(e.target).text());
            });
        });--%>

        function modificaLabelValoriAcconto() {
            var stringaAcconto = $('#<%=txt_VersatoAccontoIva.ClientID%>').val().replace(",", ".");
            var acconto = parseFloat(stringaAcconto);//.toFixed(2);
            //alert(acconto);
            
            var data = $('#<%=txt_DataVersamentoRiscossione.ClientID%>').val();
            var parts = data.split('/');
            var mydate = new Date(parts[2], parts[1] - 1, parts[0]);
            var dataNuovaRata = mydate.addDays(30);
            var dataNuovaRataFormattata = ((dataNuovaRata.getDate() > 9) ? dataNuovaRata.getDate() : ('0' + dataNuovaRata.getDate())) + '/' + ((dataNuovaRata.getMonth() > 8) ? (dataNuovaRata.getMonth() + 1) : ('0' + (dataNuovaRata.getMonth() + 1))) + '/' + dataNuovaRata.getFullYear();

            var stringaMaxImporto = $('#<%=hf_importoScadenzaFigli.ClientID%>').val().replace(",", ".");
            var maxImporto = parseFloat(stringaMaxImporto);//.toFixed(2);
            
            var differenzaImporto = (maxImporto - acconto).toFixed(2);
            //alert(differenzaImporto);

            //alert(acconto < maxImporto);
            
            if (acconto > 0 && acconto < maxImporto && data != '') {

                $('#<%=lbl_ValoriAcconto.ClientID%>').html('Verrà creata una nuova rata di <b>' + differenzaImporto.replace(".", ",") + '€</b> con scadenza <b>' + dataNuovaRataFormattata + "</b>");
            }
            else {
                $('#<%=lbl_ValoriAcconto.ClientID%>').html("Indicare l'importo e la data di versamento o riscossione dell'acconto");
            }
        }

        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
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
    <label><asp:Label ID="lblScadenzario" runat="server" Text="SCADENZARIO" ForeColor="Teal"></asp:Label></label>

    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Cliente/Fornitore</label>
                    <asp:TextBox ID="txt_RagioneSociale" runat="server" class="w3-input w3-border" ></asp:TextBox>

                    <%--<div id="divRagioneSociale" class="dropdown ">
                        <asp:HiddenField ID="hf_RagioneSociale" runat="server" Value="" />
                        <asp:Button ID="ddl_RagioneSociale" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divClienti" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoRagioneSociale" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px">
                            <input class="form-control" id="filtroRagioneSociale" type="text" placeholder="Cerca..">
                        </ul>
                    </div>--%>

                </div>

                <div class="w3-quarter">
                    <label>Tipo (Cliente/Fornitore)</label>
                    <asp:DropDownList ID="ddl_TipoAnagrafica" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" Selected></asp:ListItem>
                        <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                        <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="w3-quarter">
                    <label>Numero Fattura</label>
                    <asp:TextBox ID="txt_NumeroFattura" runat="server" MaxLength="20" class="w3-input w3-border" ></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Pagata</label>
                    <asp:DropDownList ID="ddlFatturaPagata" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" />
                        <asp:ListItem Value="1" Text="Si" />
                        <asp:ListItem Value="0" Text="No" Selected/>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="w3-row-padding" style="position:relative;">
                    <div class="w3-threequarter w3-row-padding">
                        <div class="w3-quarter">
                            <label>Data fattura da</label>
                            <asp:TextBox ID="txt_DataFatturaDa" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Data fattura a</label>
                            <asp:TextBox ID="txt_DataFatturaA" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
        
                        <div class="w3-quarter">
                            <label>Data scadenza da</label>
                            <asp:TextBox ID="txt_DataScadenzaDa" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                        <div class="w3-quarter">
                            <label>Data scadenza a</label>
                            <asp:TextBox ID="txt_DataScadenzaA" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                    </div>
                    <div class="w3-quarter">
                        <label>Tipologia pagamento</label>
                        <asp:DropDownList ID="ddl_FiltroBanca" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border"/>
                    </div>
            </div>

<%--RIQUADRI DEI TOTALI--%>
            <div class="w3-row-padding w3-margin-bottom w3-margin-top">
                <div class="w3-threequarter" style="font-size:.8em;">
                    <div class="w3-half" style="padding-right:10px;">
                        <div class="w3-row w3-padding round ">
                            <div class="w3-row">
                                <b>Fornitori</b>
                            </div>
                            <div class="w3-quarter">
                                &nbsp;
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:center">
                                    <b>Imponibile</b>
                                </div>
                                <div class="w3-half" style="text-align:center">
                                    <b>Imponibile + IVA</b>
                                </div>
                            </div>
                            <div class="w3-quarter">
                                <b>Dare</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_dare" runat="server"/>
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_dare_iva" runat="server"  />
                                </div>
                            </div>
                            <div class="w3-quarter">
                                <b>Versato</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_versato" runat="server" />
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_versato_iva" runat="server"/>
                                </div>
                            </div>
                            <div class="w3-quarter">
                                <b>Totale</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_totale_dare" runat="server"/>
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_totale_dare_iva" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="w3-half" style="padding-left:10px;">
                        <div class="w3-row w3-padding round ">
                            <div class="w3-row">
                                <b>Clienti</b>
                            </div>
                            <div class="w3-quarter">
                                &nbsp;
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:center">
                                    <b>Imponibile</b>
                                </div>
                                <div class="w3-half" style="text-align:center">
                                    <b>Imponibile + IVA</b>
                                </div>
                            </div>

                            <div class="w3-quarter">
                                <b>Avere</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_avere" runat="server"/>
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_avere_iva" runat="server"/>
                            </div>
                            </div>

                            <div class="w3-quarter">
                                <b>Riscosso</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_riscosso" runat="server"/>
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_riscosso_iva" runat="server"/>
                                </div>
                            </div>

                            <div class="w3-quarter">
                                <b>Totale</b>
                            </div>
                            <div class="w3-threequarter">
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_totale_avere" runat="server" />
                                </div>
                                <div class="w3-half" style="text-align:right; padding-right:25px">
                                    <asp:Label ID="lbl_totale_avere_iva" runat="server"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                

                <div class="w3-quarter">
                    <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaScadenza" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaScadenza_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciScadenza" runat="server"> 
                                <div id="clbtnInserisciScadenza" runat="server" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciScadenza();" >Inserisci</div>

                            </div>
                        </td>
                        <td style="width:20%;">
                             <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" OnClick="btnRicercaScadenza_Click"/>

                        </td>
                    </tr>
                </table>
                </div>

            </div>

            <div class="round">
                <asp:GridView ID="gv_scadenze" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" 
                    CssClass="grid" AllowPaging="True" OnPageIndexChanging="gv_scadenze_PageIndexChanging" PageSize="20" 
                    EmptyDataText="Nessuna scadenza trovata" EmptyDataRowStyle-HorizontalAlign="Center" OnRowCommand="gv_scadenze_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="30px" CommandName="visualizzaDoc" CommandArgument='<%#Eval("id")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ImportoDareIva" HeaderText="Dare" DataFormatString="{0:N2}" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ImportoVersatoIva" HeaderText="Versato" DataFormatString="{0:N2}" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ProtocolloRiferimento" HeaderText="Documento"  HeaderStyle-Width="12%" />
                        <asp:BoundField DataField="RagioneSocialeClienteFornitore" HeaderText="Nominativo"  HeaderStyle-Width="18%" />
                        <asp:BoundField DataField="ImportoAvereIva" HeaderText="Avere" DataFormatString="{0:N2}" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ImportoRiscossoIva" HeaderText="Riscosso" DataFormatString="{0:N2}" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="DataScadenza" HeaderText="Scadenza" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="IsImportoEstinto" HeaderText="Stato" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="Banca" HeaderText="Banca" HeaderStyle-Width="15%" />
                        <asp:BoundField DataField="DataPagamento" HeaderText="Pagamento" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/edit.png" ToolTip="Modifica" ImageAlign="AbsMiddle"  CommandName="modifica" CommandArgument='<%#Eval("id")%>'/>
                                <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" Text="Elimina" ImageUrl="~/Images/delete.png" ToolTip="Elimina" ImageAlign="AbsMiddle" CommandName="elimina" CommandArgument='<%#Eval("id")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaScadenza" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button runat="server" ID="btnEditScadenza" Style="display: none" OnClick="btnEditScadenza_Click" />
    <asp:Button runat="server" ID="btnInsScadenza" Style="display: none" OnClick="btnInsScadenza_Click" />

    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopup_Click" />

    <asp:HiddenField ID="hf_idScadenza" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />

<%--POPUP INSERIMENTO E MODIFICA--%>
    <asp:UpdatePanel ID="upColl" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto" Style="height: 85%">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE SCADENZARIO
                    </div>
                    <br />

                    <div class="w3-container">
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow" >
                                <asp:Label runat="server" id="lbl_IntestazionePopup" Text="Nuova scadenza" />
                            </div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                            <div class="w3-bar-item w3-button w3-yellow w3-right">
                                <asp:ImageButton ID="btnApriDocumento" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="30px" OnClick="btnApriDocumento_Click"/>
                            </div>
                        </div>
                    </div>

                    <div id="Scadenza" class="w3-container w3-border prot" style="display: block">
                        <div class="w3-container">
                            <p>
                                <div id="div_Fattura" runat="server" class="w3-row-padding">
                                    <div class="w3-col">
                                        <label>Fattura</label>
                                        <asp:DropDownList ID="ddl_fattura" AutoPostBack="true" OnSelectedIndexChanged="ddl_fattura_SelectedIndexChanged" runat="server"/>
                                    </div>
                                    <div class="w3-col">&nbsp;</div>
                                </div>

                                <div class="w3-row-padding">
                                    <div class="w3-quarter">
                                        <label>Tipo (cliente/fornitore)</label>
                                        <asp:DropDownList ID="ddl_Tipo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Tipo_SelectedIndexChanged" Width="100%" CssClass="w3-input w3-border">
                                            <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                                            <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Cliente/Fornitore</label>
                                        <asp:TextBox ID="txt_ClienteFornitore" runat="server" Disabled CssClass="w3-input w3-border" Text=""></asp:TextBox>
                                        
                                    </div>
                                    <div class="w3-quarter" style="position: relative">
                                        <label>Data fattura</label>
                                        <asp:TextBox ID="txt_DataDocumento" runat="server" Disabled MaxLength="10" CssClass="w3-input w3-border calendar" placeholder="GG/MM/AAAA" Text=""></asp:TextBox>
                                    </div>
                                    
                                    <div class="w3-quarter">
                                        <label>Numero fattura</label>
                                        <asp:TextBox ID="txt_NumeroDocumento" Disabled runat="server" MaxLength="20" CssClass="w3-input w3-border" placeholder="" Text=""/>
                                    </div>
                                    
                                </div>
                                <div id="div_CampiInserimento" runat="server" class="w3-row-padding" >
                                    <div class="w3-col" style="width:25%">
                                        <label>Importo con IVA</label>
                                        <asp:TextBox ID="txt_ImportoIva" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" onkeypress="return onlyNumbers();" onkeyup="calcolaScorporoIva();" DataFormatString="{0:N2}"/>
                                    </div>
                                    <div class="w3-col" style="width:12.5%">
                                        <label>IVA</label>
                                        <asp:TextBox ID="txt_Iva" runat="server" MaxLength="2" CssClass="w3-input w3-border" Text="" onkeyup="calcolaScorporoIva()" onkeypress="return onlyNumbers();"/>
                                    </div>
                                    <div class="w3-col" style="width:25%">
                                        <label>Importo netto</label>
                                        <asp:TextBox ID="txt_ImportoDocumento" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Disabled  DataFormatString="{0:N2}"/>
                                    </div>
                                    
                                    <div id="div_NumeroRate" runat="server" class="w3-col" style="width:12.5%">
                                        <label>Numero rate</label>
                                        <asp:TextBox ID="txt_NumeroRate" runat="server" MaxLength="3" CssClass="w3-input w3-border" placeholder="1" Text="" onkeypress="return onlyNumbers();"/>
                                    </div>
                                    <div id="div_AnticipoImporto" runat="server" class="w3-col" style="width:25%">
                                        <label>Anticipo importo</label>
                                        <asp:TextBox ID="txt_AnticipoImporto" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" placeholder="0" Text="" onkeypress="return onlyNumbers();" DataFormatString="{0:N2}"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="div_DatiCreazioneScadenza" runat="server" class="w3-row-padding">
                                    <div id="div_CadenzaGiorni" runat="server" class="w3-quarter">
                                        <label>Cadenza giorni</label>
                                        <asp:TextBox ID="txt_CadenzaGiorni" runat="server" CssClass="w3-input w3-border" Text="" onkeypress="return onlyNumbers();"></asp:TextBox>
                                    </div>
                                    <div id="div_APartireDa" runat="server" class="w3-quarter">
                                        <label>A partire da</label>
                                        <asp:DropDownList ID="ddl_APartireDa" runat="server"  Width="100%" CssClass="w3-input w3-border">
                                            <asp:ListItem Value="0" Text="Data fattura"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Fine mese"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div id="div_Banca" runat="server" class="w3-half">
                                        <label>Banca</label>
                                        <asp:DropDownList ID="ddl_Banca" runat="server"  Width="100%" CssClass="w3-input w3-border"/>
                                    </div>

                                </div>

                                <div id="div_CampiModifica" runat="server" class="w3-row" visible="false">
                                    <div class="w3-half" >
                                        <div class="w3-row">                                           
                                            <div class="w3-half w3-padding" >
                                                <label>Imponibile + IVA</label>
                                                <asp:TextBox ID="txt_ImponibileIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                            <div class="w3-half w3-padding" >
                                                <label>Imponibile</label>
                                                <asp:TextBox ID="txt_Imponibile" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                        </div>
                                        <div class="w3-row">
                                            <div class="w3-third w3-padding" >
                                                <asp:Label ID="lbl_VersatoRiscossoIVA" runat="server" Text="Versato + IVA"></asp:Label>
                                                <asp:TextBox ID="txt_VersatoIva"  runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Style="margin-top:8px" onkeypress="return onlyNumbers();" onkeyup="calcolaScorporoModifica()" Disabled DataFormatString="{0:N2}"/>
                                            </div>
                                            <div class="w3-third w3-padding" >
                                                <label>IVA</label>
                                                <asp:TextBox ID="txt_IvaModifica" runat="server" MaxLength="2" CssClass="w3-input w3-border" onkeypress="return onlyNumbers();" onkeyup="calcolaScorporoModifica()" Disabled DataFormatString="{0:N2}"/>
                                            </div>
                                            <div class="w3-third w3-padding" >
                                                <label><asp:Label ID="lbl_VersatoRiscosso" runat="server" Text="Versato netto" /></label>
                                                <asp:TextBox ID="txt_Versato" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Disabled DataFormatString="{0:N2}"/>
                                            </div>
                                            
                                        </div>
                                        <div class="w3-row">
                                            <div class="w3-half w3-padding" >
                                                <label>Totale + IVA</label>
                                                <asp:TextBox ID="txt_TotaleIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                            <div class="w3-half w3-padding" >
                                                <label>Totale</label>
                                                <asp:TextBox ID="txt_Totale" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="w3-half" >
                                        <div class="w3-row">
                                            <div class="w3-half w3-padding" >
                                                <label>Totale documento</label>
                                                <asp:TextBox ID="txt_TotaleDocumento" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                            <div class="w3-half w3-padding" >
                                                <label>Tot. documento + IVA</label>
                                                <asp:TextBox ID="txt_TotDocumentoIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                            </div>
                                        </div>
                                        <div class="w3-row">
                                            <div class="w3-half w3-padding" >
                                                <label>Data</label>
                                                <asp:TextBox ID="txt_DataDocumentoModifica" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border" />
                                            </div>
                                            <div class="w3-half w3-padding" >
                                                <label>Scadenza</label>
                                                <asp:TextBox ID="txt_ScadenzaDocumento" runat="server" MaxLength="10" CssClass="w3-input w3-border calendar" />
                                            </div>
                                        </div>
                                        <div class="w3-row">
                                            <%--<div class="w3-half w3-padding">
                                                <label>Data versamento/riscossione</label>
                                                <asp:TextBox ID="txt_DataVersamentoRiscossione" runat="server"  MaxLength="10" CssClass="w3-input w3-border calendar" placeholder="GG/MM/AAAA" />
                                            </div>--%>
                                            <div class="w3-row w3-padding">
                                                <label>Banca</label>
                                                <asp:DropDownList ID="ddl_BancaModifica" runat="server"  CssClass="w3-input w3-border"/>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <br />
                                <div style="text-align: center; width:95%; bottom:10px; position:absolute;">
                                    <asp:Button ID="btnInserisciScadenza" runat="server" Text="Inserisci Scadenza" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciScadenza_Click" OnClientClick="return confirm('Confermi inserimento Scadenza?')" />
                                    
                                    <asp:Button ID="btnSaldoTotale" runat="server" Text="Saldo" class="w3-panel w3-green w3-border w3-round" OnClick="btnSaldoTotale_Click" Visible="false" />
                                    <asp:Button ID="btnAcconto" runat="server" Text="Acconto" class="w3-panel w3-blue w3-border w3-round" OnClick="btnAcconto_Click" Visible="false"/>
                                    <asp:Button ID="btnModificaScadenza" runat="server" Text="Modifica Rata" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaScadenza_Click"  Visible="false" />
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>


                </asp:Panel>
            </asp:Panel>

<!-- ACCONTO -->
            <asp:Panel  id="panelAcconto" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                <asp:HiddenField ID="hf_importoScadenzaFigli" runat="server" />
                <div id="divAcconto" class="w3-modal-content w3-card-4 round" style="position: relative; width: 40%; background-color: white;">
                    <div class="w3-row-padding">
                        <div class="w3-panel w3-blue w3-center w3-round">
                            <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Registrazione acconto</b> </h5>
                            <span onclick="document.getElementById('<%= panelAcconto.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                        </div>
                        <div>
                            <asp:Label ID="lbl_MesaggioAcconto" runat="server" ></asp:Label>
                            <br />
                            <asp:Label ID="lbl_ValoriAcconto" runat="server" Text="Indicare l'importo e la data di versamento o riscossione dell'acconto"></asp:Label>
                        </div>
                        <br /><br />
                        <div class="w3-col round" style="padding: 5px; position:relative; height:10%; width: 96%">
                            <div class="w3-col" style="width:30%">
                                <asp:Label ID="lbl_VersatoRiscossoAccontoIVA" runat="server" Text="Versato + IVA"></asp:Label>
                            </div>
                            <div class="w3-rest" >
                                <asp:TextBox ID="txt_VersatoAccontoIva"  runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Style="margin-top:8px" onkeyup="modificaLabelValoriAcconto(); " onkeypress="return onlyNumbers();"  DataFormatString="{0:N2}"/>
                            </div>
                        </div>
                        
                        <div class="w3-col round" style="padding: 5px; position:relative; height:10%; width: 96%; margin-top:20px;">
                            <div class="w3-col" style="width:30%">
                                <asp:Label id="lbl_DataVersamentoRiscossione" runat="server" style="margin-bottom: 0.2rem;padding:8px;">Data versamento</asp:label>
                            </div>
                            <div class="w3-rest" >
                                <asp:TextBox ID="txt_DataVersamentoRiscossione" runat="server" CssClass="w3-input w3-border calendarAggiungiPagamento" placeholder="GG/MM/AAAA"></asp:TextBox>
                            
                            </div>
                        </div>
                    </div>
                    <br /><br />
                    <div class="w3-center" style="margin: 10px; position: relative; bottom:5px;width:96%;">
                        <asp:Button ID="btn_OkAcconto" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btn_OkAcconto_Click" />
                        <button onclick="document.getElementById('<%= panelAcconto.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                    </div>
                </div>
            </asp:Panel>

<!-- SALDO -->
            <asp:Panel  id="panelSaldo" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                <div id="divSaldo" class="w3-modal-content w3-card-4 round" style="position: relative; width: 30%; background-color: white;">
                    <div class="w3-row-padding">
                        <div class="w3-panel w3-blue w3-center w3-round">
                            <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Saldo rata</b> </h5>
                            <span onclick="document.getElementById('<%= panelSaldo.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                        </div>
                        <div>
                            È in corso il saldo della rata corrente, che ammonta a 
                            <b><asp:Label ID="lbl_importoSaldo" runat="server" ></asp:Label> €.</b>
                            <br />
                            <asp:Label ID="lbl_DataSaldo" runat="server" Text="Indicare la data di versamento o riscossione "></asp:Label>
                        </div>
                        <br /><br />
                        
                        <div class="w3-col round" style="padding: 5px; position:relative; height:10%; width: 96%; margin-top:20px;">
                            <div class="w3-col" style="width:20%">
                                Data
                            </div>
                            <div class="w3-rest" >
                                <asp:TextBox ID="txt_DataSaldo" runat="server" CssClass="w3-input w3-border calendarAggiungiPagamento" placeholder="GG/MM/AAAA"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <br /><br />
                    <div class="w3-center" style="margin: 10px; position: relative; bottom:5px;width:96%;">
                        <asp:Button ID="btn_OkSaldo" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btn_OkSaldo_Click" />
                        <button onclick="document.getElementById('<%= panelSaldo.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                    </div>
                </div>
            </asp:Panel>

<!-- MODIFICA SCADENZA CON FIGLI -->
            <div id="panelModificaScadenzaConFigli" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">

                <div id="divModificaScadenzaConFigli" class="w3-modal-content w3-card-4 round" style="position: relative; width: 40%; background-color: white;">
                    <div class="w3-row-padding">
                        <div class="w3-panel w3-blue w3-center w3-round">
                            <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Modifica scadenza</b> </h5>
                            <span onclick="document.getElementById('<%= panelModificaScadenzaConFigli.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                        </div>
                        <div>
                            <asp:Label ID="lblMessaggioPopup" runat="server" /><br />
                        </div>
                        
                    </div>
                    <br /><br />
                    <div class="w3-center" style="margin: 10px; position: relative; bottom:5px;width:96%;">
                        <asp:Button ID="btnOKModificaScadenzaConFigli" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKModificaScadenzaConFigli_Click" />
                        <button onclick="document.getElementById('<%= panelModificaScadenzaConFigli.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

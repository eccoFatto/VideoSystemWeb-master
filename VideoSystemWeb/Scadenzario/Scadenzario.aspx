﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scadenzario.aspx.cs" Inherits="VideoSystemWeb.Scadenzario.userControl.Scadenzario" EnableViewState="true" %>
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
            //$("#< %=ddl_CodiceAnagrafica.ClientID%>").val('');
            $("#<%=hf_RagioneSociale.ClientID%>").val('');
            $("#<%=txt_NumeroFattura.ClientID%>").val('');
            $("#<%=ddlFatturaPagata.ClientID%>").val('');
            $("#<%=txt_DataFatturaDa.ClientID%>").val('');
            $("#<%=txt_DataFatturaA.ClientID%>").val('');
            $("#<%=txt_DataScadenzaDa.ClientID%>").val('');
            $("#<%=txt_DataScadenzaA.ClientID%>").val('');
        }

        function confermaEliminazioneScadenza() {
            return confirm("Verranno eliminate tutte le voci dello scadenzario riguardanti la fattura cui si riferisce la voce selezionata. Confermare?");
        }

        function calcolaImportoIva() {
            var importo = $('#<%=txt_ImportoDocumento.ClientID%>').val().replace(".", "").replace(",", ".");
            var iva = $('#<%=txt_Iva.ClientID%>').val().replace(".", "").replace(",", ".");;
            var importoIva = importo * (1 + (iva / 100));

            $('#<%=txt_ImportoIva.ClientID%>').val(importoIva.toFixed(2).replace(".", ","));
        }

        function calcolaImportoModifica() {
            var importo = $('#<%=txt_Versato.ClientID%>').val().replace(".", "").replace(",",".");
            var iva = $('#<%=txt_IvaModifica.ClientID%>').val().replace(".", "").replace(",", ".");
            var importoIva = importo * (1 + (iva / 100));
            var imponibile = $('#<%=txt_Imponibile.ClientID%>').val().replace(".", "").replace(",", ".");
            var imponibileIva = $('#<%=txt_ImponibileIva.ClientID%>').val().replace(".", "").replace(",", ".");

            $('#<%=txt_VersatoIva.ClientID%>').val(importoIva.toFixed(2).replace(".", ","));

            $('#<%=txt_Totale.ClientID%>').val((imponibile - importo).toFixed(2).replace(".", ","));
            $('#<%=txt_TotaleIva.ClientID%>').val((imponibileIva - importoIva).toFixed(2).replace(".", ","));
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
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
        });

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
                    <asp:DropDownList ID="ddl_TipoAnagrafica" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" Selected></asp:ListItem>
                        <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                        <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Ragione Sociale</label>
                    <%--<asp:DropDownList ID="ddl_CodiceAnagrafica" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border"/>--%>

                    <div id="divRagioneSociale" class="dropdown ">
                        <asp:HiddenField ID="hf_RagioneSociale" runat="server" Value="" />
                        <asp:Button ID="ddl_RagioneSociale" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divClienti" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoRagioneSociale" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px">
                            <input class="form-control" id="filtroRagioneSociale" type="text" placeholder="Cerca..">
                        </ul>
                    </div>




                </div>
                <div class="w3-quarter">
                    <label>Numero Fattura</label>
                    <asp:TextBox ID="txt_NumeroFattura" runat="server" MaxLength="20" class="w3-input w3-border" ></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Pagata</label>
                    <asp:DropDownList ID="ddlFatturaPagata" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" Selected/>
                        <asp:ListItem Value="1" Text="Si" />
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="w3-row-padding" style="position:relative;">
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
                            <asp:Button ID="btnRicercaScadenza" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaScadenza_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciScadenza" runat="server"> 
                            <div id="clbtnInserisciScadenza" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciScadenza();" >Inserisci</div>

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
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last"/>
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/edit.png" ToolTip="Modifica scadenza" ImageAlign="AbsMiddle"  CommandName="modifica" CommandArgument='<%#Eval("id")%>'/>
                                <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" Text="Elimina" ImageUrl="~/Images/delete.png" ToolTip="Elimina scadenza" ImageAlign="AbsMiddle" CommandName="elimina" CommandArgument='<%#Eval("id")%>' OnClientClick="return confermaEliminazioneScadenza();"/>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="30px" CommandName="visualizzaDoc" CommandArgument='<%#Eval("id")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ImportoDare" HeaderText="Totale dare" DataFormatString="{0:N2}" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ImportoVersato" HeaderText="Tot. versato" DataFormatString="{0:N2}" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ProtocolloRiferimento" HeaderText="Documento"  HeaderStyle-Width="11%" />
                        <asp:BoundField DataField="RagioneSocialeClienteFornitore" HeaderText="Nominativo"  HeaderStyle-Width="16%" />
                        <asp:BoundField DataField="ImportoAvere" HeaderText="Totale avere" DataFormatString="{0:N2}" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="ImportoRiscosso" HeaderText="Tot. riscosso" DataFormatString="{0:N2}" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="DataScadenza" HeaderText="Scadenza" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="IsImportoEstinto" HeaderText="Stato" HeaderStyle-Width="4%" />
                        <asp:BoundField DataField="Banca" HeaderText="Banca" HeaderStyle-Width="13%" />
                        <asp:BoundField DataField="DataPagamento" HeaderText="Pagamento" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
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
    <%--<asp:Button runat="server" ID="btnCercaScadenza" Style="display: none" OnClick="btnCercaScadenza_Click" />--%>

    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopup_Click" />

    <asp:HiddenField ID="hf_idScadenza" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />


    <asp:UpdatePanel ID="upColl" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto" Style="height: auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE SCADENZARIO
                    </div>
                    <br />

                    <div class="w3-container">
                        <div class="w3-bar w3-yellow w3-round">
                            <div class="w3-bar-item w3-button w3-yellow" >Nuova scadenza</div>
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
                                        <label>Importo documento</label>
                                        <asp:TextBox ID="txt_ImportoDocumento" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" onkeypress="return onlyNumbers();" onkeyup="calcolaImportoIva();" DataFormatString="{0:N2}"/>
                                    </div>
                                    <div class="w3-col" style="width:12.5%">
                                        <label>IVA</label>
                                        <asp:TextBox ID="txt_Iva" runat="server" MaxLength="2" CssClass="w3-input w3-border" Text="" onkeyup="calcolaImportoIva()" onkeypress="return onlyNumbers();"/>
                                    </div>
                                    <div class="w3-col" style="width:25%">
                                        <label>Importo IVA</label>
                                        <asp:TextBox ID="txt_ImportoIva" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" disabled onkeypress="return onlyNumbers();" DataFormatString="{0:N2}"/>
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

                                <div id="div_CampiModifica" runat="server" class="w3-row-padding" visible="false">
                                    <div class="w3-half" >
                                        
                                        <div class="w3-half w3-padding" >
                                            <label>Imponibile</label>
                                            <asp:TextBox ID="txt_Imponibile" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>
                                        <div class="w3-half w3-padding" >
                                            <label>Imp. + IVA</label>
                                            <asp:TextBox ID="txt_ImponibileIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>

                                        <div class="w3-third w3-padding" >
                                            <asp:Label ID="lbl_VersatoRiscosso" runat="server" Text="Versato"></asp:Label>
                                            <asp:TextBox ID="txt_Versato" runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Style="margin-top:8px" onkeypress="return onlyNumbers();" onkeyup="calcolaImportoModifica()" DataFormatString="{0:N2}"/>
                                        </div>
                                        <div class="w3-third w3-padding" >
                                            <label>IVA</label>
                                            <asp:TextBox ID="txt_IvaModifica" runat="server" MaxLength="2" CssClass="w3-input w3-border" onkeypress="return onlyNumbers();" onkeyup="calcolaImportoModifica()" DataFormatString="{0:N2}"/>
                                        </div>
                                        <div class="w3-third w3-padding" >
                                            <asp:Label ID="lbl_VersatoRiscossoIVA" runat="server" Text="Versato + IVA"></asp:Label>
                                            <asp:TextBox ID="txt_VersatoIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" Style="margin-top:8px" DataFormatString="{0:N2}"/>
                                        </div>

                                        <div class="w3-half w3-padding" >
                                            <label>Totale</label>
                                            <asp:TextBox ID="txt_Totale" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>
                                        <div class="w3-half w3-padding" >
                                            <label>Totale + IVA</label>
                                            <asp:TextBox ID="txt_TotaleIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>
                                    </div>

                                    <div class="w3-half  " >
                                        <div class="w3-half w3-padding" >
                                            <label>Totale documento</label>
                                            <asp:TextBox ID="txt_TotaleDocumento" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>
                                        <div class="w3-half w3-padding" >
                                            <label>Tot. documento + IVA</label>
                                            <asp:TextBox ID="txt_TotDocumentoIva" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border w3-right-align" DataFormatString="{0:N2}"/>
                                        </div>

                                        <div class="w3-half w3-padding" >
                                            <label>Data</label>
                                            <asp:TextBox ID="txt_DataDocumentoModifica" Disabled runat="server" MaxLength="10" CssClass="w3-input w3-border" />
                                        </div>
                                        <div class="w3-half w3-padding" >
                                            <label>Scadenza</label>
                                            <asp:TextBox ID="txt_ScadenzaDocumento" runat="server" MaxLength="10" CssClass="w3-input w3-border calendar" />
                                        </div>

                                        <div class="w3-col w3-padding">
                                            <label>Banca</label>
                                            <asp:DropDownList ID="ddl_BancaModifica" runat="server"  Width="100%" CssClass="w3-input w3-border"/>
                                        </div>
                                    </div>

                                </div>
                                <br />
                                <div style="text-align: center;">
                                    <asp:Button ID="btnInserisciScadenza" runat="server" Text="Inserisci Scadenza" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciScadenza_Click" OnClientClick="return confirm('Confermi inserimento Scadenza?')" />
                                    <asp:Button ID="btnModificaScadenza" runat="server" Text="Modifica Scadenza" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaScadenza_Click" OnClientClick="return confirm('Confermi modifica Scadenza?')" Visible="false" />
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>


                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
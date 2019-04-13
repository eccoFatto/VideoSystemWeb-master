<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>

<%@ Register TagPrefix="popup" TagName="Appuntamento" Src="~/Agenda/userControl/Appuntamento.ascx" %>
<%@ Register TagPrefix="popup" TagName="Offerta" Src="~/Agenda/userControl/Offerta.ascx" %>
<%@ Register TagPrefix="popup" TagName="Lavorazione" Src="~/Agenda/userControl/Lavorazione.ascx" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            $('.calendarAgenda').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY',
                showTodayButton: true
            });

            $('.calendarAgenda').on('dp.change', function (e) {
                $('.loader').show();

                $("#<%=hf_valoreData.ClientID%>").val(e.date.format('DD/MM/YYYY'));
                $("#<%=btnsearch.ClientID%>").click();

            });

            $('.loader').hide();


            registraPassaggioMouse();

            //FUNZIONI DA ESEGUIRE DOPO IL POSTBACK PARZIALE
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                $(".filtroColonna").each(function () {
                    filtraColonna(this, this.value);
                });

                $("#<%=mostraAgenda.ClientID%>").click(function () {
                    $("#<%=pnlContainer.ClientID%>").hide();
                    $(".showAgendaBackground").show();
                });

                $(".showAgendaBackground").click(function () {
                    $(".showAgendaBackground").hide();
                    $("#<%=pnlContainer.ClientID%>").show();
                });

                $('.loader').hide();
            });
        });

        function filtraColonna(element, sottotipo) {
            $('#<%=gv_scheduler.ClientID%> tr').each(function () {
                if ($(element).is(':checked')) {
                    $(this).find('.' + sottotipo).show();
                } else {
                    $(this).find('.' + sottotipo).hide();
                }
            });
        }

        function aggiornaAgenda() {
            $("#<%=btnsearch.ClientID%>").click();
        }

        function registraPassaggioMouse() {
            $("[id*=gv_scheduler] tr").mouseover(function () {
                $(this).addClass("highlight");
            });
            $("[id*=gv_scheduler] tr").mouseout(function () {
                $(this).removeClass("highlight");
            });

            var rows = $("[id*=gv_scheduler] tr");

            rows.children().hover(function () {
                rows.children().removeClass('highlight');
                var index = $(this).prevAll().length;
                rows.find(':nth-child(' + (index + 1) + ')').addClass('highlight');
            });

            rows.children().mouseout(function () {
                rows.children().removeClass('highlight');
            });
        }

        function mostracella(row, column) {
            $('.loader').show();
            $("#<%=hf_data.ClientID%>").val(row);
            $("#<%=hf_risorsa.ClientID%>").val(column);
            $("#<%=btnEditEvent.ClientID%>").click();
        }

        function chiudiPopup() {
            $("#<%=btn_chiudi.ClientID%>").click();
        }

        function openTabEvento(evt, tipoName) {

            $("#<%=hf_tabSelezionata.ClientID%>").val(tipoName);

            var i, x, tablinks;
            x = document.getElementsByClassName("tabEvento");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablink");
            for (i = 0; i < x.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
            }
            document.getElementById(tipoName).style.display = "block";
            evt.currentTarget.className += " w3-red";

            var nomeElemento = '';

            var statoCorrente = $("#<%=val_Stato.ClientID%>").text();

            if (tipoName == 'Appuntamento') {
                nomeElemento = '<%=tab_Appuntamento.ClientID%>';
                if (statoCorrente != 'Previsione impegno' && statoCorrente != 'Riposo') {
                    $("#<%=btnElimina.ClientID%>").addClass("w3-disabled");
                }
                else {
                    $("#<%=btnElimina.ClientID%>").removeClass("w3-disabled");
                }
            } else if (tipoName == 'Offerta') {
                nomeElemento = '<%=tab_Offerta.ClientID%>';
                $("#<%=btnElimina.ClientID%>").removeClass("w3-disabled");
            } else if (tipoName == 'Lavorazione') {
                nomeElemento = '<%=tab_Lavorazione.ClientID%>';
            }
            if (document.getElementById(nomeElemento).className.indexOf("w3-red") == -1)
                document.getElementById(nomeElemento).className += " w3-red";
        }



        function confermaChiusura() {
            return confirm("Le modifiche non salvate verranno perse. Confermare la chiusura?");
        }
    </script>

    <link rel='stylesheet' href='/Css/Agenda.css' />

    <asp:HiddenField ID="hf_valoreData" runat="server" />
    <asp:HiddenField ID="hf_data" runat="server" />
    <asp:HiddenField ID="hf_risorsa" runat="server" />
    <asp:HiddenField ID="hf_tabSelezionata" runat="server" EnableViewState="true" Value="Appuntamento" />

    <table style="width: 99%; height: 100%;">
        <tr>
            <td style="width: 80%; vertical-align: top;">
                <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Style="display: none" />
                <asp:UpdatePanel ID="UpdatePanelCal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="round">
                            <asp:GridView ID="gv_scheduler" runat="server" OnRowDataBound="gv_scheduler_RowDataBound" Style="font-size: 9pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid"></asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 20%; vertical-align: top;">
                <div class="calendarAgenda" style="margin-left: 20px;"></div>
                <br />
                <div class="w3-container" style="margin-left: 20px; text-align: center">
                    <b>LEGENDA</b>
                    <div style="width: 100%; text-align: left" runat="server" id="divLegenda">
                    </div>
                </div>
                <br />
                <div class="w3-container" style="margin-left: 20px; text-align: center">
                    <b>FILTRO COLONNE AGENDA</b>
                    <div style="width: 100%; text-align: left" runat="server" id="divFiltroAgenda">
                    </div>
                </div>
            </td>
        </tr>
    </table>


    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" OnClick="btnEditEvent_Click" />

    <asp:UpdatePanel ID="upEvento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <div runat="server" id="pnlContainer" style="display: none">

                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopup round" Style="font-size: 13px;">

                    <div class="w3-container" style="width: 100%; height: 95%; position: absolute">
                        <div class="w3-bar w3-blue w3-round">
                            <div class="w3-row">
                                <div class="w3-col" style="width: 60%">
                                    <div class="w3-bar-item w3-button tablink w3-red" runat="server" id="tab_Appuntamento" onclick="openTabEvento(event, 'Appuntamento')">Appuntamento</div>
                                    <div class="w3-bar-item w3-button tablink" runat="server" id="tab_Offerta">Offerta</div>
                                    <div class="w3-bar-item w3-button tablink" runat="server" id="tab_Lavorazione">Lavorazione</div>
                                </div>
                                <div class="w3-rest">
                                    <div class="w3-row">
                                        <div class="w3-col" style="width: 90%; font-size: smaller;">
                                            <asp:Label ID="lbl_Stato" runat="server" Text="Stato attuale: "></asp:Label>
                                            <asp:Label ID="val_Stato" runat="server" Text="previsione impegno"></asp:Label>
                                            <br />
                                            <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro: "></asp:Label>
                                            <asp:Label ID="val_CodiceLavoro" runat="server"></asp:Label>
                                        </div>
                                        <div class="w3-rest">
                                            <asp:Image ID="mostraAgenda" runat="server" ImageUrl="~/Images/agenda.png" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="Appuntamento" class="w3-container w3-border tabEvento w3-padding-small" style="height: 90%; overflow: auto;">
                            <popup:Appuntamento ID="popupAppuntamento" runat="server"></popup:Appuntamento>
                        </div>

                        <div id="Offerta" class="w3-container w3-border tabEvento w3-padding-small" style="height: 90%; overflow: auto; display: none">
                            <popup:Offerta ID="popupOfferta" runat="server"></popup:Offerta>
                        </div>

                        <div id="Lavorazione" class="w3-container w3-border tabEvento w3-padding-small" style="height: 90%; overflow: auto; display: none">
                            <popup:Lavorazione ID="popupLavorazione" runat="server"></popup:Lavorazione>
                        </div>
                    </div>

                    <div style="position: absolute; width: 100%; bottom: 5px; text-align: center; height: 7%">
                        <asp:Button ID="btnRiepilogo" runat="server" Text="Visualizza riepilogo" class=" w3-btn w3-white w3-border w3-border-blue w3-round-large" OnClick="btnRiepilogo_Click" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                        <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btn_chiudi_Click" OnClientClick="return confermaChiusura(); $('.loader').show();" Style="padding: 7px 10px" />

                        <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnElimina_Click" OnClientClick="return confermaEliminazione();" Style="padding: 7px 10px" />
                        <asp:Button ID="btnOfferta" runat="server" Text="Trasforma in offerta" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnOfferta_Click" OnClientClick="return confermaCambioStato();$('.loader').show();" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnLavorazione" runat="server" Text="Trasforma in lavorazione" class="w3-btn w3-white w3-border w3-border-purple w3-round-large" OnClientClick="return confermaCambioStato();$('.loader').show();" OnClick="btnLavorazione_Click" Visible="false" Style="padding: 7px 10px" />
                    </div>

                </asp:Panel>
            </div>

            <div id="modalRiepilogoOfferta" class="w3-modal">
                <div class="w3-modal-content  w3-animate-zoom " style="position: fixed; top: 5%; width: 70%; left: 20%; overflow: auto; height: 90%; background-color:transparent ">

                    <div class="w3-center" style="background-color:white">
                        <br/>
                        <span onclick="document.getElementById('modalRiepilogoOfferta').style.display='none'" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi" style="top:22px;">&times;</span>
                    </div>
                    <div id="modalRiepilogoContent" runat="server" style="background-color:white;">
                        <div class="w3-row  w3-padding-large w3-small">
                            <div class="w3-col">
                                <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/logoVSP_trim.png" Style="height: 120px" />
                            </div>

                            <div id="intestazioneSchermo" runat="server">
                                <div class="w3-half ">
                                    <div class="w3-col">
                                        <div class="w3-section ">
                                            <div class="w3-row">
                                                <div class="w3-third">
                                                    <label><b>Roma</b></label>
                                                </div>
                                                <div class="w3-twothird">
                                                    <asp:Label ID="lbl_Data" runat="server"></asp:Label>
                                                </div>
                                                <br />
                                                <br />
                                            </div>
                                            <div class="w3-row">
                                                <div class="w3-third">
                                                    <label><b>Produzione</b></label>
                                                </div>
                                                <div class="w3-twothird">
                                                    <asp:Label ID="lbl_Produzione" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="w3-row">
                                                <div class="w3-third">
                                                    <label><b>Lavorazione</b></label>
                                                </div>
                                                <div class="w3-twothird">
                                                    <asp:Label ID="lbl_Lavorazione" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="w3-row">
                                                <div class="w3-third">
                                                    <label><b>Data Lav.ne</b></label>
                                                </div>
                                                <div class="w3-twothird">
                                                    <asp:Label ID="lbl_DataLavorazione" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="w3-half">
                                    <div class="w3-section ">

                                        <div class="w3-row">
                                            <div class="w3-third">
                                                <label><b>Spettabile</b></label>
                                            </div>
                                            <div class="w3-twothird">
                                                <asp:Label ID="lbl_Cliente" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="w3-row">
                                            <div class="w3-third">
                                                <label><b>Indirizzo</b></label>
                                            </div>
                                            <div class="w3-twothird">
                                                <asp:Label ID="lbl_IndirizzoCliente" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="w3-row">
                                            <div class="w3-third">
                                                <label><b>P. Iva / C.F.</b></label>
                                            </div>
                                            <div class="w3-twothird">
                                                <asp:Label ID="lbl_PIvaCliente" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="protocolloSchermo" runat="server">
                            <div class="w3-card-4 w3-margin-left w3-margin-right">
                                <div class="w3-row w3-padding w3-small">
                                    <div class="w3-half">
                                        <div class="w3-section ">
                                            <div class="w3-third">
                                                <label><b>Offerta numero</b></label>
                                            </div>
                                            <div class="w3-twothird">
                                                <asp:Label ID="lbl_CodLavorazione" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="w3-half">
                                        <div class="w3-section ">
                                            <div class="w3-third">
                                                <label><b>Rif. prot.</b></label>
                                            </div>
                                            <div class="w3-twothird">
                                                <asp:Label ID="lbl_Protocollo" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--STAMPA--%>
                        <div id="intestazioneStampa" runat="server" visible="false" style="font-size: 8pt">
                            <br />
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Roma</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_DataStampa" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Produzione</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_ProduzioneStampa" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Lavorazione</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_LavorazioneStampa" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Data Lav.ne</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_DataLavorazioneStampa" runat="server"></asp:Label></td>
                                            </tr>

                                        </table>
                                    </td>

                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Spettabile</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_ClienteStampa" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Indirizzo</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_IndirizzoClienteStampa" runat="server"></asp:Label></td>
                                            </tr>

                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>P. Iva / C.F.</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_PIvaClienteStampa" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Offerta numero</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_CodLavorazioneStampa" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td style="width: 50%">
                                        <table>
                                            <tr>
                                                <td style="width: 33%">
                                                    <label><b>Rif. prot.</b></label></td>
                                                <td style="width: 66%">
                                                    <asp:Label ID="lbl_ProtocolloStampa" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <%--FINE STAMPA--%>

                        <div class="w3-row w3-section w3-padding w3-small">

                            <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False"
                                Style="font-size: 8pt;max-height:200px;  width: 100%; position: relative; background-color: #FFF; text-align: center"
                                HeaderStyle-BackColor="#2196F3" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" BorderWidth="0"
                                GridLines="None" OnRowDataBound="gvArticoli_RowDataBound" HeaderStyle-HorizontalAlign="Right">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                        <HeaderTemplate>
                                            <div style="text-align: left;">Codice</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCodice" Text='<%# Eval("Descrizione") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descrizione" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                        <HeaderTemplate>
                                            <div style="text-align: left;">Descrizione</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDescrizione" Text='<%# Eval("DescrizioneLunga") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Quantita" HeaderText="Q.tà" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" />
                                    <asp:BoundField DataField="Prezzo" HeaderText="Listino" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" />
                                    <asp:BoundField DataField="Costo" HeaderText="Costo" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" />
                                    <asp:BoundField DataField="Iva" HeaderText="Iva" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top" />
                                    <asp:TemplateField HeaderText="Totale" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <asp:Label ID="totaleRiga" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div id="totaliSchermo" runat="server">
                            <div class="w3-row  w3-small">
                                <div class="w3-col">
                                    <div class="w3-twothird">&nbsp;</div>
                                    <div class="w3-third">
                                        <div class="w3-half" style="padding-left: 50px;">
                                            <label><b>Totale</b></label>
                                        </div>
                                        <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                            <b>
                                                <asp:Label ID="totale" runat="server" /></b>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="w3-row w3-small">
                                <div class="w3-col">
                                    <div class="w3-twothird">&nbsp;</div>
                                    <div class="w3-third">
                                        <div class="w3-half" style="padding-left: 50px;">
                                            <label><b>Totale i.v.a.</b></label>
                                        </div>
                                        <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                            <b>
                                                <asp:Label ID="totaleIVA" runat="server" /></b>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="w3-row  w3-small">
                                <div class="w3-col">
                                    <div class="w3-twothird">&nbsp;</div>
                                    <div class="w3-third">
                                        <div class="w3-half" style="padding-left: 50px;">
                                            <label><b>Totale Euro</b></label>
                                        </div>
                                        <div class="w3-half" style="text-align: right; padding-right: 20px;">
                                            <b>
                                                <asp:Label ID="totaleEuro" runat="server" /></b>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div id="totaliStampa" style="width: 100%; font-size: 8pt" runat="server" visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 70%">&nbsp;</td>
                                    <td style="width: 30%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 60%;" >
                                                    <label><b>Totale</b></label></td>
                                                <td style="width: 40%; text-align: right; padding-right: 20px;">
                                                    <b><asp:Label ID="totaleStampa" runat="server" /></b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 60%;">
                                                    <label><b>Totale i.v.a.</b></label></td>
                                                <td style="width: 40%; text-align: right; padding-right: 20px;">
                                                    <b><asp:Label ID="totaleIVAStampa" runat="server" /><b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 60%;">
                                                    <label><b>Totale Euro</b></label></td>
                                                <td style="width: 40%; text-align: right; padding-right: 20px;">
                                                    <b><asp:Label ID="totaleEuroStampa" runat="server" /></b>
                                                </td>
                                            </tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                            <tr><td colspan="2">&nbsp;</td></tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <%--<div id="datiClienteSchermo" runat="server" style="margin:10px; margin-top:25px;position:relative; width:98%">
                            <div class="w3-row w3-small">
                                <div class="w3-twothird">
                                    <div class="w3-quarter w3-center" style="background-color: #2196F3; color: white; border: solid 1px #fff">
                                        <label>Banca</label>
                                    </div>
                                    <div class="w3-threequarter w3-left" style="background-color: #DDD; border: solid 1px #fff; padding-left: 5px">
                                        Unicredit Banca: IBAN: IT39H0200805198000103515620
                                    </div>
                                </div>
                                <div class="w3-third w3-left" style="background-color: #2196F3; color: white; border: solid 1px #fff; padding-left: 5px">
                                    <b>
                                        <label>Timbro e firma per accettazione</label></b>
                                </div>
                            </div>

                            <div class="w3-row  w3-small">
                                <div class="w3-twothird">
                                    <div class="w3-row">
                                        <div class="w3-quarter w3-center" style="background-color: #2196F3; color: white; border: solid 1px #fff">
                                            <label>Pagamento</label>
                                        </div>
                                        <div class="w3-threequarter w3-left" style="background-color: #DDD; border: solid 1px #fff; padding-left: 5px">
                                            30 gg DFFM
                                        </div>
                                    </div>
                                    <div class="w3-row">
                                        <div class="w3-quarter w3-center" style="background-color: #2196F3; color: white; border: solid 1px #fff">
                                            <label>Consegna</label>
                                        </div>
                                        <div class="w3-threequarter w3-left" style="background-color: #DDD; border: solid 1px #fff; padding-left: 5px">
                                            Via Aurelia, 796 00165 Roma
                                        </div>
                                    </div>
                                </div>
                                <div class="w3-third w3-left" style="background-color: #CCC;border: solid 1px #fff;">
                                    &nbsp;<br />&nbsp;
                                </div>
                            </div>
                        </div>--%>

                        <div id="footerSchermo" style="margin-left:10px; margin-right:10px;margin-top:25px;position:relative; width:98%; font-size:8pt" runat="server" >
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Banca</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">Unicredit Banca: IBAN: IT39H0200805198000103515620</td>
                                    <td style="width:34%;background-color:#2196F3;color: white; border: solid 1px #fff"><b>Timbro e firma per accettazione</b></td>
                                </tr>
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Pagamento</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">
                                        <asp:label id="val_pagamentoSchermo" runat="server" />
                                    </td>
                                    <td style="width:34%;background-color:#DDD;color: white; border: solid 1px #fff" rowspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Consegna</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">
                                        <asp:label id="val_consegnaSchermo" runat="server" />
                                    </td>
                                </tr>
                            </table>

                            <div style="padding:10px; position:relative;font-size:8pt; text-align:center;">
                                <b>Videosystem Production srl&nbsp;&nbsp;P.IVA 13121341005<br />
                                Sede legale:  Via T. Val di Pesa 34 - 00148 Roma&nbsp;&nbsp;e-mail: info@vsproduction.it</b>
                            </div>

                        </div>

                        <div id="footerStampa" style="margin:10px; margin-top:25px;position:absolute; bottom: -30px; width:98%; font-size:8pt" runat="server" Visible="false">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Banca</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">Unicredit Banca: IBAN: IT39H0200805198000103515620</td>
                                    <td style="width:34%;background-color:#2196F3;color: white; border: solid 1px #fff"><b>Timbro e firma per accettazione</b></td>
                                </tr>
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Pagamento</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">
                                        <asp:label id="val_pagamentoStampa" runat="server" />
                                    </td>
                                    <td style="width:34%;background-color:#DDD;color: white; border: solid 1px #fff" rowspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width:16%;background-color:#2196F3;color: white; border: solid 1px #fff; text-align:center">Consegna</td>
                                    <td style="width:50%;background-color:#DDD;border: solid 1px #fff">
                                        <asp:label id="val_consegnaStampa" runat="server" />
                                    </td>
                                </tr>
                            </table>

                            <table style="padding:10px; position:relative;">
                                <tr>
                                    <td style="width:90%;text-align:center;font-size:8pt;">
                                        <b>Videosystem Production srl&nbsp;&nbsp;P.IVA 13121341005<br />
                                        Sede legale:  Via T. Val di Pesa 34 - 00148 Roma&nbsp;&nbsp;e-mail: info@vsproduction.it</b>

                                    </td>
                                    <td style="width:10%">
                                        <asp:Image ID="imgDNV" runat="server" ImageUrl="~/Images/DNV_2008_ITA2.jpg" Style="height: 80px" />
                                    </td>

                                </tr>

                            </table>
                        </div>
                    </div>

                    <div class="w3-center w3-padding-small" style="position: relative;background-color:white ">
                        <asp:Button ID="btnStampaOfferta" runat="server" Text="Stampa" class="w3-btn w3-white w3-border w3-border-orange w3-round-large " Style="font-size: smaller; padding: 4px 8px" OnClick="btnStampa_Click" />
                        <button onclick="document.getElementById('modalRiepilogoOfferta').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Chiudi</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditEvent" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>
    <div class="showAgendaBackground" style="display: none"></div>
</asp:Content>

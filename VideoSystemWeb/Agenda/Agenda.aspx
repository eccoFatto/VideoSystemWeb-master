<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>

<%@ Register TagPrefix="popup" TagName="Appuntamento" Src="~/Agenda/userControl/Appuntamento.ascx" %>
<%@ Register TagPrefix="popup" TagName="Offerta" Src="~/Agenda/userControl/Offerta.ascx" %>
<%@ Register TagPrefix="popup" TagName="Lavorazione" Src="~/Agenda/userControl/Lavorazione.ascx" %>
<%@ Register TagPrefix="popup" TagName="RiepilogoOfferta" Src="~/Agenda/userControl/RiepilogoOfferta.ascx" %>
<%@ Register TagPrefix="popup" TagName="RiepilogoPianoEsterno" Src="~/Agenda/userControl/RiepilogoPianoEsterno.ascx" %>
<%@ Register TagPrefix="popup" TagName="RiepilogoConsuntivo" Src="~/Agenda/userControl/RiepilogoConsuntivo.ascx" %>
<%@ Register TagPrefix="popup" TagName="RiepilogoFattura" Src="~/Agenda/userControl/RiepilogoFattura.ascx" %>
<%@ Register TagPrefix="popup" TagName="RiepilogoGiornata" Src="~/Agenda/userControl/RiepilogoGiornata.ascx" %>
<%@ Register TagPrefix="popup" TagName="ReportCollaboratori" Src="~/REPORT/userControl/collaboratoriPerGiornata.ascx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            (function blink() { 
              $('.blink').fadeOut(800).fadeIn(500, blink); 
            })();

            $('.calendarAgenda').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY',
                showTodayButton: true,
                date: new Date(<%=GetDataSelezionata()%>)
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

        function openTabEvento(tipoName) {
            var velocita = 0;
            if ($("#<%=hf_tabSelezionata.ClientID%>").val() != tipoName) {
                velocita = 300;
            }


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
            //evt.currentTarget.className += " w3-red";

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

                $('.infoEvento').fadeOut(0);
                $('#infoGenerali').removeClass("w3-quarter");
                $('#infoGenerali').addClass("w3-col");

                $("#<%=innerContainer.ClientID%>").animate({
                    width: "47%"
                }, velocita);
                $('#div_tabStato').css('width', '60%');

                $('#<%=btnRiepilogo.ClientID%>').hide();
                $('#<%=btnStampaPianoEsterno.ClientID%>').hide();
                $('#<%=btnStampaConsuntivo.ClientID%>').hide();
                $('#<%=btnStampaFattura.ClientID%>').hide();

            } else if (tipoName == 'Offerta') {
                nomeElemento = '<%=tab_Offerta.ClientID%>';

                if (statoCorrente == 'Lavorazione') {
                    $("#<%=btnElimina.ClientID%>").addClass("w3-disabled");
                }
                else {
                    $("#<%=btnElimina.ClientID%>").removeClass("w3-disabled");
                }

                $('.infoEvento').fadeOut(0);
                $('#infoGenerali').removeClass("w3-quarter");
                $('#infoGenerali').addClass("w3-col");
                $("#<%=innerContainer.ClientID%>").animate({
                    width: "47%"
                }, velocita);
                $('#div_tabStato').css('width', '60%');

                $('#<%=btnRiepilogo.ClientID%>').show();
                $('#<%=btnStampaPianoEsterno.ClientID%>').hide();
                $('#<%=btnStampaConsuntivo.ClientID%>').hide();
                $('#<%=btnStampaFattura.ClientID%>').hide();

            } else if (tipoName == 'Lavorazione') {
                nomeElemento = '<%=tab_Lavorazione.ClientID%>';
                $("#<%=btnElimina.ClientID%>").removeClass("w3-disabled");

                $("#<%=innerContainer.ClientID%>").animate({
                    width: "98%"
                }, velocita);
                $('#div_tabStato').css('width', '30%');
                $('.infoEvento').fadeIn(1000);
                $('#infoGenerali').removeClass("w3-col");
                $('#infoGenerali').addClass("w3-quarter");

                $('#<%=btnRiepilogo.ClientID%>').hide();
                $('#<%=btnStampaPianoEsterno.ClientID%>').show();
                $('#<%=btnStampaConsuntivo.ClientID%>').show();
                $('#<%=btnStampaFattura.ClientID%>').show();
            }
            if (document.getElementById(nomeElemento).className.indexOf("w3-red") == -1)
                document.getElementById(nomeElemento).className += " w3-red";
        }

        <%--function confermaChiusura() {
            var salvataggio = confirm("Salvare le modifiche?\n\nOK: Salva e chiudi\nAnnulla: Chiudi senza salvare");
            
            if (salvataggio) {
                $('#<%=hf_Salvataggio.ClientID%>').val("1");
            }
            else {
                $('#<%=hf_Salvataggio.ClientID%>').val("0");
            }
            return true;
        }--%>

        function confermaEliminazione() {
            var statoCorrente = $("#<%=val_Stato.ClientID%>").text();
            if (statoCorrente == 'Lavorazione') {
                return confirm("La lavorazione corrente sta per essere eliminata e l'evento tornerà allo stato Offerta. Confermare?");
            } else {
                return confirm("Eliminare l'evento corrente?");
            }
        }

        function confermaCambioStato() {
            return confirm("Lo stato dell'evento sta per essere modificato.\n Le modifiche andranno perse se non verrà effettuato il salvataggio");
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
                <br />
                <div class="w3-container" style="margin-left: 20px; margin-top:15px; text-align: center">
                    <a href="/Protocollo/Protocollo.aspx" class="w3-bar-item w3-button w3-green w3-border w3-round" style="width: 80%;" onclick="$('.loader').show();">Protocolli</a>
                </div>
                <div class="w3-container" style="margin-left: 20px; margin-top:15px; text-align: center">
                    <a href="/Scadenzario/Scadenzario.aspx" class="w3-bar-item w3-button w3-green w3-border w3-round" style="width: 80%;" onclick="$('.loader').show();">Scadenzario</a>
                </div>
                <div class="w3-container" style="margin-left: 20px; margin-top:15px; text-align: center">
                    <a href="/MAGAZZINO/Attrezzature.aspx" class="w3-bar-item w3-button w3-green w3-border w3-round" style="width: 80%;" onclick="$('.loader').show();">Magazzino</a>
                </div>
                <div class="w3-container" style="margin-left: 20px; margin-top:15px; text-align: center">
                    <asp:Button runat="server" CssClass="w3-bar-item w3-button w3-green w3-border w3-round" ID="btnStampaGiornata" Text="Collab. per Giornata" OnClick="btnStampaGiornata_Click" style="width: 80%;" OnClientClick="$('.loader').show();" Visible="true" />
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

                    <div style="width: 100%; height: 95%; position: absolute">
                        <div class="w3-bar w3-blue w3-round">
                            <div class="w3-row">
                                <div id="div_tabStato" class="w3-col" style="width: 60%">
                                    <div class="w3-bar-item w3-button tablink w3-red" runat="server" id="tab_Appuntamento" onclick="openTabEvento('Appuntamento')">Appuntamento</div>
                                    <div class="w3-bar-item w3-button tablink" runat="server" id="tab_Offerta">Offerta</div>
                                    <div class="w3-bar-item w3-button tablink" runat="server" id="tab_Lavorazione">Lavorazione</div>
                                </div>
                                <div class="w3-rest">
                                    <div class="w3-row">
                                        <div class="w3-col" style="width: 90%; font-size: smaller;">
                                            <div class="w3-quarter infoEvento" >
                                                <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente: "></asp:Label>
                                                <asp:Label ID="val_Cliente" runat="server" ></asp:Label>
                                                <br />
                                                <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione: "></asp:Label>
                                                <asp:Label ID="val_Produzione" runat="server" ></asp:Label>
                                            </div>
                                            <div class="w3-quarter infoEvento" >
                                                <asp:Label ID="lbl_Lavorazione" runat="server" Text="Lavorazione: "></asp:Label>
                                                <asp:Label ID="val_Lavorazione" runat="server" ></asp:Label>
                                                <br />
                                                <asp:Label ID="lbl_Tipologia" runat="server" Text="Tipologia: "></asp:Label>
                                                <asp:Label ID="val_Tipologia" runat="server" ></asp:Label>
                                            </div>
                                            <div class="w3-quarter infoEvento" >
                                                <asp:Label ID="lbl_DataInizio" runat="server" Text="Data inizio: "></asp:Label>
                                                <asp:Label ID="val_DataInizio" runat="server" ></asp:Label>
                                                <br />
                                                <asp:Label ID="lbl_DataFine" runat="server" Text="Data fine: "></asp:Label>
                                                <asp:Label ID="val_DataFine" runat="server" ></asp:Label>
                                            </div>
                                            <div id="infoGenerali" class="w3-quarter" >
                                                <asp:Label ID="lbl_Stato" runat="server" Text="Stato attuale: "></asp:Label>
                                                <asp:Label ID="val_Stato" runat="server" Text="previsione impegno"></asp:Label>
                                                <br />
                                                <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro: "></asp:Label>
                                                <asp:Label ID="val_CodiceLavoro" runat="server"></asp:Label>
                                            </div>



                                        </div>
                                        <div class="w3-rest">
                                            <asp:Image ID="mostraAgenda" runat="server" ImageUrl="~/Images/agenda.png" ToolTip="Mostra agenda"/>
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

    <!-- PULSANTI -->
                    <div style="position: absolute; width: 100%; bottom: 0px; text-align: center; height: 7%">
                        <asp:Button ID="btnRiepilogo" runat="server" Text="Stampa Offerta" class=" w3-btn w3-white w3-border w3-border-blue w3-round-large" OnClick="btnRiepilogo_Click" Visible="false" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                        <asp:Button ID="btnStampaPianoEsterno" runat="server" Text="Stampa Piano Esterno" class=" w3-btn w3-white w3-border w3-border-cyan w3-round-large" OnClick="btnStampaPianoEsterno_Click" OnClientClick="$('.loader').show();" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnStampaConsuntivo" runat="server" Text="Stampa Consuntivo" class=" w3-btn w3-white w3-border w3-border-cyan w3-round-large" OnClick="btnStampaConsuntivo_Click" OnClientClick="$('.loader').show();" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnStampaFattura" runat="server" Text="Stampa Fattura" class=" w3-btn w3-white w3-border w3-border-cyan w3-round-large" OnClick="btnStampaFattura_Click" OnClientClick="$('.loader').show();" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnMagazzino" runat="server" Text="Magazzino" class=" w3-btn w3-white w3-border w3-border-cyan w3-round-large" OnClick="btnMagazzino_Click" OnClientClick="$('.loader').show();" Visible="false" Style="padding: 7px 10px" />

                        <asp:Button ID="btnElencoLavoratoriPerGiornata" runat="server" Text="Collab. impegnati" class=" w3-btn w3-white w3-border w3-border-cyan w3-round-large" OnClick="btnElencoLavoratoriPerGiornata_Click" OnClientClick="$('.loader').show();" Visible="true" Style="padding: 7px 10px" />

                        <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                        <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-btn w3-white w3-border w3-border-yellow w3-round-large" OnClick="btn_chiudi_Click"  Style="padding: 7px 10px" />

                        <%--<asp:HiddenField ID="hf_Salvataggio" runat="server" />--%>

                        <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnElimina_Click" OnClientClick="return confermaEliminazione();$('.loader').show();" Style="padding: 7px 10px" />
                        <asp:Button ID="btnOfferta" runat="server" Text="Trasforma in offerta" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnOfferta_Click" OnClientClick="return confermaCambioStato();$('.loader').show();" Visible="false" Style="padding: 7px 10px" />
                        <asp:Button ID="btnLavorazione" runat="server" Text="Trasforma in lavorazione" class="w3-btn w3-white w3-border w3-border-purple w3-round-large" OnClientClick="return confermaCambioStato();$('.loader').show();" OnClick="btnLavorazione_Click" Visible="false" Style="padding: 7px 10px" />
                    
                        <asp:Button ID="btn_annulla" runat="server" Text="Annulla" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btn_annulla_Click"  Style="padding: 7px 10px;" />
                    </div>

                </asp:Panel>
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditEvent" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upRiepilogoOfferta" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <popup:RiepilogoOfferta ID="popupRiepilogoOfferta" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upRiepilogoPianoEsterno" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <popup:RiepilogoPianoEsterno ID="popupRiepilogoPianoEsterno" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upRiepilogoConsuntivo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <popup:RiepilogoConsuntivo ID="popupRiepilogoConsuntivo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upRiepilogoFattura" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <popup:RiepilogoFattura ID="popupRiepilogoFattura" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upRiepilogoGiornata" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <popup:RiepilogoGiornata ID="popupRiepilogoGiornata" runat="server" />
            <%--<popup:ReportCollaboratori ID="RiepilogoGiornata1" runat="server" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="showAgendaBackground" style="display: none">
        <div class="blink" style="position: fixed; width: 100%; bottom: 5%; color:darkred;">
            <asp:Label ID="lbl_backgroundAgenda" Text="Un evento è in fase di modifica" style="font-size:50pt; " runat="server" /><br />
            <asp:Label ID="lbl_sottotitoloBGAgenda" Text="Fare clic per tornare alla visualizzazione evento" style="font-size:20pt;"  runat="server" />
        </div>
    </div>
</asp:Content>

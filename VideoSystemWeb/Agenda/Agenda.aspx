<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            $('.calendarPresentazione').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            $('.calendarPresentazione').on('dp.change', function (e) {
                var data = e.date.date() + "/" + (e.date.month()+1) + "/" + e.date.year();
                $("#<%=hf_valoreData.ClientID%>").val(data);
                 $("#<%=btnsearch.ClientID%>").click();
            });

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function ()  
            {  
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
                });

                $('.time').datetimepicker({
                    locale: 'it',
                    format: 'LT'
                });
            });

            registraPassaggioMouse();
        });

        function controlloCoerenzaDate(id_calendarDataInizio, id_calendarDataFine) {
            $('#'+id_calendarDataInizio).datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });
            $('#' + id_calendarDataFine).datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY',
                useCurrent: false //Important! See issue #1075
            });

            if ($('#' + id_calendarDataInizio).val() != "") {
                var dataInizio = $('#' + id_calendarDataInizio).data('DateTimePicker').date();
                $('#'+id_calendarDataFine).data("DateTimePicker").minDate(new Date(dataInizio));
            }

            if ($('#' + id_calendarDataFine).val() != "") {
                var dataFine = $('#' + id_calendarDataFine).data('DateTimePicker').date();
                $('#'+id_calendarDataInizio).data("DateTimePicker").maxDate(new Date(dataFine));
            }

            $('#'+id_calendarDataInizio).on("dp.change", function (e) {
                $('#'+id_calendarDataFine).data("DateTimePicker").minDate(e.date);
            });
            $('#'+id_calendarDataFine).on("dp.change", function (e) {
                $('#'+id_calendarDataInizio).data("DateTimePicker").maxDate(e.date);
            });
        }

        function checkImpegnoOrario() {
            if ($("#<%=chk_ImpegnoOrario.ClientID%>").prop('checked')) {
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").prop('disabled', false);
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").removeClass("w3-light-grey");
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").addClass("w3-hover-orange");
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").addClass("w3-white");

                $("#<%=txt_ImpegnoOrarioA.ClientID%>").prop('disabled', false);
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").removeClass("w3-light-grey");
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").addClass("w3-hover-orange");
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").addClass("w3-white");
            } else {
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").prop('disabled', true);
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").removeClass("w3-white");
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").removeClass("w3-hover-orange");
                $("#<%=txt_ImpegnoOrarioDa.ClientID%>").addClass("w3-light-grey");

                $("#<%=txt_ImpegnoOrarioA.ClientID%>").prop('disabled', true);
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").removeClass("w3-white");
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").removeClass("w3-hover-orange");
                $("#<%=txt_ImpegnoOrarioA.ClientID%>").addClass("w3-light-grey");
            }
        }

        $("body").on("click", "#<%=chk_ImpegnoOrario.ClientID%>", function () {
            checkImpegnoOrario();
        });

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
            $("#<%=hf_data.ClientID%>").val(row);
            $("#<%=hf_risorsa.ClientID%>").val(column);
            $("#<%=btnEditEvent.ClientID%>").click();       
        }

        function chiudiPopup() {
            $("#<%=btn_chiudi.ClientID%>").click();
        }
    </script>

    <link rel='stylesheet' href='/Css/Agenda.css' />

    <table style="width: 98%">
        <tr>
            <td style="width: 80%; vertical-align: top;">

                <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Style="display: none" />

                <asp:UpdatePanel ID="UpdatePanelCal" runat="server">
                    <ContentTemplate>
                        <div class="round">
                            <asp:GridView ID="gv_scheduler" runat="server" OnRowDataBound="gv_scheduler_RowDataBound" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid"></asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 20%; vertical-align: top;">
                <div class="calendarPresentazione"></div>
                <asp:HiddenField ID="hf_valoreData" runat="server" />
            </td>
        </tr>
    </table>










    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" OnClick="btnEditEvent_Click" />
    <asp:HiddenField ID="hf_data" runat="server" />
    <asp:HiddenField ID="hf_risorsa" runat="server" />
    <asp:UpdatePanel ID="upEvento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <div>
                <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                    <asp:HiddenField ID="hf_idEvento" runat="server" />
                    <div class="modalBackground"></div>
                    <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopup round" ScrollBars="Auto">
                        <div class="intestazionePopup" style="width: 100%; text-align: center">
                            GESTIONE APPUNTAMENTI
                        </div>
                        <br />
                        <div class="errorMessage" style="width: 100%; text-align: center">
                            <asp:Label ID="lbl_MessaggioErrore" runat="server" Visible="false"></asp:Label>
                        </div>
                        <table style="font-size: 10pt;width:95%">
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DataInizioLavorazione" runat="server" Text="Data inizio lavorazione" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DataInizioLavorazione" runat="server" CssClass="w3-light-grey w3-border w3-round fieldMedium" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DataInizioLavorazione" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DataFineLavorazione" runat="server" Text="Data fine lavorazione" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox CssClass=" w3-light-grey w3-border w3-round fieldMedium" ID="val_DataFineLavorazione" runat="server" Enabled="false" />
                                    <asp:TextBox CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" ID="txt_DataFineLavorazione" runat="server" Visible="false" />
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DurataLavorazione" runat="server" Text="Durata lavorazione" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DurataLavorazione" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DurataLavorazione" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" runat="server" MaxLength="2" onkeypress="return onlyNumbers()" Visible="false"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_Risorsa" runat="server" Text="Pianificazione evento" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Risorse" CssClass=" w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddl_Risorse" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:DropDownList>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_Tipologia" runat="server" Text="Tipologia" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Tipologia" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddl_Tipologia" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:DropDownList>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_cliente" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddl_cliente" runat="server" CssClass="l w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DurataViaggioAndata" runat="server" Text="Durata viaggio andata" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DurataViaggioAndata" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DurataViaggioAndata" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DurataViaggioRitorno" runat="server" Text="Durata viaggio ritorno" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DurataViaggioRitorno" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DurataViaggioRitorno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DataInizioImpegno" runat="server" Text="Data inizio impegno" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DataInizioImpegno" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DataInizioImpegno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" placeholder="DD/MM/YYYY" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_DataFineImpegno" runat="server" Text="Data fine impegno" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_DataFineImpegno" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_DataFineImpegno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" placeholder="DD/MM/YYYY" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_ImpegnoOrario" runat="server" Text="Impegno orario" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_ImpegnoOrario" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:CheckBox ID="chk_ImpegnoOrario" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" Style="width: 30px;" Visible="false" ClientIDMode="Static" />
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_ImpegnoOrarioDa" runat="server" Text="Impegno orario da" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_ImpegnoOrarioDa" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_ImpegnoOrarioDa" runat="server" CssClass=" time" Enabled="false" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_ImpegnoOrarioA" runat="server" Text="Impegno orario a" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_ImpegnoOrarioA" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_ImpegnoOrarioA" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium time" Enabled="false" Visible="false" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Produzione" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_Produzione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_lavorazione" runat="server" Text="Lavorazione" CssClass="w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Lavorazione" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_Lavorazione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_indirizzo" runat="server" Text="Via" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Indirizzo" CssClass="w3-light-grey w3-border w3-round fieldLarge" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_Indirizzo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldLarge" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_luogo" runat="server" Text="Luogo" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Luogo" CssClass="lw3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_Luogo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:TextBox>
                                </td>
                                <td class="column" runat="server">
                                    <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro" CssClass=" w3-yellow w3-border w3-round" style="padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_CodiceLavoro" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_CodiceLavoro" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" runat="server">
                                    <asp:Label ID="lbl_nota" runat="server" Text="Nota" CssClass="w3-yellow w3-border w3-round" Style="vertical-align: top; position: relative; top: 1px; left:7px;padding-left:5px; padding-right:5px;"></asp:Label>
                                    <asp:TextBox ID="val_Nota" CssClass="w3-light-grey w3-border w3-round" runat="server" Enabled="false" Style="width: 90%; margin-left: 10px;margin-bottom:10px"></asp:TextBox>
                                    <asp:TextBox ID="tb_Nota" Style="width: 90%;position:relative; left:10px;" Rows="5" TextMode="MultiLine" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: center;">
                            <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-green w3-border w3-round" OnClick="btnModifica_Click" />
                            <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-green w3-border w3-round" OnClick="btnSalva_Click" Visible="false" />
                            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false" />
                        </div>
                        <p style="text-align: center;">
                            <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class= "w3-green w3-border w3-round" OnClick="btn_chiudi_Click" />
                        </p>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditEvent" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Appuntamenti.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Appuntamenti" %>

<script>
    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            $('.time').datetimepicker({
                locale: 'it',
                format: 'LT'
            });
        });
    });

    function controlloCoerenzaDate(id_calendarDataInizio, id_calendarDataFine) {
        $('#' + id_calendarDataInizio).datetimepicker({
            locale: 'it',
            format: 'DD/MM/YYYY'
        });
        $('#' + id_calendarDataFine).datetimepicker({
            locale: 'it',
            format: 'DD/MM/YYYY',
            useCurrent: false //Important! See issue #1075
        });

        if ($('#' + id_calendarDataInizio).val() != "") {
            var dataInizio = $('#' + id_calendarDataInizio).data('DateTimePicker');
            if (dataInizio != null) {
                $('#' + id_calendarDataFine).data("DateTimePicker").minDate(new Date(dataInizio.date()));
            }
        }

        if ($('#' + id_calendarDataFine).val() != "") {
            var dataFine = $('#' + id_calendarDataFine).data('DateTimePicker');
            if (dataFine != null) {
                $('#' + id_calendarDataInizio).data("DateTimePicker").maxDate(new Date(dataFine.date()));
            }
        }

        $('#' + id_calendarDataInizio).on("dp.change", function (e) {
            $('#' + id_calendarDataFine).data("DateTimePicker").minDate(e.date);
        });
        $('#' + id_calendarDataFine).on("dp.change", function (e) {
            $('#' + id_calendarDataInizio).data("DateTimePicker").maxDate(e.date);
        });
    }

    $("body").on("click", "#<%=chk_ImpegnoOrario.ClientID%>", function () {
        checkImpegnoOrario();
    });

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

    
    function confermaEliminazione() {
        return confirm("Eliminare l'appuntamento corrente?"); 
    }
</script>


<asp:Panel runat="server" ID="panelAppuntamenti" >
    <div class="w3-container w3-center w3-xlarge">GESTIONE APPUNTAMENTI</div>

    <div class="alert alert-danger alert-dismissible fade in" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    <!--LAVORAZIONE-->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin w3-padding-16">
            <h4 class="w3-center">Lavorazione</h4>
            <div class="w3-row w3-section w3-padding">
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataInizioLavorazione" runat="server" Text="Data inizio lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox ID="val_DataInizioLavorazione" runat="server" CssClass="w3-light-grey w3-border w3-round fieldMedium" Enabled="false"></asp:TextBox>
                    <asp:TextBox ID="txt_DataInizioLavorazione" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" Visible="false" placeholder="gg/mm/aaaa"></asp:TextBox>
                </div>

                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataFineLavorazione" runat="server" Text="Data fine lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox CssClass=" w3-light-grey w3-border w3-round fieldMedium" ID="val_DataFineLavorazione" runat="server" Enabled="false" />
                    <asp:TextBox CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" ID="txt_DataFineLavorazione" runat="server" Visible="false" />
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataLavorazione" runat="server" Text="Durata lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox ID="val_DurataLavorazione" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                    <asp:TextBox ID="txt_DurataLavorazione" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" runat="server" MaxLength="2" onkeypress="return onlyNumbers()" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <!--pianificazione  -->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin w3-padding-16">
            <h4 class="w3-center">Pianificazione</h4>
            <div class="w3-row w3-section w3-padding">
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Risorsa" runat="server" Text="Pianificazione evento" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:TextBox ID="val_Risorse" CssClass=" w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:DropDownList ID="ddl_Risorse" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:DropDownList>
                    </div>
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Tipologia" runat="server" Text="Tipologia" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:TextBox ID="val_Tipologia" CssClass="w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:DropDownList ID="ddl_Tipologia" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:DropDownList>
                    </div>
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:TextBox ID="val_cliente" CssClass="w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:DropDownList ID="ddl_cliente" runat="server" CssClass="l w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--IMPEGNO-->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin w3-padding-16">
            <h4 class="w3-center">Impegno</h4>
            <div class="w3-row w3-section " style="padding-left: 6px; padding-right: 6px;">
                <div class="w3-rest">
                    <div class="w3-col" style="width: 35%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataViaggioAndata" runat="server" Text="V. andata gg" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_DurataViaggioAndata" CssClass="w3-light-grey w3-border w3-round fieldSmall" Style="width: 30px;" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_DurataViaggioAndata" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" Visible="false"></asp:TextBox>
                    </div>

                    <div class="w3-col" style="width: 65%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataInizioImpegno" runat="server" Text="Inizio impegno" CssClass="w3-yellow w3-border w3-round" Style="margin-left: 10px; padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_DataInizioImpegno" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_DataInizioImpegno" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" placeholder="DD/MM/YYYY" Style="float: right;" Visible="false"></asp:TextBox>
                    </div>

                </div>
                <div class="w3-rest">

                    <div class="w3-col" style="width: 35%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataViaggioRitorno" runat="server" Text="V. ritorno gg" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_DurataViaggioRitorno" CssClass="w3-light-grey w3-border w3-round fieldSmall" Style="width: 30px;" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_DurataViaggioRitorno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" Visible="false"></asp:TextBox>
                    </div>


                    <div class="w3-col" style="width: 65%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataFineImpegno" runat="server" Text="Fine impegno" CssClass=" w3-yellow w3-border w3-round" Style="margin-left: 10px; padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_DataFineImpegno" CssClass="w3-light-grey w3-border w3-round fieldMedium" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_DataFineImpegno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" placeholder="DD/MM/YYYY" Style="float: right;" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-rest">
                    <div class="w3-col" style="width: 45%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_ImpegnoOrario" runat="server" Text="Impegno orario" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:CheckBox ID="val_ImpegnoOrario" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" Style="width: 30px;" Enabled="false" Visible="false" ClientIDMode="Static" />
                        <asp:CheckBox ID="chk_ImpegnoOrario" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" Style="width: 30px;" Visible="false" ClientIDMode="Static" />


                    </div>
                    <div class="w3-col" style="width: 55%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_ImpegnoOrarioDa" runat="server" Text="Dalle" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_ImpegnoOrarioDa" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_ImpegnoOrarioDa" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall time" Enabled="false" Visible="false"></asp:TextBox>

                        <asp:Label ID="lbl_ImpegnoOrarioA" runat="server" Text="Alle" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="val_ImpegnoOrarioA" CssClass="w3-light-grey w3-border w3-round fieldSmall" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_ImpegnoOrarioA" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall time" Enabled="false" Visible="false"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--PRODUZIONE-->
    <div class="w3-col m6">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin w3-padding-16">
            <h4 class="w3-center">Produzione</h4>
            <div class="w3-row w3-section">
                <div class="w3-rest">
                    <div>
                        <div class="w3-col" style="width: 15%; margin-bottom: 5px;">
                            <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>

                        <div class="w3-col" style="width: 35%; margin-bottom: 5px;">
                            <asp:TextBox ID="val_Produzione" CssClass="w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                            <asp:TextBox ID="txt_Produzione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:TextBox>
                        </div>

                        <div class="w3-col" style="width: 15%; margin-bottom: 5px;">
                            <asp:Label ID="lbl_lavorazione" runat="server" Text="Lavorazione" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>

                        <div class="w3-col" style="width: 35%; margin-bottom: 5px;">
                            <asp:TextBox ID="val_Lavorazione" CssClass="w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                            <asp:TextBox ID="txt_Lavorazione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="w3-rest">
                    <div>
                        <div class="w3-col" style="width: 15%; margin-bottom: 5px;">
                            <asp:Label ID="lbl_indirizzo" runat="server" Text="Via" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>
                        <div class="w3-col" style="width: 85%; margin-bottom: 5px;">
                            <asp:TextBox ID="val_Indirizzo" CssClass="w3-light-grey w3-border w3-round" Style="width: 98%" runat="server" Enabled="false"></asp:TextBox>
                            <asp:TextBox ID="txt_Indirizzo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round" Style="width: 98%" Visible="false"></asp:TextBox>
                        </div>

                    </div>
                </div>

                <div class="w3-rest">
                    <div class="w3-col" style="width: 15%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_luogo" runat="server" Text="Luogo" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>

                    <div class="w3-col" style="width: 35%; margin-bottom: 5px;">
                        <asp:TextBox ID="val_Luogo" CssClass="lw3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_Luogo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:TextBox>

                    </div>

                    <div class="w3-col" style="width: 20%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>

                    <div class="w3-col" style="width: 30%; margin-bottom: 5px;">
                        <asp:TextBox ID="val_CodiceLavoro" CssClass="w3-light-grey w3-border w3-round fieldMax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_CodiceLavoro" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" Visible="false"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--NOTA-->
    <div class="w3-col m6">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin w3-padding-16">
            <h4 class="w3-center">Nota</h4>
            <div class="w3-row w3-section">
                <div class="w3-rest">
                    <asp:TextBox ID="val_Nota" CssClass="w3-light-grey w3-border w3-round" runat="server" Rows="4" TextMode="MultiLine" Enabled="false" Style="width: 96%; margin-left: 10px; margin-bottom: 10px"></asp:TextBox>
                    <asp:TextBox ID="tb_Nota" Style="width: 96%; position: relative; left: 10px;" Rows="4" TextMode="MultiLine" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round" Visible="false"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="w3-col m12">
        <<div class="w3-center w3-margin">
            <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-green w3-border w3-round" OnClick="btnModifica_Click" />
            <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-green w3-border w3-round" OnClick="btnElimina_Click" OnClientClick="return confermaEliminazione();"/>
            <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-green w3-border w3-round" OnClick="btnSalva_Click" Visible="false" />
            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false" />
        </div>
    </div>
</asp:Panel>

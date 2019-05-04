<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Appuntamento.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Appuntamento" %>

<style>
    label {
        margin-bottom: 0px;
    }

</style>

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

            $('.calendar').on('dp.change', function (e) {
                var dataDa = $('#<%=txt_DataInizioLavorazione.ClientID%>');
                var dataA = $('#<%=txt_DataFineLavorazione.ClientID%>');

                $('#<%=txt_DurataLavorazione.ClientID%>').val(datediff(parseDate(dataDa.val()), parseDate(dataA.val())) + 1);

                var durataAndata = parseInt($('#<%=txt_DurataViaggioAndata.ClientID%>').val());
                var durataRitorno = parseInt($('#<%=txt_DurataViaggioRitorno.ClientID%>').val());
                $('#<%=txt_DataInizioImpegno.ClientID%>').val(convertDate(parseDate(dataDa.val()).addDays(durataAndata * -1)));
                $('#<%=txt_DataFineImpegno.ClientID%>').val(convertDate(parseDate(dataA.val()).addDays(durataRitorno)));
            });

            $('#<%=txt_DurataViaggioAndata.ClientID%>').on('change keyup paste', function () {
                var dataDa = $('#<%=txt_DataInizioLavorazione.ClientID%>');
                var durata = parseInt($('#<%=txt_DurataViaggioAndata.ClientID%>').val());

                $('#<%=txt_DataInizioImpegno.ClientID%>').val(convertDate(parseDate(dataDa.val()).addDays(durata * -1)));

            });

            $('#<%=txt_DurataViaggioRitorno.ClientID%>').on('change keyup paste', function () {
                var dataA = $('#<%=txt_DataFineLavorazione.ClientID%>');
                var durata = parseInt($('#<%=txt_DurataViaggioRitorno.ClientID%>').val());

                $('#<%=txt_DataFineImpegno.ClientID%>').val(convertDate(parseDate(dataA.val()).addDays(durata)));
            });

            // GESTIONE DROPDOWN RISORSE
            $("#filtroRisorse").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divRis .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoRisorse.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_Risorse.ClientID%>").val($(this.firstChild).attr('val'));
                $("#<%=ddl_Risorse.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Risorse.ClientID%>").attr("title", $(e.target).text());

                $("#<%=btn_Risorse.ClientID%>").click();
            });

            // GESTIONE DROPDOWN TIPOLOGIE
            $("#filtroTipologie").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divTip .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoTipologie.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_Tipologie.ClientID%>").val($(this.firstChild).attr('val'));
                $("#<%=ddl_Tipologie.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Tipologie.ClientID%>").attr("title", $(e.target).text());
            });

            // GESTIONE DROPDOWN CLIENTI
            $("#filtroClienti").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divClienti .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoClienti.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_Clienti.ClientID%>").val($(this.firstChild).attr('val'));
                $("#<%=ddl_Clienti.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Clienti.ClientID%>").attr("title", $(e.target).text());
            });

            // AUTOCOMPLETAMENTO
            <%--$("#<%=txt_Produzione.ClientID%>").easyAutocomplete(produzioni);
            $("#<%=txt_Lavorazione.ClientID%>").easyAutocomplete(lavorazioni);--%>

        });

    });

    function autocompletamento() {
        $("#<%=txt_Produzione.ClientID%>").easyAutocomplete(produzioni);
        $("#<%=txt_Lavorazione.ClientID%>").easyAutocomplete(lavorazioni);
    }

    function confermaEliminazione() {
        return confirm("Eliminare l'appuntamento corrente?");
    }

    function confermaCambioStato() {
        return confirm("Lo stato dell'evento sta per essere modificato.\n Le modifiche andranno perse se non verrà effettuato il salvataggio");
    }

    function setElenchi(elencoProduzioni, elencoLavorazioni) {
        produzioni = { data: elencoProduzioni, list: { match: { enabled: true } } };
        lavorazioni = { data: elencoLavorazioni, list: { match: { enabled: true } } };
    }

    var produzioni;
    var lavorazioni;
</script>


<asp:Panel runat="server" ID="panelAppuntamenti" ScrollBars="Auto">

    <div class="w3-container w3-center w3-xlarge">GESTIONE APPUNTAMENTI</div>

    <!--PIANIFICAZIONE-->
    <div class="w3-col" style="width: 100%">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right">
            <h4 class="w3-center">Pianificazione</h4>
            <div class="w3-padding">
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_DataInizioLavorazione" runat="server" Text="Data inizio lavorazione" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <asp:TextBox ID="txt_DataInizioLavorazione" runat="server" CssClass="w3-white w3-border w3-hover-shadow w3-round fieldMedium calendar" placeholder="gg/mm/aaaa"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_DataFineLavorazione" runat="server" Text="Data fine lavorazione" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div class="w3-row">
                            <div class="w3-third">
                                <asp:TextBox CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMedium calendar" ID="txt_DataFineLavorazione" placeholder="gg/mm/aaaa" runat="server" />
                            </div>
                            <div class="w3-twothird" style="text-align: right">
                                <asp:Label ID="lbl_DurataLavorazione" runat="server" Text="Durata lavorazione" class="label"></asp:Label>
                                <asp:TextBox ID="txt_DurataLavorazione" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldSmall" runat="server" MaxLength="2" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_DurataViaggioAndata" runat="server" Text="Durata viaggio andata" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div class="w3-row">
                            <div class="w3-third">
                                <asp:TextBox ID="txt_DurataViaggioAndata" runat="server" CssClass="w3-white w3-border w3-hover-shadow w3-round fieldSmall" MaxLength="2" Style="width: 30px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                            </div>
                            <div class="w3-twothird" style="text-align: right">
                                <asp:Label ID="lbl_DataInizioImpegno" runat="server" Text="Inizio impegno" class="label"></asp:Label>
                                <asp:TextBox ID="txt_DataInizioImpegno" runat="server" CssClass="w3-white w3-border w3-hover-shadow w3-round fieldMedium " Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_DurataViaggioRitorno" runat="server" Text="Durata viaggio ritorno" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div class="w3-row">
                            <div class="w3-third">
                                <asp:TextBox ID="txt_DurataViaggioRitorno" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldSmall" MaxLength="2" Style="width: 30px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                            </div>
                            <div class="w3-twothird" style="text-align: right">
                                <asp:Label ID="lbl_DataFineImpegno" runat="server" Text="Fine impegno" class="label"></asp:Label>
                                <asp:TextBox ID="txt_DataFineImpegno" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMedium" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--TIPOLOGIA  -->
    <div class="w3-col" style="width: 100%">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right">
            <h4 class="w3-center">Tipologia</h4>
            <div class="w3-padding">
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_Risorsa" runat="server" Text="Unità esterna" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div id="divRis" class="dropdown ">
                            <asp:HiddenField ID="hf_Risorse" runat="server" Value="" ClientIDMode="Static" />
                            <asp:Button ID="btn_Risorse" runat="server" Text="" Style="display: none" OnClick="btn_Risorse_Click" />
                            <asp:Button ID="ddl_Risorse" runat="server" CssClass="btn btn-primary dropdown-toggle w3-hover-shadow fieldMax" data-toggle="dropdown" data-boundary="divRis" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoRisorse" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;">
                                <input class="form-control" id="filtroRisorse" type="text" placeholder="Cerca..">
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_Tipologia" runat="server" Text="Tipologia" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div id="divTip" class="dropdown">
                            <asp:HiddenField ID="hf_Tipologie" runat="server" Value="" />
                            <asp:Button ID="ddl_Tipologie" runat="server" CssClass="btn btn-primary dropdown-toggle w3-hover-shadow fieldMax" data-toggle="dropdown" data-boundary="divTip" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoTipologie" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto">
                                <input class="form-control" id="filtroTipologie" type="text" placeholder="Cerca..">
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_tender" runat="server" Text="Unità appoggio" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div class=" w3-white w3-border w3-round w3-padding w3-hover-shadow" style="height: 80px; width: 95%; position: relative; overflow: auto;">
                            <asp:CheckBoxList ID="check_tender" AutoPostBack="True" runat="server" Height="50px" OnSelectedIndexChanged="check_tender_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!--DESCRIZIONE-->
    <div class="w3-col" style="width: 100%">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right">
            <h4 class="w3-center">Descrizione</h4>
            <div class="w3-padding">
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <div id="divClienti" class="dropdown ">
                            <asp:HiddenField ID="hf_Clienti" runat="server" Value="" />
                            <asp:Button ID="ddl_Clienti" runat="server" CssClass="btn btn-primary dropdown-toggle w3-hover-shadow fieldMax" data-toggle="dropdown" data-boundary="divClienti" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoClienti" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto">
                                <input class="form-control" id="filtroClienti" type="text" placeholder="Cerca..">
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <asp:TextBox ID="txt_Produzione" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMax" Style="padding: 3px;"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_lavorazione" runat="server" Text="Lavorazione" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <asp:TextBox ID="txt_Lavorazione" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMax" Style="padding: 3px;"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_luogo" runat="server" Text="Luogo" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <asp:TextBox ID="txt_Luogo" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMax"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-third">
                        <asp:Label ID="lbl_indirizzo" runat="server" Text="Descrizione / Via" class="label"></asp:Label>
                    </div>
                    <div class="w3-twothird">
                        <asp:TextBox ID="txt_Indirizzo" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round fieldMax"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!--NOTA-->
    <div class="w3-col" style="width: 100%">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right">
            <h4 class="w3-center">Nota</h4>
            <div class="w3-padding">
                <div class="w3-row" style="margin-bottom: 5px;">
                    <div class="w3-col">
                        <asp:TextBox ID="tb_Nota" Style="width: 100%; position: relative;" Rows="6" TextMode="MultiLine" runat="server" CssClass="w3-white w3-border w3-hover-shadow w3-round"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Panel>


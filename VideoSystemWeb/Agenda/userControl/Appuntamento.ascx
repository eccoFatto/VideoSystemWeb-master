<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Appuntamento.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Appuntamento" %>


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
            });

            $('#<%=txt_DurataViaggioAndata.ClientID%>').on('change keyup paste', function () {
                var dataDa = $('#<%=txt_DataInizioLavorazione.ClientID%>');
                var durata = parseInt($('#<%=txt_DurataViaggioAndata.ClientID%>').val());

                $('#<%=txt_DataInizioImpegno.ClientID%>').val(convertDate(parseDate(dataDa.val()).addDays(durata)));

            });

            $('#<%=txt_DurataViaggioRitorno.ClientID%>').on('change keyup paste', function () {
                var dataA = $('#<%=txt_DataFineLavorazione.ClientID%>');
                var durata = parseInt($('#<%=txt_DurataViaggioRitorno.ClientID%>').val());

                $('#<%=txt_DataFineImpegno.ClientID%>').val(convertDate(parseDate(dataA.val()).addDays(durata * -1)));
            });

            // GESTIONE DROPDOWN RISORSE
            $("#filtroRisorse").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divRis .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoRisorse.ClientID%> .elemLista").on("click", function (e) {
                $("#<%=hf_Risorse.ClientID%>").val($(this).attr('val'));
                $("#<%=ddl_Risorse.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Risorse.ClientID%>").attr("title", $(e.target).text());
            });

            // GESTIONE DROPDOWN TIPOLOGIE
            $("#filtroTipologie").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divTip .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#<%=elencoTipologie.ClientID%> .elemLista").on("click", function (e) {
                $("#<%=hf_Tipologie.ClientID%>").val($(this).attr('val'));
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

            $("#<%=elencoClienti.ClientID%> .elemLista").on("click", function (e) {
                $("#<%=hf_Clienti.ClientID%>").val($(this).attr('val'));
                $("#<%=ddl_Clienti.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Clienti.ClientID%>").attr("title", $(e.target).text());
            });

            $("#<%=txt_Produzione.ClientID%>").easyAutocomplete(produzioni);
            $("#<%=txt_Lavorazione.ClientID%>").easyAutocomplete(lavorazioni);

        });
        
    });

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


<asp:Panel runat="server" ID="panelAppuntamenti">
    
    <div class="w3-container w3-center w3-xlarge">GESTIONE APPUNTAMENTI</div>

    <!--LAVORAZIONE-->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right">
            <h4 class="w3-center">Lavorazione</h4>
            <div class="w3-row w3-section w3-padding-small">
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataInizioLavorazione" runat="server" Text="Data inizio lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox ID="txt_DataInizioLavorazione" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" placeholder="gg/mm/aaaa"></asp:TextBox>
                </div>

                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataFineLavorazione" runat="server" Text="Data fine lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium calendar" ID="txt_DataFineLavorazione" placeholder="gg/mm/aaaa" runat="server" />
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataLavorazione" runat="server" Text="Durata lavorazione" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <asp:TextBox ID="txt_DurataLavorazione" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" runat="server" MaxLength="2" Enabled="false"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <!--PIANIFICAZIONE  -->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue">
            <h4 class="w3-center">Pianificazione</h4>
            <div class="w3-row w3-section w3-padding-small">
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Risorsa" runat="server" Text="Pianificazione evento" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <%--<asp:DropDownList ID="ddl_Risorse" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax"></asp:DropDownList>--%>

                        <div id="divRis" class="dropdown" style="position: absolute; width: 190px;">
                            <asp:HiddenField ID="hf_Risorse" runat="server" Value="" />
                            <asp:Button ID="ddl_Risorse" runat="server" CssClass="btn btn-primary dropdown-toggle fieldMax" data-toggle="dropdown" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoRisorse" class="dropdown-menu" runat="server" style="transform: translateY(20px) !important; max-height: 350px; overflow: auto">
                                <input class="form-control" id="filtroRisorse" type="text" placeholder="Cerca..">
                            </ul>
                        </div>


                    </div>
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Tipologia" runat="server" Text="Tipologia" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <%--<asp:DropDownList ID="ddl_Tipologia" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax"></asp:DropDownList>--%>

                        <div id="divTip" class="dropdown" style="position: absolute; width: 190px;">
                            <asp:HiddenField ID="hf_Tipologie" runat="server" Value="" />
                            <asp:Button ID="ddl_Tipologie" runat="server" CssClass="btn btn-primary dropdown-toggle fieldMax" data-toggle="dropdown" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoTipologie" class="dropdown-menu" runat="server" style="transform: translateY(20px) !important; max-height: 350px; overflow: auto">
                                <input class="form-control" id="filtroTipologie" type="text" placeholder="Cerca..">
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>
                    <div class="w3-col" style="width: 49%; margin-bottom: 5px;">
                        <%--<asp:DropDownList ID="ddl_cliente" runat="server" CssClass="l w3-white w3-border w3-hover-orange w3-round fieldMax"></asp:DropDownList>--%>

                        <div id="divClienti" class="dropdown" style="position: absolute; width: 190px;">
                            <asp:HiddenField ID="hf_Clienti" runat="server" Value="" />
                            <asp:Button ID="ddl_Clienti" runat="server" CssClass="btn btn-primary dropdown-toggle fieldMax" data-toggle="dropdown" Text="" Style="text-overflow: ellipsis; overflow: hidden;" />
                            <ul id="elencoClienti" class="dropdown-menu" runat="server" style="transform: translateY(20px) !important; max-height: 350px; overflow: auto">
                                <input class="form-control" id="filtroClienti" type="text" placeholder="Cerca..">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--IMPEGNO-->
    <div class="w3-col m4">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-left">
            <h4 class="w3-center">Impegno</h4>
            <div class="w3-row w3-section w3-padding-small" <%--style="padding-left: 6px; padding-right: 6px;"--%>>
                <div class="w3-rest">
                    <div class="w3-col" style="width: 38%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataViaggioAndata" runat="server" Text="V. andata gg" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_DurataViaggioAndata" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                    </div>

                    <div class="w3-col" style="width: 62%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataInizioImpegno" runat="server" Text="Inizio impegno" CssClass="w3-yellow w3-border w3-round" Style="margin-left: 10px; padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_DataInizioImpegno" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldMedium " Style="float: right;" Enabled="false"></asp:TextBox>
                    </div>

                </div>
                <div class="w3-rest">

                    <div class="w3-col" style="width: 38%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DurataViaggioRitorno" runat="server" Text="V. ritorno gg" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_DurataViaggioRitorno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldSmall" MaxLength="2" Style="width: 30px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                    </div>


                    <div class="w3-col" style="width: 62%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_DataFineImpegno" runat="server" Text="Fine impegno" CssClass=" w3-yellow w3-border w3-round" Style="margin-left: 10px; padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_DataFineImpegno" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMedium" Style="float: right;" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <%--<div class="w3-rest">
                    <div class="w3-col" style="width: 45%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_ImpegnoOrario" runat="server" Text="Impegno orario" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:CheckBox ID="chk_ImpegnoOrario" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall" Style="width: 30px;" ClientIDMode="Static" />


                    </div>
                    <div class="w3-col" style="width: 55%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_ImpegnoOrarioDa" runat="server" Text="Dalle" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_ImpegnoOrarioDa" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall time" Enabled="false"></asp:TextBox>

                        <asp:Label ID="lbl_ImpegnoOrarioA" runat="server" Text="Alle" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>

                        <asp:TextBox ID="txt_ImpegnoOrarioA" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round fieldSmall time" Enabled="false"></asp:TextBox>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>

    <!--PRODUZIONE-->
    <div class="w3-col m6">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-right ">
            <h4 class="w3-center">Produzione</h4>
            <div class="w3-row w3-section w3-padding-small">
                <div class="w3-rest">
                    <div>
                        <div class="w3-col" style="width: 17%; margin-bottom: 5px;">
                            <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>

                        <div class="w3-col" style="margin-bottom: 5px; position:absolute; left:10.5%; width: 15.5%">
                            <asp:TextBox ID="txt_Produzione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" style="padding:3px;"></asp:TextBox>
                        </div>

                        <div class="w3-col" style=" margin-bottom: 5px; position:absolute; left:25.5%;width:10%;">
                            <asp:Label ID="lbl_lavorazione" runat="server" Text="Lavorazione" CssClass="w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>

                        <div class="w3-col" style=" margin-bottom: 5px; position:absolute; left:34%; width: 14%">
                            <asp:TextBox ID="txt_Lavorazione" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" style="padding:3px;"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="w3-rest">
                    <div>
                        <div class="w3-col" style="width: 17%; margin-bottom: 5px;">
                            <asp:Label ID="lbl_indirizzo" runat="server" Text="Via" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                        </div>
                        <div class="w3-col" style="width: 83%; margin-bottom: 5px;">
                            <asp:TextBox ID="txt_Indirizzo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round" Style="width: 98%"></asp:TextBox>
                        </div>

                    </div>
                </div>

                <div class="w3-rest">
                    <div class="w3-col" style="width: 17%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_luogo" runat="server" Text="Luogo" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>

                    <div class="w3-col" style="width: 33%; margin-bottom: 5px;">
                        <asp:TextBox ID="txt_Luogo" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax"></asp:TextBox>
                    </div>

                    <div class="w3-col" style="width: 20%; margin-bottom: 5px;">
                        <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                    </div>

                    <div class="w3-col" style="width: 30%; margin-bottom: 5px;">
                        <asp:TextBox ID="txt_CodiceLavoro" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--NOTA-->
    <div class="w3-col m6">
        <div class="w3-card-4 w3-light-grey w3-text-blue w3-margin-left">
            <h4 class="w3-center">Nota</h4>
            <div class="w3-row w3-section w3-padding-small">
                <div class="w3-col" style="width: 20%; margin-bottom: 5px;">
                    <asp:Label ID="lbl_nota" runat="server" Text="Nota" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                </div>

                <div class="w3-rest" style="width: 80%; margin-bottom: 5px;">
                    <%--<asp:TextBox ID="val_Nota" CssClass="w3-light-grey w3-border w3-round" runat="server" Rows="4" TextMode="MultiLine" Enabled="false" Style="width: 96%; margin-left: 10px; margin-bottom: 10px"></asp:TextBox>--%>
                    <asp:TextBox ID="tb_Nota" Style="width: 96%; position: relative;" Rows="3" TextMode="MultiLine" runat="server" CssClass="w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </div>
                <div class="w3-col" style="width: 20%; margin-bottom: 5px;">
                    <asp:Label ID="lbl_Stato" runat="server" Text="Stato attuale" CssClass=" w3-yellow w3-border w3-round" Style="padding-left: 5px; padding-right: 5px;"></asp:Label>
                </div>

                <div class="w3-rest" style="width: 80%; margin-bottom: 5px;">
                    <asp:HiddenField ID="hf_IdStato" runat="server" />
                    <asp:TextBox ID="txt_Stato" runat="server" CssClass=" w3-white w3-border w3-hover-orange w3-round fieldMax" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>





</asp:Panel>

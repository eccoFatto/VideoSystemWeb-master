<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Lavorazione.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Lavorazione" %>

<script>

    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            // MODIFICA FIGURA PROFESSIONALE PIANO ESTERNO
            $('#<%=chk_diaria.ClientID%>').click(function () {
                $('#<%=diaria15.ClientID%>').prop('checked', false);
                $('#<%=diaria15.ClientID%>').attr("disabled", !this.checked);
                $('#<%=diaria30.ClientID%>').prop('checked', false);
                $('#<%=diaria30.ClientID%>').attr("disabled", !this.checked);
                $('#<%=diariaLibera.ClientID%>').prop('checked', false);
                $('#<%=diariaLibera.ClientID%>').attr("disabled", !this.checked);
            });

            $('#<%=diaria15.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            $('#<%=diaria30.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            $('#<%=diariaLibera.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            if ($('#<%=diariaLibera.ClientID%>').prop("checked")) {
                $("#<%=txt_diaria.ClientID%>").attr("readonly", false);
                $("#<%=txt_diaria.ClientID%>").removeClass(" w3-disabled");
            } else {
                $("#<%=txt_diaria.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria.ClientID%>").addClass(" w3-disabled");
            }

            $('#<%=diaria15.ClientID%>').click(function () {
                $("#<%=txt_diaria.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria.ClientID%>").addClass(" w3-disabled");
            });
            $('#<%=diaria30.ClientID%>').click(function () {
                $("#<%=txt_diaria.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria.ClientID%>").addClass(" w3-disabled");
            });
            $('#<%=diariaLibera.ClientID%>').click(function () {
                $("#<%=txt_diaria.ClientID%>").attr("readonly", false);
                $("#<%=txt_diaria.ClientID%>").removeClass(" w3-disabled");
            });


            //INSERIMENTO GENERALE
            $('#<%=chk_diaria_InsGenerale.ClientID%>').click(function () {
                $('#<%=diaria15_InsGenerale.ClientID%>').prop('checked', false);
                $('#<%=diaria15_InsGenerale.ClientID%>').attr("disabled", !this.checked);
                $('#<%=diaria30_InsGenerale.ClientID%>').prop('checked', false);
                $('#<%=diaria30_InsGenerale.ClientID%>').attr("disabled", !this.checked);
                $('#<%=diariaLibera_InsGenerale.ClientID%>').prop('checked', false);
                $('#<%=diariaLibera_InsGenerale.ClientID%>').attr("disabled", !this.checked);
            });

            $('#<%=diaria15_InsGenerale.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            $('#<%=diaria30_InsGenerale.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            $('#<%=diariaLibera_InsGenerale.ClientID%>').attr("disabled", !$('#<%=chk_diaria.ClientID%>').prop("checked"));
            if ($('#<%=diariaLibera_InsGenerale.ClientID%>').prop("checked")) {
                $("#<%=txt_diaria_InsGenerale.ClientID%>").attr("readonly", false);
                $("#<%=txt_diaria_InsGenerale.ClientID%>").removeClass(" w3-disabled");
            } else {
                $("#<%=txt_diaria_InsGenerale.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria_InsGenerale.ClientID%>").addClass(" w3-disabled");
            }

            $('#<%=diaria15_InsGenerale.ClientID%>').click(function () {
                $("#<%=txt_diaria_InsGenerale.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria_InsGenerale.ClientID%>").addClass(" w3-disabled");
            });
            $('#<%=diaria30_InsGenerale.ClientID%>').click(function () {
                $("#<%=txt_diaria_InsGenerale.ClientID%>").attr("readonly", true);
                $("#<%=txt_diaria_InsGenerale.ClientID%>").addClass(" w3-disabled");
            });
            $('#<%=diariaLibera_InsGenerale.ClientID%>').click(function () {
                $("#<%=txt_diaria_InsGenerale.ClientID%>").attr("readonly", false);
                $("#<%=txt_diaria_InsGenerale.ClientID%>").removeClass(" w3-disabled");
            });


            $("#<%=txt_FiltroGruppiLavorazione.ClientID%>").val("");
            $("#<%=txt_FiltroGruppiLavorazione.ClientID%>").keyup(function () {
                filterLavorazione(2);
            });


            $('#<%=ddl_FPtipoPagamento.ClientID%>').change(function () {

                if ($('#<%=ddl_FPtipoPagamento.ClientID%>').val() == "") {
                    $("#<%=txt_FPnetto.ClientID%>").attr("readonly", true);
                    $("#<%=txt_FPnetto.ClientID%>").addClass(" w3-disabled");
                    $("#<%=txt_FPnetto.ClientID%>").val("0");

                    $("#<%=txt_Costo.ClientID%>").attr("readonly", false);
                    $("#<%=txt_Costo.ClientID%>").removeClass(" w3-disabled");
                } else {
                    $("#<%=txt_FPnetto.ClientID%>").attr("readonly", false);
                    $("#<%=txt_FPnetto.ClientID%>").removeClass(" w3-disabled");

                    $("#<%=txt_Costo.ClientID%>").attr("readonly", true);
                    $("#<%=txt_Costo.ClientID%>").addClass(" w3-disabled");
                }

                $("#<%=txt_FPnetto.ClientID%>").val("0");
                $("#<%=txt_FPRimborsoKM.ClientID%>").val("0");
                $("#<%=txt_FPlordo.ClientID%>").val("0");
                $("#<%=txt_Costo.ClientID%>").val("0");
            });

        });
    });

    function calcolaLordo() {
        var valoreNetto = ($('#<%=txt_FPnetto.ClientID%>').val()).replace(",",".");
        var tipoPagamento = $('#<%=ddl_FPtipoPagamento.ClientID%>').val();
        var valoreLordo = "";

        //alert(tipoPagamento);
        if (tipoPagamento == "1") { // ASSUNZIONE
            valoreLordo = 2 * valoreNetto;
        } else if (tipoPagamento == "2") { // MISTA 
            var rimborsoKm = valoreNetto - <%=quotaFissa_PagamentoMisto%>;//  45;
            $('#<%=txt_FPRimborsoKM.ClientID%>').val(rimborsoKm);

            valoreLordo = 90 + rimborsoKm;
        } else if (tipoPagamento == "3") { // RITENUTA ACCONTO
            valoreLordo = valoreNetto / <%=aliquota_RitenutaAcconto%>;//0.8;
        } else if (tipoPagamento == "4") { // FATTURA
            valoreLordo = valoreNetto;
        } 

        $('#<%=txt_FPlordo.ClientID%>').val(valoreLordo.toString().replace(".", ","));
    }

    function filterLavorazione(columnIndex) {
        var filterText = $("#<%=txt_FiltroGruppiLavorazione.ClientID%>").val().toLowerCase();
        var cellText = "";

        $("#<%=gvGruppiLavorazione.ClientID%> tr:has(td)").each(function () {
            cellText = $(this).find("td:eq(" + columnIndex + ")").text().toLowerCase();
            cellText = $.trim(cellText);

            if (cellText.indexOf(filterText) == -1) {
                $(this).css('display', 'none');
            }
            else {
                $(this).css('display', '');
            }
        });
    }

    function openTabEventoLavorazione(evt, tipoName) {
        $("#<%=hf_tabSelezionataLavorazione.ClientID%>").val(tipoName);

        var i, x, tablinks;
        x = document.getElementsByClassName("tabLavorazione");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinkLav");
        for (i = 0; i < x.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
        }
        document.getElementById(tipoName).style.display = "block";
        evt.currentTarget.className += " w3-red";

        var nomeElemento = '';

        if (tipoName == 'Lavoraz') {
            nomeElemento = '<%=tab_Lavorazione.ClientID%>';

        } else if (tipoName == 'DettEconomico') {
            nomeElemento = '<%=tab_DettEconomico.ClientID%>';

        } else if (tipoName == 'PianoEsterno') {
            nomeElemento = '<%=tab_PianoEsterno.ClientID%>';


        }
        if (document.getElementById(nomeElemento).className.indexOf("w3-red") == -1)
            document.getElementById(nomeElemento).className += " w3-red";
    }

    function confermaEliminazioneArticolo() {
        return confirm("Eliminare l'articolo corrente?");
    }


    function confermaEliminazioneFigProf() {
        return confirm("Eliminare la figura professionale corrente?");
    }

    function enterEvent(e) {
    if (e.keyCode == 13) {
        $("input[id=<%=btn_Cerca.ClientID%>]").click();
    }
    }
</script>

<asp:HiddenField ID="hf_tabSelezionataLavorazione" runat="server" EnableViewState="true" Value="Lavoraz" />

<asp:Panel runat="server" ID="panelLavorazione" Style="height: 99%">
    <div class="w3-row">
        <div class="w3-col">
            <div class="w3-container w3-center w3-xlarge">LAVORAZIONE</div>
        </div>

    </div>


    <div class="w3-container" style="width: 100%; height: 99%; position: relative; padding: 0px;">

        <div id="Lavoraz" class="w3-container w3-border tabLavorazione w3-padding-small" style="height: 90%; overflow: auto;">
            <div class="w3-row">
                <div class="w3-third">
                    <label>Ordine</label><br />
                    <asp:TextBox ID="txt_Ordine" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round "></asp:TextBox>
                </div>

                <div class="w3-third">
                    <label>Fattura</label><br />
                    <asp:TextBox ID="txt_Fattura" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round w3-disabled" Enabled="false"></asp:TextBox>
                </div>

                <div class="w3-third">
                    <label>Contratto</label><br />
                    <asp:DropDownList ID="ddl_Contratto" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round "></asp:DropDownList>
                </div>
            </div>
            <div class="w3-row">&nbsp;</div>
            <div class="w3-row">
                <div class="w3-third">
                    <label>Referente</label><br />
                    <asp:DropDownList ID="ddl_Referente" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round "></asp:DropDownList>
                </div>

                <div class="w3-third">
                    <label>Capotecnico</label><br />
                    <asp:DropDownList ID="ddl_Capotecnico" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round "></asp:DropDownList>
                </div>

                <div class="w3-third">
                    <label>Produttore</label><br />
                    <asp:DropDownList ID="ddl_Produttore" runat="server" CssClass=" w3-white w3-border w3-hover-shadow w3-round "></asp:DropDownList>
                </div>
            </div>
        </div>

        <div id="DettEconomico" class="w3-container w3-border tabLavorazione w3-padding-small" style="height: 90%; overflow: auto; display: none">
            <div class="w3-row" style="height: 60%; font-size: small;">
                <div class="w3-col" style="height: 80%">
                    <asp:Panel runat="server" ID="panelArticoli" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
                        <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Articoli selezionati</p>
                        <asp:Label ID="lbl_selezionareArticolo" runat="server" Text="Selezionare un articolo dalla lista" Style="position: absolute; top: 45%; left: 38%; font-size: large; color: cornflowerblue" />
                        <asp:GridView ID="gvArticoliLavorazione" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvArticoliLavorazione_RowCommand" DataMember="IdentificatoreOggetto" OnRowDataBound="gvArticoliLavorazione_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="15%" />
                                <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" HeaderStyle-Width="15%" />
                                <asp:TemplateField HeaderText="Seleziona" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUp" runat="server" ImageUrl="/Images/arrow-up-icon.png" ToolTip="Sposta su" CommandName="moveUp" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDown" runat="server" ImageUrl="/Images/arrow-down-icon.png" ToolTip="Sposta giù" CommandName="moveDown" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/Male-user-edit-icon.png" ToolTip="Modifica e aggiungi riferimento" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" CommandName="elimina" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' OnClientClick="return confermaEliminazioneArticolo();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Collab./Fornitore" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Riferimento" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Prezzo" HeaderText="Listino" DataFormatString="{0:N2}" HeaderStyle-Width="8%" />
                                <asp:TemplateField HeaderText="Costo">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Costo" runat="server" HeaderStyle-Width="8%" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FP_lordo" HeaderText="Lordo" DataFormatString="{0:N2}" HeaderStyle-Width="8%" />
                                <asp:TemplateField HeaderText="Tipo pagamento" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_TipoPagamento" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Stampa" HeaderText="Stampa" HeaderStyle-Width="6%" />
                            </Columns>

                        </asp:GridView>
                    </asp:Panel>
                </div>

                <div class="w3-col" style="height: 15%; padding-left: 5px; margin-bottom: 20px;">
                    <div class="w3-col" style="padding: 5px; width: 20%">
                        <label style="margin-bottom: 0.2rem;">Totale listino</label><br />
                        <asp:TextBox ID="txt_TotPrezzo" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-col" style="padding: 5px; width: 20%">
                        <label style="margin-bottom: 0.2rem;">Totale costo</label><br />
                        <asp:TextBox ID="txt_TotCosto" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-col" style="padding: 5px; width: 20%">
                        <label style="margin-bottom: 0.2rem;">Totale lordo</label><br />
                        <asp:TextBox ID="txt_TotLordo" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-col" style="padding: 5px; width: 20%">
                        <label style="margin-bottom: 0.2rem;">Totale IVA</label><br />
                        <asp:TextBox ID="txt_TotIva" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-col" style="padding: 5px; width: 20%">
                        <label style="margin-bottom: 0.2rem;">% Ricavo</label><br />
                        <asp:TextBox ID="txt_PercRicavo" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>


            <div class="w3-col" style="height: 25%; position: relative;">
                <asp:Panel runat="server" ID="panelGruppi" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
                    <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Articoli e Articoli composti</p>
                    <asp:GridView ID="gvGruppiLavorazione" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvGruppiLavorazione_RowCommand" DataMember="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" />
                            <asp:BoundField DataField="nome" HeaderText="Descrizione" />
                            <asp:BoundField DataField="Descrizione" HeaderText="Descrizione lunga" />
                            <asp:TemplateField HeaderText="Seleziona">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgOk" runat="server" ImageUrl="/Images/add.png" ToolTip="Aggiungi" CommandName="aggiungi" CommandArgument='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <div class="w3-col">
                    <asp:TextBox ID="txt_FiltroGruppiLavorazione" runat="server" CssClass="w3-round" placeholder="Cerca.." Style="width: 100%; padding: 5px; margin-top: 10px;"></asp:TextBox>
                </div>
            </div>

            <div id="panelModificaArticolo" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                <asp:UpdatePanel ID="upModificaArticolo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <div class="w3-modal-content w3-card-4 w3-animate-top round" style="position: relative; width: 80%; background-color: white; overflow: auto; max-height: 80%;">
                            <div class="w3-row-padding">

                                <div class="w3-panel w3-blue w3-center w3-round">
                                    <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Articolo</b> </h5>
                                    <span onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                                </div>
                                <div class="w3-col round" style="padding: 5px;">
                                    <div class="w3-half" style="padding: 5px;">

                                        <div class="w3-col">
                                            <label style="margin-bottom: 0.2rem;">Descrizione</label>
                                            <asp:TextBox ID="txt_Descrizione" runat="server" class="w3-input w3-border w3-round-medium" MaxLength="60" placeholder="Descrizione" Style="padding: 2px;"></asp:TextBox>
                                        </div>

                                        <div class="w3-col">
                                            <div class="w3-half">

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Prezzo</label>
                                                    <asp:TextBox ID="txt_Prezzo" runat="server" class="w3-input w3-border w3-round-medium" placeholder="Prezzo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Costo</label>
                                                    <asp:TextBox ID="txt_Costo" runat="server" class="w3-input w3-border w3-round-medium" placeholder="Costo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                                <div class="w3-third" style="padding: 5px">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <div class="w3-half">
                                                <div class="w3-third" style="padding: 5px">&nbsp;</div>

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Iva</label>
                                                    <asp:TextBox ID="txt_Iva" runat="server" class="w3-input w3-border w3-round-medium" placeholder="iva" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Stampa</label><br />
                                                    <asp:DropDownList ID="ddl_Stampa" runat="server">
                                                        <asp:ListItem Value="1" Text="SI" />
                                                        <asp:ListItem Value="0" Text="NO" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="w3-half" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Descrizione lunga</label>
                                        <asp:TextBox ID="txt_DescrizioneLunga" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border w3-round-medium" MaxLength="100" placeholder="Descrizione lunga" Style="padding: 2px;"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />

                            <div class="w3-row-padding">
                                <div class="w3-panel w3-amber w3-center w3-round">
                                    <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Figura professionale</b> </h5>
                                </div>

                                <div class="w3-col round" style="padding: 5px;">
                                    <div class="w3-row " style="background-color: #cccccc;">
                                        <div class="w3-col" style="padding: 5px; width: 23%">
                                            <label style="margin-bottom: 0.2rem;">Tipo figura professionale</label>
                                            <asp:DropDownList ID="ddl_FPtipo" class="w3-input w3-border w3-round-medium" runat="server" Style="padding: 2px;"  AutoPostBack="true">
                                                <asp:ListItem Value="" Text="<seleziona>" />
                                                <asp:ListItem Value="0" Text="Collaboratore" />
                                                <asp:ListItem Value="1" Text="Fornitore" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="w3-col" style="padding: 5px; width: 23%">
                                            <label style="margin-bottom: 0.2rem;">Qualifica</label>
                                            <asp:DropDownList ID="ddl_FPqualifica" class="w3-input w3-border w3-round-medium" runat="server" Style="padding: 2px;" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="w3-col" style="padding: 5px; width: 23%">
                                            <label style="margin-bottom: 0.2rem;">Città</label>
                                            <asp:TextBox ID="txt_FPCitta" class="w3-input w3-border w3-round-medium" runat="server" Style="padding: 2px;" onkeydown="javascript:enterEvent(event);"></asp:TextBox>
                                        </div>
                                        <div class="w3-col" style="padding: 5px; width: 25%">
                                            <label style="margin-bottom: 0.2rem;">Nominativo (Cognome Nome)</label>
                                            <asp:TextBox ID="txt_FPNominativo" class="w3-input w3-border w3-round-medium" runat="server" Style="padding: 2px;" onkeydown="javascript:enterEvent(event);"></asp:TextBox>
                                        </div>
                                        <div class="w3-rest" style="padding: 5px;">
                                            <asp:Button ID="btn_Cerca" class=" w3-btn w3-white w3-border w3-border-green w3-round-medium" Style="font-size: smaller; padding: 4px 8px; margin-top: 21px;" runat="server" Text="Cerca" OnClick="btn_Cerca_Click"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="w3-row w3-border" style="margin-top: 20px; max-height: 190px;overflow:auto">
                                        <asp:GridView ID="gv_FigureProfessionaliModifica" runat="server" AutoGenerateColumns="False"
                                            Style="font-size: 8pt; max-height: 150px; width: 100%; position: relative; background-color: #FFF; text-align: center"
                                            HeaderStyle-BackColor="#2196F3" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" BorderWidth="0"
                                            GridLines="None" AllowPaging="True" PageSize="5" OnPageIndexChanging="gv_FigureProfessionaliModifica_PageIndexChanging" 
                                            OnRowCommand="gv_FigureProfessionaliModifica_RowCommand" OnRowCreated="gv_FigureProfessionali_RowCreated">
                                            <Columns>
                                                <asp:BoundField DataField="NominativoCompleto" HeaderText="Nominativo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="DecodificaTipo" HeaderText="Tipo Fig. prof." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ElencoQualifiche" HeaderText="Qualifiche" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Citta" HeaderText="Città" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left"/>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_SelezionaFP" runat="server" ImageUrl="/Images/Male-user-accept-icon.png" ToolTip="Seleziona la Figura Professionale corrente" CommandName="seleziona" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") + "," + Eval("Tipo") %>' Height="15"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div class="w3-row" id="div_FiguraProfessionale" runat="server" visible="false">
                                        <div class="w3-row" style="text-align:center">
                                            <h5 style="margin-top:5px;margin-bottom:5px;"><b><asp:Label ID="lbl_NominativoFiguraProfessionale" runat="server" /></b></h5>
                                            <asp:HiddenField ID="hf_IdFiguraProfessionale" runat="server" />
                                        </div>
                                        <div class="w3-third" style="padding: 5px">
                                            <label style="margin-bottom: 0.2rem;">Nota Collaboratore</label>
                                            <asp:TextBox ID="txt_FPnotaCollaboratore" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border w3-round-medium" MaxLength="100" placeholder="Nota collaboratore" Style="padding: 2px;"></asp:TextBox>
                                        </div>

                                        <div class="w3-twothird" style="padding: 5px">
                                            <div class="w3-row">
                                                
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Telefono</label><br />
                                                    <asp:TextBox ID="txt_FPtelefono" class="w3-input w3-border  w3-round-medium w3-disabled" ReadOnly runat="server" Style="padding: 2px;"></asp:TextBox>
                                                </div>

                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Tipo pagamento</label>
                                                    <asp:DropDownList ID="ddl_FPtipoPagamento" class="w3-input w3-border w3-round-medium" runat="server" Style="padding: 2px;"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="w3-row">
                                               
                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Netto</label>
                                                    <asp:TextBox ID="txt_FPnetto" runat="server" class="w3-input w3-border w3-round-medium " placeholder="Netto" Style="padding: 2px;" onkeypress="return onlyNumbers();" onkeyup="calcolaLordo()"></asp:TextBox>
                                                </div>

                                                <div class="w3-third" style="padding: 5px;">
                                                    <label style="margin-bottom: 0.2rem;">Rimborso km</label><br />
                                                    <asp:TextBox ID="txt_FPRimborsoKM" runat="server" class="w3-input w3-border w3-round-medium" disabled placeholder="Rimborso km" Style="padding: 2px;"></asp:TextBox>
                                                </div>

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Lordo</label>
                                                    <asp:TextBox ID="txt_FPlordo" runat="server" class="w3-input w3-border w3-round-medium" disabled placeholder="Lordo" Style="padding: 2px;"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="w3-center" style="margin: 10px">
                                <asp:Button ID="btnOKModificaArtLavorazione" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKModificaArtLavorazione_Click" />
                                <button onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

        <div id="PianoEsterno" class="w3-container w3-border tabLavorazione w3-padding-small" style="height: 90%; overflow: auto; display: none">
            <div class="w3-row" style="height: 60%; font-size: small;">
                <div class="w3-col" style="height: 80%">
                    <asp:Panel runat="server" ID="panel1" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
                        <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Figure professionali</p>
                        <asp:Label ID="lbl_nessunaFiguraProf" runat="server" Text="Nessuna figura professionale importata" Style="position: absolute; top: 45%; left: 38%; font-size: large; color: cornflowerblue" />
                        <asp:GridView ID="gvFigProfessionali" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" 
                            OnRowCommand="gvFigProfessionali_RowCommand" DataMember="IdentificatoreOggetto">
                            <Columns>
                                <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:BoundField DataField="Elencoqualifiche" HeaderText="Qualifiche"/>

                                <asp:BoundField DataField="NominativoCompleto" HeaderText="Personale" />
                               
                                <asp:TemplateField HeaderText="Seleziona">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUp" runat="server" ImageUrl="/Images/arrow-up-icon.png" ToolTip="Sposta su" CommandName="moveUp" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDown" runat="server" ImageUrl="/Images/arrow-down-icon.png" ToolTip="Sposta giù" CommandName="moveDown" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" CommandName="elimina" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' OnClientClick="return confermaEliminazioneFigProf();" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Intervento" HeaderText="Intervento" />
                                <asp:BoundField DataField="Diaria" HeaderText="Diaria" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="Nota" HeaderText="Note" />
                                
                            </Columns>

                        </asp:GridView>
                    </asp:Panel>
                </div>

                <div class="w3-col" style="height: 5%; padding-left: 5px; margin-bottom: 20px;">
                    <div class="w3-col w3-center-align" style="padding: 20px; text-align:center">
                        <label>Filtro data lavorazione</label>
                        <asp:DropDownList ID="ddl_FiltroGiorniLavorazione" runat="server" AutoPostBack="true" CssClass=" w3-white w3-border w3-hover-shadow w3-round " OnSelectedIndexChanged="ddl_FiltroGiorniLavorazione_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>

                <div class="w3-col" style="height: 20%; padding-left: 5px; margin-bottom: 20px; padding-top:20px;">
                    <div class="w3-col " style="padding: 8px; text-align:center">
                        <asp:Button ID="btnImporta" runat="server" Text="Importa" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnImporta_Click" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                        
                        <asp:Button ID="btnInserimentoGenerale" runat="server" Text="Inserimento generale" class=" w3-btn w3-white w3-border w3-border-blue w3-round-large" OnClick="btnInserimentoGenerale_Click" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                    </div>
                    
                </div>

                <div class="w3-col" style="height: 20%;">
                    <label style="margin-bottom: 0.2rem;">Note piano esterno</label>
                    <asp:TextBox ID="txt_notaGeneralePianoEsterno" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-round-large w3-border w3-hover-shadow" Style="padding: 2px; "></asp:TextBox>
                </div>

<!-- MODIFICA ELEMENTO SINGOLO -->
                <div id="panelModificaPianoEsterno" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                    <asp:UpdatePanel ID="upModificaPianoEsterno" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <div class="w3-modal-content w3-card-4 w3-animate-top round" style="position: relative; width: 80%; background-color: white; overflow: auto;">
                                <div class="w3-row-padding">

                                    <div class="w3-panel w3-blue w3-center w3-round">
                                        <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Piano esterno Figura professionale</b> </h5>
                                        <span onclick="document.getElementById('<%= panelModificaPianoEsterno.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                                    </div>
                                    <div class="w3-col round" style="padding: 5px;">

                                        <div class="w3-twothird" style="padding: 5px;">
                                            <div class="w3-col">
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Data convocazione</label>
                                                    <asp:TextBox ID="txt_data" runat="server" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA" Style="padding: 2px;"></asp:TextBox>
                                                </div>
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Orario convocazione</label>
                                                    <asp:TextBox ID="txt_orario" runat="server" class="w3-input w3-border time" placeholder="hh:mm" Style="padding: 2px;"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="w3-col">
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Intervento</label><br />
                                                    <asp:DropDownList ID="ddl_intervento" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Albergo</label>
                                                    <asp:CheckBox ID="chk_albergo" runat="server" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="w3-third" style="padding: 5px">
                                            <div class="w3-col">
                                                <label style="margin-bottom: 0.2rem;">Diaria</label>
                                                <asp:CheckBox ID="chk_diaria" runat="server" /><br />
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diaria15" runat="server" GroupName="radioDiaria" Style="margin: 5px" /><asp:Label ID="val_diaria15" runat="server" Text="15€" />
                                                </div>
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diaria30" runat="server" GroupName="radioDiaria" Style="margin: 5px" /><asp:Label ID="val_diaria30" runat="server" Text="30€" />
                                                </div>
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diariaLibera" runat="server" GroupName="radioDiaria" Style="margin: 5px; float: left" /><asp:TextBox ID="txt_diaria" runat="server" class="w3-input w3-border w3-disabled" Style="padding: 2px; width: 100px" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="w3-row-padding" id="div_note" runat="server">
                                            <div class="w3-col" style="padding: 5px">
                                                <label style="margin-bottom: 0.2rem;">Note</label>
                                                <asp:TextBox ID="txt_notaPianoEsterno" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border" Style="padding: 2px;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />

                                <div class="w3-center" style="margin: 10px">
                                    <asp:Button ID="btnOKModificaPianoEsterno" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKModificaPianoEsterno_Click" />
                                    <button onclick="document.getElementById('<%= panelModificaPianoEsterno.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

<!-- INSERIMENTO GENERALE -->
                <div id="panelInserimentoGeneralePianoEsterno" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                    <asp:UpdatePanel ID="upInserimentoGeneralePianoEsterno" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <div class="w3-modal-content w3-card-4 w3-animate-top round" style="position: relative; width: 80%; background-color: white; overflow: auto; min-height:350px;">
                                <div class="w3-row-padding">

                                    <div class="w3-panel w3-blue w3-center w3-round">
                                        <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Inserimento generale</b> </h5>
                                        <span onclick="document.getElementById('<%= panelInserimentoGeneralePianoEsterno.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                                    </div>
                                    <div class="w3-col round" style="padding: 5px;">

<%--                                        <div class="w3-twothird" style="padding: 5px;">
                                            <div class="w3-col">--%>
                                                
                                                <div class="w3-quarter" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Orario convocazione</label>
                                                    <asp:TextBox ID="txt_orario_InsGenerale" runat="server" class="w3-input w3-border time" placeholder="hh:mm" Style="padding: 2px;"></asp:TextBox>
                                                </div>
                                                <div class="w3-quarter" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Albergo</label><br />
                                                    <asp:CheckBox ID="chk_albergo_InsGenerale" runat="server" />
                                                </div>
                                            <%--</div>
                                            <div class="w3-col">--%>
                                                <div class="w3-quarter" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Intervento</label><br />
                                                    <asp:DropDownList ID="ddl_intervento_InsGenerale" runat="server"></asp:DropDownList>
                                                </div>
                                           <%-- </div>--%>
                                       <%-- </div>--%>

                                        <div class="w3-quarter" style="padding: 5px">
                                            <div class="w3-col">
                                                <label style="margin-bottom: 0.2rem;">Diaria</label>
                                                <asp:CheckBox ID="chk_diaria_InsGenerale" runat="server" /><br />
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diaria15_InsGenerale" runat="server" GroupName="radioDiaria" Style="margin: 5px" /><asp:Label ID="Label1" runat="server" Text="15€" />
                                                </div>
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diaria30_InsGenerale" runat="server" GroupName="radioDiaria" Style="margin: 5px" /><asp:Label ID="Label2" runat="server" Text="30€" />
                                                </div>
                                                <div class="w3-row">
                                                    <asp:RadioButton ID="diariaLibera_InsGenerale" runat="server" GroupName="radioDiaria" Style="margin: 5px; float: left" /><asp:TextBox ID="txt_diaria_InsGenerale" runat="server" class="w3-input w3-border w3-disabled" Style="padding: 2px; width: 100px" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />

                                <div class="w3-center" style="margin: 10px; bottom:15px;position:absolute;width:99%;">
                                    <asp:Button ID="btnOKInserimentoGenerale" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOKInserimentoGenerale_Click" />
                                    <button onclick="document.getElementById('<%= panelInserimentoGeneralePianoEsterno.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


            </div>
        </div>

        <div class="w3-bar w3-blue w3-round">
            <div class="w3-row">
                <div class="w3-col" style="width: 100%">
                    <div class="w3-third w3-center tablinkLav w3-red" style="padding: 3px; cursor: pointer" runat="server" id="tab_Lavorazione" onclick="openTabEventoLavorazione(event, 'Lavoraz')">Lavorazione</div>
                    <div class="w3-third w3-center tablinkLav" style="padding: 3px; cursor: pointer" runat="server" id="tab_DettEconomico" onclick="openTabEventoLavorazione(event, 'DettEconomico')">Dettaglio economico</div>
                    <div class="w3-third w3-center tablinkLav" style="padding: 3px; cursor: pointer;" runat="server" id="tab_PianoEsterno" onclick="openTabEventoLavorazione(event, 'PianoEsterno')">Piano esterno</div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Lavorazione.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Lavorazione" %>


<script>

    $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            $('#<%=diaria15.ClientID%>').prop('checked', false);
            $('#<%=diaria30.ClientID%>').prop('checked', false);
            $('#<%=diariaLibera.ClientID%>').prop('checked', false);
            $("#<%=txt_diaria.ClientID%>").attr("readonly", true);


            $("#<%=txt_FiltroGruppiLavorazione.ClientID%>").val("");
            $("#<%=txt_FiltroGruppiLavorazione.ClientID%>").keyup(function () {
                filterLavorazione(2);
            });

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


            $('#<%=chk_ModCosto.ClientID%>').click(function () {

                if (this.checked) {
                    $("#<%=txt_FPnetto.ClientID%>").attr("readonly", false);
                    $("#<%=txt_FPnetto.ClientID%>").removeClass(" w3-disabled");

                    $("#<%=txt_Costo.ClientID%>").attr("readonly", true);
                    $("#<%=txt_Costo.ClientID%>").addClass(" w3-disabled");
                    $("#<%=txt_Costo.ClientID%>").val("0");

                }
                else {
                    $("#<%=txt_FPnetto.ClientID%>").attr("readonly", true);
                    $("#<%=txt_FPnetto.ClientID%>").addClass(" w3-disabled");
                    $("#<%=txt_FPnetto.ClientID%>").val("0");

                    $("#<%=txt_FPnetto.ClientID%>").val("0");

                    $("#<%=txt_Costo.ClientID%>").attr("readonly", false);
                    $("#<%=txt_Costo.ClientID%>").removeClass(" w3-disabled");

                }
            });
        });
    });

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
</script>

<asp:HiddenField ID="hf_tabSelezionataLavorazione" runat="server" EnableViewState="true" Value="Lavoraz" />

<asp:Panel runat="server" ID="panelLavorazione" Style="height: 99%">
    <div class="w3-container w3-center w3-xlarge">LAVORAZIONE</div>

    <div class="w3-container" style="width: 100%; height: 99%; position: relative; padding: 0px;">


        <div id="Lavoraz" class="w3-container w3-border tabLavorazione w3-padding-small" style="height: 90%; overflow: auto;">
            <div class="w3-row">
                <div class="w3-third">
                    <label>Ordine</label><br />
                    <asp:TextBox ID="txt_Ordine" runat="server"></asp:TextBox>
                </div>

                <div class="w3-third">
                    <label>Fattura</label><br />
                    <asp:TextBox ID="txt_Fattura" runat="server" Enabled="false"></asp:TextBox>
                </div>

                <div class="w3-third">
                    <label>Contratto</label><br />
                    <asp:DropDownList ID="ddl_Contratto" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="w3-row">&nbsp;</div>
            <div class="w3-row">
                <div class="w3-third">
                    <label>Referente</label><br />
                    <asp:DropDownList ID="ddl_Referente" runat="server"></asp:DropDownList>
                </div>

                <div class="w3-third">
                    <label>Capotecnico</label><br />
                    <asp:DropDownList ID="ddl_Capotecnico" runat="server"></asp:DropDownList>
                </div>

                <div class="w3-third">
                    <label>Produttore</label><br />
                    <asp:DropDownList ID="ddl_Produttore" runat="server"></asp:DropDownList>
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
                                <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                                <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="TV" HeaderText="TV" />
                                <asp:BoundField DataField="Prezzo" HeaderText="Listino" DataFormatString="{0:N2}" />
                                <asp:TemplateField HeaderText="Costo">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Costo" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Stampa" HeaderText="Stampa" />
                                <asp:TemplateField HeaderText="Riferimento">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Riferimento" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo pagamento">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_TipoPagamento" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Seleziona">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUp" runat="server" ImageUrl="/Images/arrow-up-icon.png" ToolTip="Sposta su" CommandName="moveUp" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDown" runat="server" ImageUrl="/Images/arrow-down-icon.png" ToolTip="Sposta giù" CommandName="moveDown" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/user-add-icon.png" ToolTip="Modifica e aggiungi riferimento" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" CommandName="elimina" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' OnClientClick="return confermaEliminazioneArticolo();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </asp:Panel>
                </div>

                <div class="w3-col" style="height: 15%; padding-left: 5px; margin-bottom: 20px;">
                    <div class="w3-quarter" style="padding: 5px">
                        <label style="margin-bottom: 0.2rem;">Totale prezzo</label><br />
                        <asp:TextBox ID="txt_TotPrezzo" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
                        <label style="margin-bottom: 0.2rem;">Totale costo</label><br />
                        <asp:TextBox ID="txt_TotCosto" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
                        <label style="margin-bottom: 0.2rem;">Totale IVA</label><br />
                        <asp:TextBox ID="txt_TotIva" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
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
                        <div class="w3-modal-content w3-card-4 round" style="position: relative; width: 80%; background-color: white; overflow: auto;">
                            <div class="w3-row-padding">

                                <div class="w3-panel w3-blue w3-center w3-round">
                                    <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Articolo</b> </h5>
                                    <span onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                                </div>
                                <div class="w3-col round" style="padding: 5px;">
                                    <div class="w3-half" style="padding: 5px;">

                                        <div class="w3-col">
                                            <label style="margin-bottom: 0.2rem;">Descrizione</label>
                                            <asp:TextBox ID="txt_Descrizione" runat="server" class="w3-input w3-border" MaxLength="60" placeholder="Descrizione" Style="padding: 2px;"></asp:TextBox>
                                        </div>

                                        <div class="w3-col">
                                            <div class="w3-half">

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Prezzo</label>
                                                    <asp:TextBox ID="txt_Prezzo" runat="server" class="w3-input w3-border" placeholder="Prezzo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Costo</label>
                                                    <asp:TextBox ID="txt_Costo" runat="server" class="w3-input w3-border" placeholder="Costo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                </div>
                                                <div class="w3-third" style="padding: 5px">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <div class="w3-half">
                                                <div class="w3-third" style="padding: 5px">
                                                    &nbsp;
                                                </div>

                                                <div class="w3-third" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Iva</label>
                                                    <asp:TextBox ID="txt_Iva" runat="server" class="w3-input w3-border" placeholder="iva" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
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
                                        <asp:TextBox ID="txt_DescrizioneLunga" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border" MaxLength="100" placeholder="Descrizione lunga" Style="padding: 2px;"></asp:TextBox>
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
                                    <div class="w3-row " style="background-color: #cccccc">
                                        <div class="w3-third" style="padding: 5px">
                                            <label style="margin-bottom: 0.2rem;">Tipo figura professionale</label>
                                            <asp:DropDownList ID="ddl_FPtipo" class="w3-input w3-border" runat="server" Style="padding: 2px;" OnSelectedIndexChanged="filtraFP" AutoPostBack="true">
                                                <asp:ListItem Value="0" Text="Collaboratore" />
                                                <asp:ListItem Value="1" Text="Fornitore" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="w3-third" style="padding: 5px">
                                            <label style="margin-bottom: 0.2rem;">Qualifica</label>
                                            <asp:DropDownList ID="ddl_FPqualifica" class="w3-input w3-border" runat="server" Style="padding: 2px;" OnSelectedIndexChanged="filtraFP" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="w3-third" style="padding: 5px">
                                            <label style="margin-bottom: 0.2rem;">Città</label>
                                            <asp:DropDownList ID="ddl_FPcitta" class="w3-input w3-border" runat="server" Style="padding: 2px;" OnSelectedIndexChanged="filtraFP" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="w3-row">
                                        <div class="w3-third" style="padding: 5px">
                                            <label style="margin-bottom: 0.2rem;">Nota Collaboratore</label>
                                            <asp:TextBox ID="txt_FPnotaCollaboratore" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border" MaxLength="100" placeholder="Nota collaboratore" Style="padding: 2px;"></asp:TextBox>
                                        </div>

                                        <div class="w3-twothird" style="padding: 5px">
                                            <div class="w3-row">
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;"><b>Nominativo</b></label>
                                                    <asp:DropDownList ID="ddl_FPnominativo" class="w3-input w3-border" runat="server" Style="padding: 2px;" OnSelectedIndexChanged="visualizzaFP" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="w3-half" style="padding: 5px">
                                                    <label style="margin-bottom: 0.2rem;">Telefono</label><br />
                                                    <asp:TextBox ID="txt_FPtelefono" class="w3-input w3-border w3-disabled" ReadOnly runat="server" Style="padding: 2px;"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="w3-row">
                                                <div class="w3-half" style="padding: 5px">
                                                    <div class="w3-threequarter" style="padding: 5px">
                                                        <label style="margin-bottom: 0.2rem;">Tipo pagamento</label>
                                                        <asp:DropDownList ID="ddl_FPtipoPagamento" class="w3-input w3-border" runat="server" Style="padding: 2px;"></asp:DropDownList>
                                                    </div>
                                                    <div class="w3-quarter" style="padding: 5px">
                                                        <label style="margin-bottom: 0.2rem;">Mod.Costo</label>
                                                        <asp:CheckBox ID="chk_ModCosto" runat="server" Style="padding: 2px;"></asp:CheckBox>
                                                    </div>
                                                </div>
                                                <div class="w3-half" style="padding: 5px">
                                                    <div class="w3-half" style="padding: 5px">
                                                        <label style="margin-bottom: 0.2rem;">Netto</label>
                                                        <asp:TextBox ID="txt_FPnetto" runat="server" class="w3-input w3-border " placeholder="Netto" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                                    </div>
                                                    <div class="w3-half" style="padding: 5px">
                                                        <label style="margin-bottom: 0.2rem;">Lordo</label>
                                                        <asp:TextBox ID="txt_FPlordo" runat="server" class="w3-input w3-border " ReadOnly placeholder="Lordo" Style="padding: 2px;"></asp:TextBox>
                                                    </div>
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
                        <asp:GridView ID="gvFigProfessionali" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvFigProfessionali_RowCommand" DataMember="IdentificatoreOggetto">
                            <Columns>
                                <asp:BoundField DataField="Cognome" HeaderText="Nominativo" />
                                <asp:BoundField DataField="Citta" HeaderText="Località" />
                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                <asp:BoundField DataField="Netto" HeaderText="Netto" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="Lordo" HeaderText="Lordo" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="Nota" HeaderText="Nota" />
                                <asp:TemplateField HeaderText="Seleziona">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUp" runat="server" ImageUrl="/Images/arrow-up-icon.png" ToolTip="Sposta su" CommandName="moveUp" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDown" runat="server" ImageUrl="/Images/arrow-down-icon.png" ToolTip="Sposta giù" CommandName="moveDown" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" CommandName="elimina" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' OnClientClick="return confermaEliminazioneFigProf();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </asp:Panel>
                </div>

                <div class="w3-col" style="height: 15%; padding-left: 5px; margin-bottom: 20px;">
                    <div class="w3-quarter" style="padding: 5px">
                        <asp:Button ID="btnImporta" runat="server" Text="Importa" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnImporta_Click" OnClientClick="$('.loader').show();" Style="padding: 7px 10px" />
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="w3-quarter" style="padding: 5px">
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="w3-round" Style="padding: 2px; width: 100%;" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div id="panelModificaPianoEsterno" class="w3-modal " style="padding-top: 50px; position: fixed;" runat="server">
                    <asp:UpdatePanel ID="upModificaPianoEsterno" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <div class="w3-modal-content w3-card-4 round" style="position: relative; width: 80%; background-color: white; overflow: auto;">
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
                                                    <asp:TextBox ID="txt_orario" runat="server" class="w3-input w3-border time" placeholder="hh:mm" Style="padding: 2px;" ></asp:TextBox>
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
                                                <label style="margin-bottom: 0.2rem;">Diaria</label><br />
                                                <div class="w3-row"><asp:RadioButton id="diaria15" runat="server" GroupName="radioDiaria" style="margin:5px"/><asp:Label ID="val_diaria15" runat="server" Text="15€" /></div>
                                                <div class="w3-row"><asp:RadioButton id="diaria30" runat="server" GroupName="radioDiaria" style="margin:5px"/><asp:Label  ID="val_diaria30" runat="server" Text="30€" /></div>
                                                <div class="w3-row"><asp:RadioButton id="diariaLibera" runat="server" GroupName="radioDiaria" style="margin:5px; float:left"/><asp:TextBox ID="txt_diaria" runat="server" class="w3-input w3-border w3-disabled" Style="padding: 2px;width:100px"  onkeypress="return onlyNumbers();"></asp:TextBox></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />
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

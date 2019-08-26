<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>

<script>
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txt_FiltroGruppi.ClientID%>").val("");
            $("#<%=txt_FiltroGruppi.ClientID%>").keyup(function () {
                filterOfferta(2);
            });
        });
    });

    function confermaEliminazioneArticolo() {
        return confirm("Eliminare l'articolo corrente?");
    }

    function confermaEliminazioneTuttiArticoli() {
        return confirm("Eliminare tutti gli articoli inseriti?");
    }

    function filterOfferta(columnIndex) {
        var filterText = $("#<%=txt_FiltroGruppi.ClientID%>").val().toLowerCase();
        var cellText = "";

        $("#<%=gvGruppi.ClientID%> tr:has(td)").each(function () {
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

    function azzeraCampiRicerca() {
        $("#<%=txt_dataLavorazione_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_cliente_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_codiceLavoro_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_lavorazione_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_luogo_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_produzione_FiltroRecuperaOfferta.ClientID%>").val('');
        $("#<%=ddl_tipologia_FiltroRecuperaOfferta.ClientID%>").val('');
        return false;
    }
</script>

<asp:Panel runat="server" ID="panelOfferta" Style="height: 100%">

    <div class="w3-container w3-center w3-xlarge" style="height: 5%; margin-bottom: 10px">OFFERTA</div>

    <div class="w3-row" style="height: 55%; font-size: small;">
        <div class="w3-col" style="height: 80%">
            <asp:Panel runat="server" ID="panelArticoli" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
                <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Articoli selezionati</p>
                <asp:Label ID="lbl_selezionareArticolo" runat="server" Text="Selezionare un articolo dalla lista" Style="position: absolute; top: 45%; left: 25%; font-size: large; color: cornflowerblue" />
                <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvArticoli_RowCommand" DataMember="IdentificatoreOggetto">
                    <Columns>
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                        <asp:BoundField DataField="Quantita" HeaderText="Q.tà" />
                        <asp:BoundField DataField="Prezzo" HeaderText="Listino" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Iva" HeaderText="Iva" />
                        <asp:BoundField DataField="Stampa" HeaderText="Stampa" />
                        <asp:TemplateField HeaderText="Seleziona">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgUp" runat="server" ImageUrl="/Images/arrow-up-icon.png" ToolTip="Sposta su" CommandName="moveUp" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                <asp:ImageButton ID="imgDown" runat="server" ImageUrl="/Images/arrow-down-icon.png" ToolTip="Sposta giù" CommandName="moveDown" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
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

    <div style="width: 99%; text-align: center; height: 5%; margin-bottom: 20px;">
        <asp:Button ID="btnEliminaArticoli" runat="server" Text="Elimina tutti gli articoli" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnEliminaArticoli_Click" Style="font-size: smaller; padding: 4px 8px" OnClientClick="return confermaEliminazioneTuttiArticoli();" />
        <asp:Button ID="btnRecuperaOfferta" runat="server" Text="Recupera offerta" class="w3-btn w3-white w3-border w3-border-orange w3-round-large" OnClick="btnRecuperaOfferta_Click" Style="font-size: smaller; padding: 4px 8px" />
    </div>


    <div class="w3-col" style="height: 25%; position: relative;">
        <asp:Panel runat="server" ID="panelGruppi" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
            <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Articoli e Articoli composti</p>
            <asp:GridView ID="gvGruppi" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvGruppi_RowCommand" DataMember="ID">
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
            <asp:TextBox ID="txt_FiltroGruppi" runat="server" CssClass="w3-round" placeholder="Cerca.." Style="width: 100%; padding: 5px; margin-top: 10px;"></asp:TextBox>
        </div>
    </div>




    <div id="panelModificaArticolo" class="w3-modal " style="position: fixed;" runat="server">
        <div class="w3-modal-content w3-card-4 w3-animate-top round" style="position: relative; width: 80%; background-color: white; overflow: auto;">
            <div class="w3-row-padding">

                <div class="w3-panel w3-blue w3-center w3-round">
                    <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Modifica Articolo</b> </h5>
                    <span onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                </div>

                <div class="w3-col round" style="padding: 5px;">
                    <div class="w3-half " style="padding: 5px;">

                        <div class="w3-col">
                            <label style="margin-bottom: 0.2rem;">Descrizione</label>
                            <asp:TextBox ID="txt_Descrizione" runat="server" class="w3-input w3-border  w3-round" MaxLength="60" placeholder="Descrizione" Style="padding: 2px;"></asp:TextBox>
                        </div>

                        <div class="w3-col">
                            <div class="w3-half">

                                <div class="w3-third" style="padding: 5px">
                                    <label style="margin-bottom: 0.2rem;">Prezzo</label>
                                    <asp:TextBox ID="txt_Prezzo" runat="server" class="w3-input w3-border  w3-round" placeholder="Prezzo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                </div>
                                <div class="w3-third" style="padding: 5px">
                                    <label style="margin-bottom: 0.2rem;">Costo</label>
                                    <asp:TextBox ID="txt_Costo" runat="server" class="w3-input w3-border  w3-round" placeholder="Costo" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                </div>
                                <div class="w3-third" style="padding: 5px">
                                    <label style="margin-bottom: 0.2rem;">Quantità</label>
                                    <asp:TextBox ID="txt_Quantita" runat="server" class="w3-input w3-border  w3-round" placeholder="Quantità" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                </div>
                            </div>
                            <div class="w3-half">
                                <div class="w3-third" style="padding: 5px">
                                    &nbsp;
                                </div>

                                <div class="w3-third" style="padding: 5px">
                                    <label style="margin-bottom: 0.2rem;">Iva</label>
                                    <asp:TextBox ID="txt_Iva" runat="server" class="w3-input w3-border w3-round" placeholder="iva" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
                                </div>
                                <div class="w3-third" style="padding: 5px">
                                    <label style="margin-bottom: 0.2rem;">Stampa</label><br />
                                    <asp:DropDownList ID="ddl_Stampa" runat="server" class=" w3-border w3-round">
                                        <asp:ListItem Value="1" Text="SI" />
                                        <asp:ListItem Value="0" Text="NO" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="w3-half" style="padding: 5px">
                        <label style="margin-bottom: 0.2rem;">Descrizione lunga</label>
                        <asp:TextBox ID="txt_DescrizioneLunga" runat="server" Rows="5" TextMode="MultiLine" class="w3-input w3-border w3-round" MaxLength="100" placeholder="Descrizione lunga" Style="padding: 2px;"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="w3-center" style="margin: 10px">
                <asp:Button ID="btnOK" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOK_Click" />
                <button onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
            </div>
        </div>
    </div>

<!-- RECUPERA OFFERTA -->
    <div id="panelRecuperaOfferta" class="w3-modal " style="position: fixed;" runat="server">
        <asp:UpdatePanel ID="upRecuperaOfferta" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="w3-modal-content w3-card-4 w3-animate-top round" style="width: 75%;min-height:350px; position: relative; background-color: white; overflow: auto;">
                    <div class="w3-row-padding">
                        <div class="w3-panel w3-blue w3-center w3-round">
                            <h5 class="w3-text-white" style="text-shadow: 1px 1px 0 #444"><b>Recupera offerta</b> </h5>
                            <span onclick="document.getElementById('<%= panelRecuperaOfferta.ClientID%>').style.display='none'" style="padding: 0px; top: 0px; margin-top: 16px; margin-right: 16px;" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Chiudi">&times;</span>
                        </div>
                        <div class="w3-col round" style="padding: 5px;">
                            <div class="w3-row " style="background-color: #cccccc;">
                                <div class="w3-row">
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Data lavorazione</label>
                                        <asp:TextBox ID="txt_dataLavorazione_FiltroRecuperaOfferta" runat="server" placeholder="Data lavorazione" class="calendar w3-input w3-border"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Tipologia</label>
                                        <asp:DropDownList ID="ddl_tipologia_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Cliente</label>
                                        <asp:DropDownList ID="ddl_cliente_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Codice lavoro</label>
                                        <asp:DropDownList ID="ddl_codiceLavoro_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="w3-row">
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Produzione</label>
                                        <asp:DropDownList ID="ddl_produzione_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Lavorazione</label>
                                        <asp:DropDownList ID="ddl_lavorazione_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter" style="padding: 5px">
                                        <label style="margin-bottom: 0.2rem;">Luogo</label>
                                        <asp:DropDownList ID="ddl_luogo_FiltroRecuperaOfferta" runat="server" class="w3-input w3-border w3-round"></asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter w3-center" style="padding: 5px; margin-top: 27px;">
                                        <asp:Button ID="btn_cerca_FiltroRecuperaOfferta" runat="server" Text="Cerca" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="padding: 4px 8px" OnClick="btn_cerca_FiltroRecuperaOfferta_Click" />&nbsp;&nbsp;
                                    <span id="btn_PulisciCampiRicerca_FiltroRecuperaOfferta" class="w3-btn w3-circle w3-red" onclick="return azzeraCampiRicerca();" title="Azzera campi di ricerca">&times;</span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <asp:GridView ID="gv_OfferteRecuperate" runat="server" AutoGenerateColumns="False"
                            Style="font-size: 8pt; max-height: 150px; width: 100%; position: relative; background-color: #FFF; text-align: center;top: 20px; max-height: 220px; overflow: auto"
                            HeaderStyle-BackColor="#2196F3" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" BorderWidth="0"
                            GridLines="None" AllowPaging="True" PageSize="5" OnPageIndexChanging="gv_OfferteRecuperate_PageIndexChanging"
                            OnRowCommand="gv_OfferteRecuperate_RowCommand" OnRowCreated="gv_OfferteRecuperate_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="data_inizio_lavorazione" HeaderText="Inizio lavor." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="data_fine_lavorazione" HeaderText="Fine lavor." HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="DecodificaTipologia" HeaderText="Tipologia" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DecodificaCliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="24%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="produzione" HeaderText="Produzione" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="lavorazione" HeaderText="Lavorazione" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="luogo" HeaderText="Luogo" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="codice_lavoro" HeaderText="Cod. lavoro" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_SelezionaOfferta" runat="server" ImageUrl="/Images/add.png" ToolTip="Seleziona l'offerta corrente" CommandName="seleziona" CommandArgument='<%#Eval("id")%>' Height="15" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <br />
                        <div class="w3-col" style="margin-top:80px;margin-bottom: 10px;" >
                            <div class="w3-center">
                                <%--<asp:Button ID="btnOK_recuperaOfferta" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" />--%>
                                <button onclick="document.getElementById('<%= panelRecuperaOfferta.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



</asp:Panel>

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
                                    <label style="margin-bottom: 0.2rem;">Quantità</label>

                                    <asp:TextBox ID="txt_Quantita" runat="server" class="w3-input w3-border" placeholder="Quantità" Style="padding: 2px;" onkeypress="return onlyNumbers();"></asp:TextBox>
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
            <div class="w3-center" style="margin: 10px">
                <asp:Button ID="btnOK" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" OnClick="btnOK_Click" />
                <button onclick="document.getElementById('<%= panelModificaArticolo.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
            </div>
        </div>
    </div>

    <div id="panelRecuperaOfferta" class="w3-modal " style="position: fixed;" runat="server">
        <div class="w3-modal-content w3-card-4 w3-animate-top round" style="max-width: 800px; width: 60%; height: 220px; position: relative; background-color: white; overflow: auto; font-size: 0.8em;">

            <div class="w3-center">
                <br>
                <span onclick="document.getElementById('<%= panelRecuperaOfferta.ClientID%>').style.display='none'" class="w3-button w3-xlarge w3-hover-red w3-display-topright" title="Close Modal">&times;</span>
            </div>

            <div class="w3-center">
                <h3>Recupera Offerta</h3>
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div class="w3-center" style="margin: 10px">
                <asp:Button ID="btnOK_recuperaOfferta" runat="server" Text="Cerca" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" Style="font-size: smaller; padding: 4px 8px" />
                <button onclick="document.getElementById('<%= panelRecuperaOfferta.ClientID%>').style.display='none'" type="button" class=" w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size: smaller; padding: 4px 8px">Annulla</button>
            </div>
        </div>
    </div>



</asp:Panel>

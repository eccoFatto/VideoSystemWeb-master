<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Lavorazione.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Lavorazione" %>


<script>

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

        //var statoCorrente = $("#< % = val_Stato.ClientID %>").text();

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
                        <asp:GridView ID="gvArticoliLavorazione" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvArticoli_RowCommand" DataMember="IdentificatoreOggetto">
                            <Columns>
                                <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
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

                <div class="w3-half">

                    <asp:TextBox ID="txt_FiltroGruppi" runat="server" CssClass="w3-round" placeholder="Cerca.." Style="width: 100%; padding: 5px; margin-top: 10px;"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    spazio1
        
                </div>
                <div class="w3-quarter">
                    <asp:Button ID="btn_SwitchArtPers" runat="server" Text="Inserimento personale/fornitore" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btn_SwitchArtPers_Click" Style="padding: 7px 10px" />
                </div>
            </div>
        </div>






        <div id="PianoEsterno" class="w3-container w3-border tabLavorazione w3-padding-small" style="height: 90%; overflow: auto; display: none">
            <div class="w3-row" style="height: 60%; font-size: small;">
                <div class="w3-col" style="height: 80%">
                    <asp:Panel runat="server" ID="panel1" CssClass="round" Style="height: 100%; position: relative; background-color: white; overflow: auto;">
                        <p style="text-align: center; font-weight: bold; font-size: medium; margin-bottom: 2px;">Figure professionali</p>
                        <asp:Label ID="Label1" runat="server" Text="Selezionare un articolo dalla lista" Style="position: absolute; top: 45%; left: 38%; font-size: large; color: cornflowerblue" />
                        <asp:GridView ID="gvFigProfessionali" runat="server" AutoGenerateColumns="False" Style="font-size: 8pt; width: 100%; position: relative; background-color: #EEF1F7; text-align: center" OnRowCommand="gvArticoli_RowCommand" DataMember="IdentificatoreOggetto">
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
            </div>
        </div>

        <div class="w3-bar w3-blue w3-round">
            <div class="w3-row">
                <div class="w3-col" style="width: 100%">
                    <div class="w3-third w3-center tablinkLav w3-red" style="padding:3px; cursor:pointer" runat="server" id="tab_Lavorazione" onclick="openTabEventoLavorazione(event, 'Lavoraz')">Lavorazione</div>
                    <div class="w3-third w3-center tablinkLav" style="padding:3px; cursor:pointer" runat="server" id="tab_DettEconomico" onclick="openTabEventoLavorazione(event, 'DettEconomico')">Dettaglio economico</div>
                    <div class="w3-third w3-center tablinkLav" style="padding:3px; cursor:pointer;" runat="server" id="tab_PianoEsterno" onclick="openTabEventoLavorazione(event, 'PianoEsterno')">Piano esterno</div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>

<script>
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txt_FiltroGruppi.ClientID%>").val("");
            $("#<%=txt_FiltroGruppi.ClientID%>").keyup(function () {
                filter(1);
            });
        });
    });

    function confermaEliminazioneArticolo() {
        return confirm("Eliminare l'articolo corrente?");
    }

    function filter(columnIndex) {
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

<asp:Panel runat="server" ID="panelOfferta">

    <div class="w3-container w3-center w3-xlarge">OFFERTA</div>

    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    <div class="w3-row-padding" style="font-size: small;">
        <div class="w3-half" style="padding-right:5px">
            <asp:Panel runat="server" ID="panelGruppi" CssClass="round" Style="height: 200px; position: relative; background-color: white; overflow: auto;">
                <asp:TextBox ID="txt_FiltroGruppi" runat="server" CssClass="w3-round"  placeholder="Filtra i risultati" Style="width:100%;padding:5px"></asp:TextBox>
                <asp:GridView ID="gvGruppi" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;text-align:center" OnRowCommand="gvGruppi_RowCommand" DataMember="ID">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:BoundField DataField="nome" HeaderText="Nome" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                        <asp:TemplateField HeaderText="Seleziona">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgOk" runat="server" ImageUrl="/Images/add.png" ToolTip="Aggiungi" CommandName="aggiungi" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <div class="w3-half" tyle="padding-left:5px">
            <asp:Panel runat="server" ID="panelArticoli"  CssClass="round" Style="height: 200px; position: relative; background-color: white; overflow: auto;">
                <asp:Label ID="lbl_selezionareArticolo" runat="server" Text="Selezionare un articolo dalla lista" Style="position: absolute; top: 45%; left: 25%; font-size: large; color: cornflowerblue" />
                <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;text-align:center" OnRowCommand="gvArticoli_RowCommand" DataMember="IdentificatoreOggetto">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="Codice" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                        <asp:BoundField DataField="Prezzo" HeaderText="Prezzo" />
                        <asp:BoundField DataField="Costo" HeaderText="Costo" />
                        <asp:BoundField DataField="Iva" HeaderText="Iva" />
                        <asp:BoundField DataField="Stampa" HeaderText="Stampa" />
                        <asp:TemplateField HeaderText="Seleziona">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' />
                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" CommandName="elimina" CommandArgument='<%#Eval("id") + "," + Eval("IdentificatoreOggetto") %>' OnClientClick="return confermaEliminazioneArticolo();" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </asp:Panel>
        </div>


    </div>
    <br />


    <asp:Panel runat="server" class="round" ID="panelModificaArticolo" Style="width: 99%; height: 150px; position: relative; background-color: white; overflow: auto; font-size: 0.8em; display: none">

        <div class="w3-row-padding">
            <div class="w3-col">
                <div class="w3-third" style="padding:5px;">
                    <label style="margin-bottom:0.2rem;">Descrizione</label>
                    <asp:TextBox ID="txt_Descrizione" runat="server" class="w3-input w3-border" MaxLength="60" placeholder="Descrizione" Style="padding: 2px;"></asp:TextBox>
                </div>
                <div class="w3-twothird" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Descrizione lunga</label>
                    <asp:TextBox ID="txt_DescrizioneLunga" runat="server" class="w3-input w3-border" MaxLength="60" placeholder="Descrizione lunga" Style="padding: 2px;"></asp:TextBox>
                </div>

            </div>
            <div class="w3-twothird">
                <div class="w3-third" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Genere</label><br />
                    <asp:DropDownList ID="ddl_Genere" runat="server">
                        <asp:ListItem Value="1" Text="Vari" Selected="True" />
                    </asp:DropDownList>
                </div>
                <div class="w3-third" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Gruppo</label><br />
                    <asp:DropDownList ID="ddl_Gruppo" runat="server">
                        <asp:ListItem Value="1" Text="Vari" Selected="True" />
                    </asp:DropDownList>
                </div>
                <div class="w3-third" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Sottogruppo</label><br />
                    <asp:DropDownList ID="ddl_Sottogruppo" runat="server">
                        <asp:ListItem Value="1" Text="Vari" Selected="True" />
                    </asp:DropDownList>
                </div>

            </div>
            <div class="w3-third">
                <div class="w3-quarter" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Prezzo</label>
                    <asp:TextBox ID="txt_Prezzo" runat="server" class="w3-input w3-border" placeholder="Prezzo" Style="padding: 2px;"></asp:TextBox>
                </div>
                <div class="w3-quarter" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Costo</label>
                    <asp:TextBox ID="txt_Costo" runat="server" class="w3-input w3-border" placeholder="Costo" Style="padding: 2px;"></asp:TextBox>
                </div>
                <div class="w3-quarter" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Iva</label>
                    <asp:TextBox ID="txt_Iva" runat="server" class="w3-input w3-border" placeholder="iva" Style="padding: 2px;"></asp:TextBox>
                </div>
                <div class="w3-quarter" style="padding:5px">
                    <label style="margin-bottom:0.2rem;">Stampa</label><br />
                    <asp:DropDownList ID="ddl_Stampa" runat="server">
                        <asp:ListItem Value="1" Text="SI" />
                        <asp:ListItem Value="0" Text="NO" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="w3-center" style="margin: 10px">
            <asp:Button ID="btnOK" runat="server" Text="OK" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" style="font-size:smaller; padding:4px 8px" OnClick="btnOK_Click" />
            <asp:Button ID="btnAnnullaModifiche" runat="server" Text="Annulla" class="w3-btn w3-white w3-border w3-border-red w3-round-large" style="font-size:smaller; padding:4px 8px" OnClick="btnAnnullaModifiche_Click" />
        </div>

    </asp:Panel>

</asp:Panel>

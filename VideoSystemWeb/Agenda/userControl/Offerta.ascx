<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>



<asp:Panel runat="server" ID="panelAppuntamenti">




    <div class="w3-container w3-center w3-xlarge">OFFERTA</div>

    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    <div>
        <asp:Panel runat="server" class="round" ID="panelArticoli" Style="width: 99%; height: 200px; position: relative; background-color: white; overflow: auto">
            <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7; width: 98%" DataMember="id">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Codice" />
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                    <asp:BoundField DataField="Prezzo" HeaderText="Prezzo" />
                    <asp:BoundField DataField="Costo" HeaderText="Costo" />
                    <asp:BoundField DataField="Iva" HeaderText="Iva" />
                    <asp:BoundField DataField="Stampa" HeaderText="Stampa" />
                    <asp:TemplateField HeaderText="Seleziona">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%# Eval("ArtArticoli.id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </asp:Panel>
    </div>
    <br />
    <br />
    <div>
        <asp:Panel runat="server" class="round" ID="panelGruppi" Style="width: 50%; height: 150px; position: relative; background-color: white; overflow: auto;">
            <asp:GridView ID="gvGruppi" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7; width: 98%" OnRowCommand="gvGruppi_RowCommand" DataMember="ID">
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

    <div class="w3-col m12">
        <div class="w3-center w3-margin">
            <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click" Visible="false" />
            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnAnnulla_Click" Visible="false" />
        </div>
    </div>
</asp:Panel>

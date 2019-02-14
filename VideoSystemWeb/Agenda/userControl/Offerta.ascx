<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>


    


<asp:Panel runat="server" ID="panelAppuntamenti" >
    



    <div class="w3-container w3-center w3-xlarge">OFFERTA</div>

    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    <div>
        <asp:Panel runat="server" ID="panel1" style="width:90%; height: 200px; position:relative ;background-color:white;">
            <asp:GridView ID="gvSelezioneOfferta" runat="server" AutoGenerateColumns="False" style="width:98%" DataMember="ID">
                <Columns>
                    <asp:BoundField DataField="Codice" HeaderText="Codice tipo"/>
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                    <asp:BoundField DataField="Misura" HeaderText="Misura" />
                    <asp:BoundField DataField="Quantita" HeaderText="Quantità" />
                    <asp:BoundField DataField="Prezzo" HeaderText="Prezzo" />
                    <asp:BoundField DataField="Costo" HeaderText="Costo" />

                </Columns>

            </asp:GridView>

        </asp:Panel>
    </div>
    <br /><br />
    <div>
        <asp:Panel runat="server" ID="panel2" style="width:40%; height: 150px; position:relative ;background-color:white;">
            <asp:GridView ID="gvElencoOfferta" runat="server" AutoGenerateColumns="False" style="width:98%" OnRowCommand="gvElencoOfferta_RowCommand" DataMember="ID">
                <Columns>
                    <asp:BoundField DataField="Codice" HeaderText="Codice tipo" />
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                    <asp:BoundField DataField="Misura" HeaderText="Misura" />
                    <asp:TemplateField HeaderText="Seleziona">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgOk" runat="server" ImageUrl="/Images/add.png" ToolTip="Aggiungi" CommandName="aggiungi" CommandArgument='<%# Eval("ID") %>'/>
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
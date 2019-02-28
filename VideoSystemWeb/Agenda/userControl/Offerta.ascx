<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offerta.ascx.cs" Inherits="VideoSystemWeb.Agenda.userControl.Offerta" %>



<asp:Panel runat="server" ID="panelAppuntamenti">




    <div class="w3-container w3-center w3-xlarge">OFFERTA</div>

    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
    </div>

    <div class="w3-row-padding" style="font-size:small;">
        <asp:Panel runat="server" class="w3-half round" ID="panelGruppi" Style="width: 50%; height: 200px; position: relative; background-color: white; overflow: auto; ">
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


        <asp:Panel runat="server" class="w3-half round" ID="panelArticoli" Style="height: 200px; position: relative; background-color: white; overflow: auto; ">
            <asp:Label ID="lbl_selezionareArticolo" runat="server" Text="Selezionare un articolo dalla lista" Style="position: absolute; top: 45%; left: 25%; font-size: large; color: cornflowerblue" />
            <asp:GridView ID="gvArticoli" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7; width: 98%" OnRowCommand="gvArticoli_RowCommand" DataMember="IdentificatoreOggetto">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Codice" />
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                    <asp:BoundField DataField="Prezzo" HeaderText="Prezzo" />
                    <asp:BoundField DataField="Costo" HeaderText="Costo" />
                    <asp:BoundField DataField="Iva" HeaderText="Iva" />
                    <asp:BoundField DataField="Stampa" HeaderText="Stampa" />
                    <asp:TemplateField HeaderText="Seleziona">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" CommandName="modifica" CommandArgument='<%# Eval("IdentificatoreOggetto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </asp:Panel>



    </div>
    <br />


    <asp:Panel runat="server" class="round" ID="panelModificaArticolo" Style="width: 99%; height: 150px; position: relative; background-color: white; overflow: auto;font-size:smaller;display:none" >

        <div class="w3-row-padding">
            <div class="w3-third">
                <div class="w3-third">
                    <label>Descrizione</label>
                    <asp:TextBox ID="txt_Descrizione" runat="server" class="w3-input w3-border" placeholder="Descrizione" style="padding:0px;"></asp:TextBox>
                </div>
                <div class="w3-twothird">
                    <label>Descr. lunga</label>
                    <asp:TextBox ID="txt_DescrizioneLunga" runat="server" class="w3-input w3-border" placeholder="Descrizione lunga" style="padding:0px;"></asp:TextBox>
                </div>
                
            </div>
            <div class="w3-third">
                <div class="w3-third">
                    <label>Genere</label><br />
                    <asp:DropDownList ID="ddl_Genere" runat="server" />
                </div>
                <div class="w3-third">
                    <label>Gruppo</label><br />
                    <asp:DropDownList ID="ddl_Gruppo" runat="server" />
                </div>
                <div class="w3-third">
                    <label>Sottogruppo</label><br />
                    <asp:DropDownList ID="ddl_Sottogruppo" runat="server" />
                </div>
               
            </div>
            <div class="w3-third">
                 <div class="w3-quarter">
                    <label>Prezzo</label>
                    <asp:TextBox ID="txt_Prezzo" runat="server" class="w3-input w3-border" placeholder="Prezzo" style="padding:0px;"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Costo</label>
                    <asp:TextBox ID="txt_Costo" runat="server" class="w3-input w3-border" placeholder="Costo" style="padding:0px;"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Iva</label>
                    <asp:TextBox ID="txt_Iva" runat="server" class="w3-input w3-border" placeholder="iva" style="padding:0px;"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Stampa</label><br />
                    <asp:DropDownList ID="ddl_Stampa" runat="server" >
                        <asp:ListItem Value="1" Text="SI" />
                        <asp:ListItem Value="0" Text="NO" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="w3-center w3-margin">
            <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click"  />
            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnAnnulla_Click" />
        </div>

    </asp:Panel>

</asp:Panel>

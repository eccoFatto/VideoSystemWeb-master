<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Magazzino.aspx.cs" Inherits="VideoSystemWeb.MAGAZZINO.Magazzino" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();
        });
    </script>

    <label><asp:Label ID="lblMagazzino" runat="server" Text="MAGAZZINO" ForeColor="SteelBlue"></asp:Label></label>

    <asp:UpdatePanel ID="UpdatePanelMagazzino" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label style="font-weight:bold">Cliente</label>
                    <asp:label ID="lbl_Cliente" runat="server"></asp:label>
                </div>
                <div class="w3-threequarter">
                    <div class="w3-half">
                        <label style="font-weight:bold">Lavorazione</label>
                        <asp:label ID="lbl_Lavorazione" runat="server"></asp:label>
                    </div>
                    <div class="w3-half">
                        <label style="font-weight:bold">Produzione</label>
                        <asp:label ID="lbl_Produzione" runat="server"></asp:label>
                    </div>
                </div>
                
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label style="font-weight:bold">Tipologia</label>
                    <asp:label ID="lbl_Tipologia" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Data inizio lavorazione</label>
                    <asp:label ID="lbl_DataInizio" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Data fine lavorazione</label>
                    <asp:label ID="lbl_DataFine" runat="server"></asp:label>   
                </div>
                <div class="w3-quarter">
                    <label style="font-weight:bold">Codice lavoro</label>
                    <asp:label ID="lbl_CodLavoro" runat="server"></asp:label>
                </div>
            </div>
            <br />
            <asp:Button ID="btnInserisciRiga" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnInserisciRiga_Click" OnClientClick="$('.loader').show();" Text="Inserisci Riga" />
            <br />
            <br />
            <div class="round">
                <asp:GridView ID="gv_attrezzature" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_attrezzature_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_attrezzature_PageIndexChanging" PageSize="20" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                    <Columns>
<%--                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Attrezzatura" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div class="w3-row-padding">
                <asp:TextBox ID="txt_Note" runat="server" Rows="5" TextMode="MultiLine" width="100%"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="GestioneRaggruppamentiArticoli.aspx.cs" Inherits="VideoSystemWeb.Articoli.GestioneRaggruppamentiArticoli" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script>
    $(document).ready(function () {
        $('.loader').hide();
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA RAGGRUPPAMENTO
    function mostraRaggruppamento(row) {
        $('.loader').show();
        $("#<%=hf_idRagg.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditRaggruppamento.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO RAGGRUPPAMENTO
    function inserisciRaggruppamento() {
        $('.loader').show();
        $("#<%=hf_idRagg.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsRaggruppamento.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO RAGGRUPPAMENTO
    function openDettaglioRaggruppamento(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            var i;
            var x = document.getElementsByClassName("ragg");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";  
            }
            document.getElementById(tipoName).style.display = "block";  
        }
    }

    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA RAGGRUPPAMENTO E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Raggruppamento');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        pannelloPopup.style.display = "none";
    }
</script>
<Label><asp:Label ID="lblRaggruppamenti" runat="server" Text="ARTICOLI COMPOSTI" ForeColor="Teal"></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <div class="w3-cell-row" style="width:100%">
            <div class="w3-container w3-cell w3-cell-middle">
                <div class="round" style="overflow:auto;height:400px;min-height:300px;">
                    <asp:GridView ID="gvMod_Raggruppamenti" AutoGenerateColumns="true" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gvMod_Raggruppamenti_RowDataBound"  >
                        <Columns>
                            <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgSeleziona" runat="server" CausesValidation="false"  Text="Seleziona" ImageUrl="~/Images/detail-icon.png" ToolTip="Seleziona Raggruppamento" ImageAlign="AbsMiddle" Height="30px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="w3-container w3-cell w3-cell-top">
                <div id="divBtnInserisciRaggruppamento" runat="server"> 
                    <div id="insRaggruppamento" class="w3-btn w3-border w3-green w3-round w3-margin" onclick="inserisciRaggruppamento();" style="width:100px">Inserisci</div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>

<asp:Button runat="server" ID="btnEditRaggruppamento" Style="display: none" OnClick="btnEditRaggruppamento_Click"/>
<asp:Button runat="server" ID="btnInsRaggruppamento" Style="display: none" OnClick="btnInsRaggruppamento_Click"/>

<asp:HiddenField ID="hf_idRagg" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Raggruppamento" />


<asp:UpdatePanel ID="upColl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-row-padding w3-margin w3-center w3-large w3-yellow w3-round" >
                    <div class="w3-half">
                        GESTIONE ARTICOLI COMPOSTI
                    </div>
                    <div class="w3-half">
                        <%=dettaglioModifica%>
                    </div>
                </div> 
                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI COLLABORATORE -->
                    <div class="w3-bar w3-yellow w3-round">
                        <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioRaggruppamento('Raggruppamento')">Articoli Composti</div>
                        <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioRaggruppamento('Articoli')">Articoli Associati</div>
                        <div class="w3-bar-item w3-button w3-yellow w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
                    </div>
                </div>
                    <!-- TAB RAGGRUPPAMENTI -->
                    <div id="Raggruppamento" class="w3-container w3-border ragg" style="display:block" >
                        <label>Articoli Composti</label>
                        <div class="w3-container w3-center">
                            <p>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-half">
                                        <label>Nome Articolo Composto</label>
                                        <asp:TextBox ID="tbInsNomeRaggruppamento" runat="server" MaxLength="50" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-half">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbInsDescrizioneRaggruppamento" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID ="tbIdRaggruppamentoDaModificare"  runat="server" Visible ="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnInserisciRaggruppamento" runat="server" Text="Inserisci Ragruppamento" class="w3-panel w3-green w3-border w3-round" OnClick="btnConfermaInserimentoRaggruppamento_Click" OnClientClick="return confirm('Confermi inserimento Raggruppamento?')" />
                                    <asp:Button ID="btnModificaRaggruppamento" runat="server" Text="Modifica Raggruppamento" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaRaggruppamento_Click" OnClientClick="return confirm('Confermi modifica Raggruppamento?')" Visible="false" />
                                    <asp:Button ID="btnEliminaRaggruppamento" runat="server" Text="Elimina Raggruppamento" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaRaggruppamento_Click" OnClientClick="return confirm('Confermi eliminazione Raggruppamento?')" Visible="false" />
                                </div>
                            </p>
                        </div>
                    </div>
                <!-- TAB ARTICOLI -->
                    <div id="Articoli" class="w3-container w3-border ragg" style="display:none">
                        <label>Articoli Associati</label>
                        <div class="round">
                            <asp:GridView ID="gvMod_Articoli" AutoGenerateColumns="false" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" HeaderText="Sel." HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgElimina" runat="server" CausesValidation="false"  Text="Elimina" ImageUrl="~/Images/delete.png" ToolTip="Elimina Articolo" ImageAlign="AbsMiddle" Height="30px" CommandName="EliminaArticolo" CommandArgument='<%#Eval("id")%>' OnCommand="imgElimina_Command" OnClientClick="return confirm('Confermi eliminazione articolo?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                                    <asp:BoundField DataField="Desc. Lunga" HeaderText="Desc. Lunga" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="w3-container w3-center">
                            <asp:Button ID="btnApriArticoli" runat="server" OnClick="btnApriArticoli_Click" Text="Gestione Articoli" class="w3-panel w3-green w3-border w3-round" />
                            <asp:PlaceHolder ID="phArticoli" runat="server" Visible="false">                                
                                <div class="w3-row-padding w3-center w3-text-center" >
                                    <div class="w3-threequarter">
                                        <label>Selezione Gruppi da aggiungere</label>
                                        <asp:DropDownList ID="ddlArticoliDaAggiungere" runat="server" AutoPostBack="false" Width="100%" class="w3-input w3-border">
                                        </asp:DropDownList>  
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Qta</label>
                                        <asp:TextBox ID="tbQtaArticoliDaAggiungere" class="w3-input w3-border" runat="server" MaxLength="2" Text="1"></asp:TextBox>
                                        <%--<ajaxToolkit:NumericUpDownExtender ID="nudeArticoli" Width="200" runat="server" Minimum="1" Maximum="20" Step="1" TargetControlID="tbQtaArticoliDaAggiungere" ></ajaxToolkit:NumericUpDownExtender>--%>
                                    </div>
                                </div>
                                <asp:Button ID="btnInserisciArticolo" runat="server" Text="Inserisci Articolo" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciArticolo_Click" OnClientClick="return confirm('Confermi inserimento Articolo?')" />
                                <asp:Button ID="btnEliminaArticolo" runat="server" Text="Elimina Articolo" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaArticolo_Click" OnClientClick="return confirm('Confermi eliminazione Articolo?')" />
                            </asp:PlaceHolder>
                        </div>
                    </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

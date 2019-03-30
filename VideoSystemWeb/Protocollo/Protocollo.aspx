<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Protocollo.aspx.cs" Inherits="VideoSystemWeb.Protocollo.Protocollo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
    $(document).ready(function () {
        $('.loader').hide();
    });

    // APRO POPUP VISUALIZZAZIONE/MODIFICA PROTOCOLLO
        function mostraProtocollo(row) {
        $('.loader').show();
        $("#<%=hf_idProt.ClientID%>").val(row);
        $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
        $("#<%=btnEditProtocollo.ClientID%>").click();
    }
    // APRO POPUP DI INSERIMENTO PROTOCOLLO
    function inserisciProtocollo() {
        $('.loader').show();
        $("#<%=hf_idProt.ClientID%>").val('');
        $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
        $("#<%=btnInsProtocollo.ClientID%>").click();
    }

    // APRO LE TAB DETTAGLIO PROTOCOLLO
    function openDettaglioProtocollo(tipoName) {
        $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
        if (document.getElementById(tipoName) != undefined) {
            var i;
            var x = document.getElementsByClassName("prot");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";  
            }
            document.getElementById(tipoName).style.display = "block";  
        }
    }

    function chiudiPopup() {
        // QUANDO APRO IL POPUP RIPARTE SEMPRE DA PROTOCOLLO E NON DALL'ULTIMA TAB APERTA
        $("#<%=hf_tabChiamata.ClientID%>").val('Protocollo');
        var pannelloPopup = document.getElementById('<%=pnlContainer.ClientID%>');
        pannelloPopup.style.display = "none";
    }

    // AZZERO TUTTI I CAMPI RICERCA
    function azzeraCampiRicerca() {
        $("#<%=tbCodiceLavoro.ClientID%>").val('');
        $("#<%=tbRagioneSociale.ClientID%>").val('');
        $("#<%=tbNumeroProtocollo.ClientID%>").val('');
        $("#<%=tbProtocolloRiferimento.ClientID%>").val('');
        $("#<%=ddlTipoProtocollo.ClientID%>").val('');
    }
</script>
<Label><asp:Label ID="lblProtocolli" runat="server" Text="PROTOCOLLI" ForeColor="Teal"></asp:Label></Label>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <div class="w3-row-padding">
            <div class="w3-quarter">
                <label>Codice Lavoro</label>
                <asp:TextBox ID="tbCodiceLavoro" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Numero Protocollo</label>
                <asp:TextBox ID="tbNumeroProtocollo" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Protocollo Riferimento</label>
                <asp:TextBox ID="tbProtocolloRiferimento" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Tipo</label>
                <asp:DropDownList ID="ddlTipoProtocollo" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                </asp:DropDownList>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-threequarter">
                <label>Cliente</label>
                <asp:TextBox ID="tbRagioneSociale" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaProtocollo" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaProtocollo_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <div id="divBtnInserisciProtocollo" runat="server"> 
                                <div id="clbtnInserisciProtocollo" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciProtocollo();">Inserisci</div>
                            </div>

                        </td>
                        <td style="width:20%;">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;"  OnClientClick="azzeraCampiRicerca();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>   

        <div class="round">
            <asp:GridView ID="gv_protocolli" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_protocolli_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_protocolli_PageIndexChanging" OnRowCommand="gv_protocolli_RowCommand" PageSize="20">
                <Columns>
<%--                    <asp:TemplateField HeaderText="Seleziona">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="/Images/edit.png" ToolTip="Modifica" />
                            <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/Images/delete.png" ToolTip="Elimina" />
                        </ItemTemplate>
                    </asp:TemplateField> --%>   
                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false"  Text="Vis." ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Protocollo" ImageAlign="AbsMiddle" Height="30px" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                    </asp:TemplateField>                       
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>

<asp:Button runat="server" ID="btnEditProtocollo" Style="display: none" OnClick="btnEditProtocollo_Click"/>
<asp:Button runat="server" ID="btnInsProtocollo" Style="display: none" OnClick="btnInsProtocollo_Click"/>

<asp:HiddenField ID="hf_idProt" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Protocollo" />

<asp:UpdatePanel ID="upColl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel  runat="server" ID="pnlContainer" visible="false">
            <div class="modalBackground"></div>
            <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                <div class="w3-container w3-center w3-xlarge">
                    GESTIONE PROTOCOLLI
                </div>
                <br />

                <div class="w3-container">
                    <!-- ELENCO TAB DETTAGLI PROTOCOLLO -->
                    <div class="w3-bar w3-yellow w3-round">
                        <div class="w3-bar-item w3-button w3-yellow" onclick="openDettaglioProtocollo('Protocollo')">Protocollo</div>
                        <div class="w3-bar-item w3-button w3-yellow w3-right">
                            <div id="btnChiudiPopup" class="w3-button w3-yellow w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                        </div>
                    </div>
                </div>
                    <!-- TAB PROTOCOLLI -->
                    <div id="Protocollo" class="w3-container w3-border prot" style="display:block" >
                        <label>Protocolli</label>
                        <div class="w3-container w3-center">
                            <p>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-quarter">
                                        <label>Codice Lavorazione</label>
                                        <asp:TextBox ID="tbMod_CodiceLavoro" runat="server" MaxLength="30" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Numero Protocollo</label>
                                        <asp:TextBox ID="tbMod_NumeroProtocollo" runat="server" MaxLength="30" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                        <asp:TextBox ID ="tbIdProtocolloDaModificare"  runat="server" Visible ="false"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Prot. Riferimento</label>
                                        <asp:TextBox ID="tbMod_ProtocolloRiferimento" runat="server" MaxLength="20" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Cliente</label>
                                        <asp:TextBox ID="tbMod_Cliente" runat="server" MaxLength="60" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-twothird">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbMod_Descrizione" runat="server" MaxLength="200" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-third">
                                        <label>Tipo</label>
                                        <asp:DropDownList ID="cmbMod_Tipologia" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-third">
                                        <label>Nome File</label>
                                        <asp:TextBox ID="tbMod_NomeFile" runat="server" MaxLength="100" class="w3-input w3-border" placeholder="" Text="" ></asp:TextBox>
                                    </div>
                                    <div class="w3-third" >
                                        <label>Seleziona File</label>
                                        <asp:FileUpload ID="fuFileProt" runat="server" Font-Size="X-Small" class="w3-input w3-border" style="vertical-align:central !important;"/>
                                    </div>
                                    <div class="w3-third" >
                                        <label>&nbsp;</label>
                                        <asp:Button ID="uploadButton" runat="server" class="w3-input w3-border w3-round w3-centered" Width="50%" OnClick="uploadButton_Click" Text="Carica File" />
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <asp:Label ID="lblImage" runat="server" Font-Size="Medium"></asp:Label>
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnGestisciProtocollo" runat="server" Text="Gestisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciProtocollo_Click" />
                                    <asp:Button ID="btnInserisciProtocollo" runat="server" Text="Inserisci Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciProtocollo_Click" OnClientClick="return confirm('Confermi inserimento Protocollo?')" />
                                    <asp:Button ID="btnModificaProtocollo" runat="server" Text="Modifica Protocollo" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaProtocollo_Click" OnClientClick="return confirm('Confermi modifica Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnEliminaProtocollo" runat="server" Text="Elimina Protocollo" class="w3-panel w3-green w3-border w3-round"  OnClick="btnEliminaProtocollo_Click" OnClientClick="return confirm('Confermi eliminazione Protocollo?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaProtocollo" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaProtocollo_Click" />
                                </div>
                            </p>
                        </div>
                    </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

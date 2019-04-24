<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneUtenti.aspx.cs" Inherits="VideoSystemWeb.Utente.GestioneUtenti" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaUtenti.ClientID%>").click();
                    }
                }
            });
        });

        // APRO POPUP VISUALIZZAZIONE/MODIFICA UTENTE
        function mostraUtente(row) {
            $('.loader').show();
            $("#<%=hf_idUtente.ClientID%>").val(row);
            $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
            $("#<%=btnEditUtente.ClientID%>").click();
        }
        // APRO POPUP DI INSERIMENTO UTENTE
        function inserisciUtente() {
            $('.loader').show();
            $("#<%=hf_idUtente.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
            $("#<%=btnInsUtente.ClientID%>").click();
        }

        // APRO LE TAB DETTAGLIO UTENTE
        function openDettaglioUtente(tipoName) {
            $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
            if (document.getElementById(tipoName) != undefined) {
                var i;
                var x = document.getElementsByClassName("utente");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(tipoName).style.display = "block";
            }
        }

        function chiudiPopup() {
            // QUANDO APRO IL POPUP RIPARTE SEMPRE DA UTENTE E NON DALL'ULTIMA TAB APERTA
            $("#<%=hf_tabChiamata.ClientID%>").val('Utente');
            $("#<%=hf_idUtente.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('VISUALIZZAZIONE');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();

        }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCognome.ClientID%>").val('');
            $("#<%=tbNome.ClientID%>").val('');
            $("#<%=tbUserName.ClientID%>").val('');
            $("#<%=ddlTipoUtente.ClientID%>").val('');
        }

    </script>
    <label>
    <asp:Label ID="lblUtenti" runat="server" Text="UTENTI" ForeColor="LightBlue"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Cognome</label>
                    <asp:TextBox ID="tbCognome" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Nome</label>
                    <asp:TextBox ID="tbNome" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Nome Utente</label>
                    <asp:TextBox ID="tbUserName" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Tipo Utenza</label>
                    <asp:DropDownList ID="ddlTipoUtente" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-half">
                    <label>&nbsp;</label>                
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>                
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Button ID="btnRicercaUtenti" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaUtente_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 40%;">
                                <div id="divBtnInserisciUtente" runat="server">
                                    <div id="clbtnInserisciUtente" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciUtente();" >Inserisci</div>
                                </div>
                            </td>
                            <td style="width: 20%;">
                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="round">
                <asp:GridView ID="gv_utenti" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_utenti_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_utenti_PageIndexChanging" PageSize="20">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Protocollo" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaUtenti" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button runat="server" ID="btnEditUtente" Style="display: none" OnClick="btnEditUtente_Click" />
    <asp:Button runat="server" ID="btnInsUtente" Style="display: none" OnClick="btnInsUtente_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopupServer_Click" />

    <asp:HiddenField ID="hf_idUtente" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Utente" />
   <asp:UpdatePanel ID="upUtenti" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE UTENTI
                    </div>
                    <br />

                    <div class="w3-container">
                        <!-- ELENCO TAB DETTAGLI UTENTE -->
                        <div class="w3-bar w3-light-blue w3-round">
                            <div class="w3-bar-item w3-button w3-light-blue" onclick="openDettaglioUtente('Utente')">Utente</div>
                            <div class="w3-bar-item w3-button w3-light-blue w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-light-blue w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                        </div>
                    </div>
                    <!-- TAB UTENTI -->
                    <div id="Utente" class="w3-container w3-border prot" style="display: block">
                        <div class="w3-container w3-center">
                            <p>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-quarter">
                                        <label>Cognome</label>
                                        <asp:TextBox ID="tbMod_Cognome" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Nome</label>
                                        <asp:TextBox ID="tbMod_Nome" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbIdUtenteDaModificare" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Nome Utente</label>
                                        <asp:TextBox ID="tbMod_Username" runat="server" MaxLength="50" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Tipo Utenza</label>
                                        <asp:DropDownList ID="cmbMod_TipoUtente" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-half">
                                        <label>E-mail</label>
                                        <asp:TextBox ID="tbMod_Email" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-half">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbMod_Descrizione" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnGestisciUtente" runat="server" Text="Gestisci Utente" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciUtente_Click" />
                                    <asp:Button ID="btnInserisciUtente" runat="server" Text="Inserisci Utente" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciUtente_Click" OnClientClick="return confirm('Confermi inserimento Utente?')" />
                                    <asp:Button ID="btnModificaUtente" runat="server" Text="Modifica Utente" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaUtente_Click" OnClientClick="return confirm('Confermi modifica Utente?')" Visible="false" />
                                    <asp:Button ID="btnEliminaUtente" runat="server" Text="Elimina Utente" class="w3-panel w3-green w3-border w3-round" OnClick="btnEliminaUtente_Click" OnClientClick="return confirm('Confermi eliminazione Utente?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaUtente" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaUtente_Click" />
                                </div>
                                <p>
                                </p>
                            </p>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnInserisciUtente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnModificaUtente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEliminaUtente" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attrezzature.aspx.cs" Inherits="VideoSystemWeb.MAGAZZINO.Attrezzature" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaAttrezzatura.ClientID%>").click();
                    }
                }
            });

            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
                });
            });
        });  

        // APRO POPUP VISUALIZZAZIONE/MODIFICA ATTREZZATURA
        function mostraAttrezzatura(row) {
            $('.loader').show();
            $("#<%=hf_idAttrezzatura.ClientID%>").val(row);
            $("#<%=hf_tipoOperazione.ClientID%>").val('MODIFICA');
            $("#<%=btnEditAttrezzatura.ClientID%>").click();
        }
        // APRO POPUP DI INSERIMENTO ATTREZZATURA
        function inserisciAttrezzatura() {
            $('.loader').show();
            $("#<%=hf_idAttrezzatura.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('INSERIMENTO');
            $("#<%=btnInsAttrezzatura.ClientID%>").click();
        }

        // APRO LE TAB DETTAGLIO ATTREZZATURA
        function openDettaglioAttrezzatura(tipoName) {
            $("#<%=hf_tabChiamata.ClientID%>").val(tipoName);
            if (document.getElementById(tipoName) != undefined) {
                var i;
                var x = document.getElementsByClassName("attr");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(tipoName).style.display = "block";
            }
        }

        function chiudiPopup() {
            // QUANDO APRO IL POPUP RIPARTE SEMPRE DA ATTREZZATURA E NON DALL'ULTIMA TAB APERTA
            $("#<%=hf_tabChiamata.ClientID%>").val('Attrezzatura');
            $("#<%=hf_idAttrezzatura.ClientID%>").val('');
            $("#<%=hf_tipoOperazione.ClientID%>").val('VISUALIZZAZIONE');
            $("#<%=btnChiudiPopupServer.ClientID%>").click();
        }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCodiceVS.ClientID%>").val('');
            $("#<%=ddlTipoCategoria.ClientID%>").val('');
            $("#<%=ddlTipoSubCategoria.ClientID%>").val('');
            $("#<%=tbDescrizione.ClientID%>").val('');
            $("#<%=tbSeriale.ClientID%>").val('');
            $("#<%=ddlTipoGruppoMagazzino.ClientID%>").val('');
            $("#<%=ddlTipoPosizioneMagazzino.ClientID%>").val('');
            $("#<%=tbMarca.ClientID%>").val('');
            $("#<%=tbModello.ClientID%>").val('');
<%--        $("#<%=cbDisponibile.ClientID%>").val('');
            $("#<%=cbGaranzia.ClientID%>").val('');--%>
        }
    </script>
    
    <label><asp:Label ID="lblProtocolli" runat="server" Text="ATTREZZATURE" ForeColor="SteelBlue"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Codice Video System</label>
                    <asp:TextBox ID="tbCodiceVS" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Categoria</label>
                    <asp:DropDownList ID="ddlTipoCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoCategoria_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Sub Categoria</label>
                    <asp:DropDownList ID="ddlTipoSubCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlTipoSubCategoria_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Gruppo</label>
                    <asp:DropDownList ID="ddlTipoGruppoMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>   
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Posizione Magazzino</label>
                    <asp:DropDownList ID="ddlTipoPosizioneMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Seriale</label>
                    <asp:TextBox ID="tbSeriale" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-half">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>

            </div>
            <div class="w3-row-padding">
                <div class="w3-half">
                    <label>Marca</label>
                    <asp:TextBox ID="tbMarca" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-half">
                    <label>Modello</label>
                    <asp:TextBox ID="tbModello" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
<%--                <div class="w3-quarter">
                    <label>Garanzia</label><br />
                    <asp:CheckBox ID="cbGaranzia" runat="server" CssClass="w3-check"></asp:CheckBox>
                </div>
                <div class="w3-quarter">
                    <label>Disponibile</label><br />
                    <asp:CheckBox ID="cbDisponibile" runat="server" CssClass="w3-check"></asp:CheckBox>
                </div>--%>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-half">
                    <label>&nbsp;</label>
                    &nbsp;
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>&nbsp;</label>
                    &nbsp;
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Button ID="btnRicercaAttrezzatura" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaAttrezzatura_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 40%;">
                                <div id="divBtnInserisciAttrezzatura" runat="server">
                                    <div id="clbtnInserisciAttrezzatura" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciAttrezzatura();" >Inserisci</div>
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
                <div class="w3-container w3-center">
                    <table class="w3-table w3-small" style="width:200px">
                        <tr>
                            <th>Tot.Elementi</th>
                            <th><asp:TextBox runat="server" class="w3-input w3-border" ID="tbTotElementiGriglia" Text="" ReadOnly="true" Height="15px" /></th>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gv_attrezzature" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_attrezzature_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_attrezzature_PageIndexChanging" PageSize="20" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Attrezzatura" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaAttrezzatura" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:Button runat="server" ID="btnEditAttrezzatura" Style="display: none" OnClick="btnEditAttrezzatura_Click" />
    <asp:Button runat="server" ID="btnInsAttrezzatura" Style="display: none" OnClick="btnInsAttrezzatura_Click" />
    <asp:Button runat="server" ID="btnChiudiPopupServer" Style="display: none" OnClick="btnChiudiPopupServer_Click" />

    <asp:HiddenField ID="hf_idAttrezzatura" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_tabChiamata" runat="server" EnableViewState="true" Value="Attrezzatura" />

    <asp:UpdatePanel ID="upColl" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlContainer" Visible="false">
                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                    <div class="w3-container w3-center w3-xlarge">
                        GESTIONE ATTREZZATURE
                    </div>
                    <br />

                    <div class="w3-container">
                        <!-- ELENCO TAB DETTAGLI ATTREZZATURE -->
                        <div class="w3-bar w3-red w3-round">
                            <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAttrezzatura('Attrezzatura')">Attrezzatura</div>
                            <div class="w3-bar-item w3-button w3-red w3-right">
                                <div id="btnChiudiPopup" class="w3-button w3-red w3-small w3-round" onclick="chiudiPopup();">Chiudi</div>
                            </div>
                        </div>
                    </div>
                    <!-- TAB ATTREZZATURE -->
                    <div id="Attrezzatura" class="w3-container w3-border attr" style="display: block">
                        <div class="w3-container w3-center">
                            <p>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-quarter">
                                        <label>Codice Videosystem</label>
                                        <asp:TextBox ID="tbMod_CodiceVideoSystem" runat="server" MaxLength="20" class="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Marca</label>
                                        <asp:TextBox ID="tbMod_Marca" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Modello</label>
                                        <asp:TextBox ID="tbMod_Modello" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>

                                    <div class="w3-quarter">
                                        <label>Seriale</label>
                                        <asp:TextBox ID="tbMod_Seriale" runat="server" MaxLength="20" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-threequarter">
                                        <label>Descrizione</label>
                                        <asp:TextBox ID="tbMod_Descrizione" runat="server" MaxLength="100" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                        <asp:TextBox ID="tbIdAttrezzaturaDaModificare" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="w3-quarter" style="position: relative">
                                        <label>Data Acquisto</label>
                                        <asp:TextBox ID="tbMod_DataAcquisto" runat="server" MaxLength="10" CssClass="w3-input w3-border calendar" placeholder="GG/MM/AAAA" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-quarter">
                                        <label>Categoria</label>
                                        <asp:DropDownList ID="cmbMod_Categoria" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Subcategoria</label>
                                        <asp:DropDownList ID="cmbMod_SubCategoria" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Gruppo</label>
                                        <asp:DropDownList ID="cmbMod_Gruppo" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>                                        
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Posizione</label>
                                        <asp:DropDownList ID="cmbMod_Posizione" runat="server" AutoPostBack="True" Width="100%" CssClass="w3-input w3-border">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="w3-row-padding w3-center w3-text-center">
                                    <div class="w3-quarter">
                                        <label>Garanzia</label><br />
                                        <asp:CheckBox ID="cbMod_Garanzia" runat="server" CssClass="w3-check"></asp:CheckBox>
                                    </div>
                                    <div class="w3-quarter">
                                        <label>Disponibile</label><br />
                                        <asp:CheckBox ID="cbMod_Disponibile" runat="server" CssClass="w3-check"></asp:CheckBox>
                                    </div>
                                    <div class="w3-rest">
                                        <label>Note</label>
                                        <asp:TextBox ID="tbMod_Note" runat="server" CssClass="w3-input w3-border" placeholder="" Text=""></asp:TextBox>
                                    </div>
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnGestisciAttrezzatura" runat="server" Text="Gestisci Attrezzatura" class="w3-panel w3-green w3-border w3-round" OnClick="btnGestisciAttrezzatura_Click" />
                                    <asp:Button ID="btnInserisciAttrezzatura" runat="server" Text="Inserisci Attrezzatura" class="w3-panel w3-green w3-border w3-round" OnClick="btnInserisciAttrezzatura_Click" OnClientClick="return confirm('Confermi inserimento Attrezzatura?')" />
                                    <asp:Button ID="btnModificaAttrezzatura" runat="server" Text="Modifica Attrezzatura" class="w3-panel w3-green w3-border w3-round" OnClick="btnModificaAttrezzatura_Click" OnClientClick="return confirm('Confermi modifica Attrezzatura?')" Visible="false" />
                                    <asp:Button ID="btnEliminaAttrezzatura" runat="server" Text="Elimina Attrezzatura" class="w3-panel w3-green w3-border w3-round" OnClick="btnEliminaAttrezzatura_Click" OnClientClick="return confirm('Confermi eliminazione Attrezzatura?')" Visible="false" />
                                    <asp:Button ID="btnAnnullaAttrezzatura" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnullaAttrezzatura_Click" />
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
            <asp:AsyncPostBackTrigger ControlID="btnInserisciAttrezzatura" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnModificaAttrezzatura" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEliminaAttrezzatura" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>
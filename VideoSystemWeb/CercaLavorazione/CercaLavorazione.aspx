<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CercaLavorazione.aspx.cs" Inherits="VideoSystemWeb.CercaLavorazione.CercaLavorazione" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script>
        $(document).ready(function () {
            $('.loader').hide();

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

        // APRO POPUP VISUALIZZAZIONE/MODIFICA PROTOCOLLO
            function apriLavorazione(dataInizioImpegno, idColonneAgenda) { 
                $('.loader').show();
                $("#<%=hf_dataInizioImpegno.ClientID%>").val(dataInizioImpegno);
                $("#<%=hf_idColonneAgenda.ClientID%>").val(idColonneAgenda);
                 $("#<%=btnVaiALavorazione.ClientID%>").click();
            }

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=tbCodiceLavoro.ClientID%>").val('');

            $("#<%=tbRagioneSociale.ClientID%>").val('');

            $("#<%=tbNumeroProtocollo.ClientID%>").val('');
            $("#<%=tbDataProtocollo.ClientID%>").val('');
            $("#<%=tbDataProtocolloA.ClientID%>").val('');
            $("#<%=tbDataLavorazione.ClientID%>").val('');
            $("#<%=tbDataLavorazioneA.ClientID%>").val('');

            $("#<%=tbProduzione.ClientID%>").val('');

            $("#<%=tbProtocolloRiferimento.ClientID%>").val('');

            $("#<%=tbLavorazione.ClientID%>").val('');

            $("#<%=tbDescrizione.ClientID%>").val('');

            $("#<%=ddlTipoProtocollo.ClientID%>").val('');
            $("#<%=ddlDestinatario.ClientID%>").val('');
        }

        function popupProt(messaggio) {
            document.getElementById('popMessage').style.display = 'block';
            $('#textPopMess').html(messaggio);
        }
        </script>

        <div id="popMessage" class="w3-modal" style="z-index:10000">
            <div class="w3-modal-content w3-animate-opacity w3-card-4">
                <header class="w3-container w3-green">
                    <span onclick="document.getElementById('popMessage').style.display='none'"
                        class="w3-button w3-display-topright">&times;</span>
                    <h2>Operazione completata</h2>
                </header>
                <div class="w3-container">
                    <p id="textPopMess"></p>
                </div>
            </div>
        </div>
    <label><asp:Label ID="lblCercaLavorazione" runat="server" Text="CERCA LAVORAZIONE" ForeColor="Teal"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Cliente/Fornitore</label>
                    <asp:TextBox ID="tbRagioneSociale" runat="server" MaxLength="60" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Numero Protocollo</label>
                    <asp:TextBox ID="tbNumeroProtocollo" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Riferimento Documento</label>
                    <asp:TextBox ID="tbProtocolloRiferimento" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Lav. (Da-A)</label>
                    <table style="width:100%;">
                        <tr>
                            <td style="position:relative;"><asp:TextBox ID="tbDataLavorazione" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                            <td style="position:relative;"><asp:TextBox ID="tbDataLavorazioneA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Produzione</label>
                    <asp:TextBox ID="tbProduzione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Lavorazione</label>
                    <asp:TextBox ID="tbLavorazione" runat="server" MaxLength="50" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                
                <div class="w3-quarter">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="200" class="w3-input w3-border" placeholder=""></asp:TextBox>                   
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Prot. (Da-A)</label>
                    <table style="width:100%;">
                        <tr>
                            <td style="position:relative;"><asp:TextBox ID="tbDataProtocollo" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                            <td style="position:relative;"><asp:TextBox ID="tbDataProtocolloA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-quarter">
                    <label>Codice Lavoro</label>
                    <asp:TextBox ID="tbCodiceLavoro" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Destinatario</label>
                    <asp:DropDownList ID="ddlDestinatario" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                        <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Tipo</label>
                    <asp:DropDownList ID="ddlTipoProtocollo" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 80%;">
                                <asp:Button ID="btnRicercaLavorazione" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaLavorazione_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>

                            <td style="width: 20%;">
                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" OnClick="BtnPulisciCampiRicerca_Click" />
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
                <asp:GridView ID="gv_protocolli" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_protocolli_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_protocolli_PageIndexChanging" PageSize="20"  AllowSorting="true" OnSorting="gv_protocolli_Sorting" AutoGenerateColumns="false">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="id"  ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="data_inizio_impegno" HeaderText="data_inizio_impegno"  ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="id_colonne_agenda" HeaderText="id_colonne_agenda"  ItemStyle-HorizontalAlign="Right"/>

                        <asp:BoundField DataField="Cod. Lav." HeaderText="Cod. Lav." HeaderStyle-Width="10%" />
                        <asp:BoundField DataField="data_inizio_lavorazione" HeaderText="Data Inizio Lav." DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="10%" />
                        <%--<asp:BoundField DataField="Num. Prot." HeaderText="Num. Prot."  HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Data Prot." HeaderText="Data Prot." DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="4%" />
                        <asp:BoundField DataField="protocollo_riferimento" HeaderText="Riferimento"  HeaderStyle-Width="4%" />--%>
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente/Fornitore"  HeaderStyle-Width="20%" />
                        <asp:BoundField DataField="produzione" HeaderText="Produzione" HeaderStyle-Width="15%" />
                        <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="30%" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" HeaderStyle-Width="8%" />

                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/arrow-right-icon.png" ToolTip="Apri Lavorazione" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaLavorazione" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hf_dataInizioImpegno" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hf_idColonneAgenda" runat="server" EnableViewState="true" />
    <asp:Button runat="server" ID="btnVaiALavorazione" Style="display: none" OnClick="btnVaiALavorazione_Click" />

</asp:Content>

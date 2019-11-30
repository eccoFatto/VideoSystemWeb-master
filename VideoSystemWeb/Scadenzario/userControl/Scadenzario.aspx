<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scadenzario.aspx.cs" Inherits="VideoSystemWeb.Scadenzario.userControl.Scadenzario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaScadenza.ClientID%>").click();
                    }
                    else {
                        $("#<%=btnRicercaScadenza.ClientID%>").click();
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

        function inserisciScadenza() {
            $('.loader').show();
           
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
    <label><asp:Label ID="lblScadenzario" runat="server" Text="SCADENZARIO" ForeColor="Teal"></asp:Label></label>

    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Tipo Anagrafica</label>
                    <asp:DropDownList ID="ddl_TipoAnagrafica" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" Selected></asp:ListItem>
                        <asp:ListItem Value="Cliente" Text="Cliente"></asp:ListItem>
                        <asp:ListItem Value="Fornitore" Text="Fornitore"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Codice Anagrafica</label>
                    <asp:DropDownList ID="ddl_CodiceAnagrafica" runat="server"/>
                </div>
                <div class="w3-quarter">
                    <label>Numero Fattura</label>
                    <asp:TextBox ID="txt_NumeroFattura" runat="server" MaxLength="20" class="w3-input w3-border" ></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Pagata</label>
                    <asp:DropDownList ID="ddlFatturaPagata" runat="server">
                        <asp:ListItem Value="" Text="<tutti>" Selected/>
                        <asp:ListItem Value="1" Text="Si" />
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-half">
                    <div class="w3-third">
                        <label>Data Fattura</label>
                    </div>
                    <div class="w3-third">
                        <label>Da</label>
                        <asp:TextBox ID="txt_DataFatturaDa" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                    </div>
                    <div class="w3-third">
                        label>A</label>
                        <asp:TextBox ID="txt_DataFatturaA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-half">
                    <div class="w3-third">
                        <label>Scadenza</label>
                    </div>
                    <div class="w3-third">
                        <label>Da</label>
                        <asp:TextBox ID="txt_DataScadenzaDa" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                    </div>
                    <div class="w3-third">
                        label>A</label>
                        <asp:TextBox ID="txt_DataScadenzaA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                    </div>
                </div>
            </div>

            <<div class="w3-row-padding" >
                <div class="w3-third">&nbsp;<//div>

                <div class="w3-third">
                    <asp:Button ID="btnRicercaScadenza" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaScadenza_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                    <div id="clbtnInserisciScadenza" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciScadenza();" >Inserisci</div>
                    <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                <//div>

                <div class="w3-third">&nbsp;<//div>
                    
            </div>

            <div class="round">
                <asp:GridView ID="gv_scadenze" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_scadenze_RowDataBound" AllowPaging="True" OnPageIndexChanging="gv_scadenze_PageIndexChanging" PageSize="20"  AllowSorting="true" OnSorting="gv_scadenze_Sorting">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last"/>
                    <%--<Columns>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/detail-icon.png" ToolTip="Visualizza Protocollo" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" Text="Vis." ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Allegato" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>--%>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

     <asp:HiddenField ID="hf_tipoOperazione" runat="server" EnableViewState="true" />
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ricavi.aspx.cs" Inherits="VideoSystemWeb.STATISTICHE.Ricavi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
                });
            });
        });

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=hf_NomeCliente.ClientID%>").val('');
            $("#<%=ddl_Cliente.ClientID%>").val('');

            $("#<%=hf_NomeProduzione.ClientID%>").val('');
            $("#<%=ddl_Produzione.ClientID%>").val('');

            $("#<%=hf_NomeLavorazione.ClientID%>").val('');
            $("#<%=ddl_Lavorazione.ClientID%>").val('');

            $("#<%=hf_NomeContratto.ClientID%>").val('');
            $("#<%=ddl_Contratto.ClientID%>").val('');

            $("#<%=ddlFatturato.ClientID%>").val('1');

            $("#<%=txt_PeriodoDa.ClientID%>").val('');
            $("#<%=txt_PeriodoA.ClientID%>").val('');

            <%--$("#<%=btn_aggiornaFiltri.ClientID%>").click();--%>
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            // GESTIONE DROPDOWN CLIENTE
            $("#filtroCliente").on("keyup", function () {
                var value = $(this).val().toLowerCase();               
                $("#divCliente .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
                
            });
            $("#<%=elencoClienti.ClientID%> .dropdown-item").on("click", function (e) {               
                $("#<%=hf_NomeCliente.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Cliente.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Cliente.ClientID%>").attr("title", $(e.target).text());

                <%--$("#<%=btn_aggiornaFiltri.ClientID%>").click();--%> //per eliminare aggiornamento filtri commentare questa riga 
            });

            // GESTIONE DROPDOWN PRODUZIONE
            $("#filtroProduzione").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divProduzione .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#<%=elencoProduzioni.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_NomeProduzione.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Produzione.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Produzione.ClientID%>").attr("title", $(e.target).text());

                <%--$("#<%=btn_aggiornaFiltri.ClientID%>").click();--%>  //per eliminare aggiornamento filtri commentare questa riga 
            });

            // GESTIONE DROPDOWN LAVORAZIONE
            $("#filtroLavorazione").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divLavorazione .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#<%=elencoLavorazioni.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_NomeLavorazione.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Lavorazione.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Lavorazione.ClientID%>").attr("title", $(e.target).text());

                <%--$("#<%=btn_aggiornaFiltri.ClientID%>").click();--%>  //per eliminare aggiornamento filtri commentare questa riga 
            });

            // GESTIONE DROPDOWN CONTRATTO
            $("#filtroContratto").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#divContratto .dropdown-menu li").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#<%=elencoContratti.ClientID%> .dropdown-item").on("click", function (e) {
                $("#<%=hf_NomeContratto.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Contratto.ClientID%>").val($(e.target).text());
                $("#<%=ddl_Contratto.ClientID%>").attr("title", $(e.target).text());

                <%--$("#<%=btn_aggiornaFiltri.ClientID%>").click();--%>  //per eliminare aggiornamento filtri commentare questa riga 
            });
        });
    </script>

    <label><asp:Label ID="lblStatisticaRicavi" runat="server" Text="STATISTICA RICAVI" ForeColor="Teal"></asp:Label></label>
    <%--<asp:Button ID="btn_aggiornaFiltri" runat="server" Text="" OnClick="btn_aggiornaFiltri_Click" style="display:none"/>--%>

    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Cliente</label>
                    <div id="divCliente" class="dropdown ">
                        <asp:HiddenField ID="hf_NomeCliente" runat="server" Value=""/>
                        <asp:Button ID="ddl_Cliente" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divClienti" Text=""  Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoClienti" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                    </div>
                </div>

                <div class="w3-quarter">
                    <label>Produzione</label>
                    <div id="divProduzione" class="dropdown ">
                        <asp:HiddenField ID="hf_NomeProduzione" runat="server" Value="" />
                        <asp:Button ID="ddl_Produzione" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divProduzione" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoProduzioni" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                    </div>
                </div>

                <div class="w3-quarter">
                    <label>Lavorazione</label>
                    <div id="divLavorazione" class="dropdown ">
                        <asp:HiddenField ID="hf_NomeLavorazione" runat="server" Value="" />
                        <asp:Button ID="ddl_Lavorazione" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divLavorazione" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoLavorazioni" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                    </div>
                </div>

                <div class="w3-quarter">
                    <label>Contratto</label>
                    <div id="divContratto" class="dropdown ">
                        <asp:HiddenField ID="hf_NomeContratto" runat="server" Value="" />
                        <asp:Button ID="ddl_Contratto" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divContratto" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                        <ul id="elencoContratti" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                    </div>
                </div>
            </div>

            <div class="w3-row-padding" style="position:relative;">
                <div class="w3-quarter">
                    <label>Fatturato</label>
                    <asp:DropDownList ID="ddlFatturato" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                        <asp:ListItem Value="" Text="<tutti>" />
                        <asp:ListItem Value="1" Text="Si" Selected/>
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </div>

                <div class="w3-quarter">
                    <label>Periodo da</label>
                    <asp:TextBox ID="txt_PeriodoDa" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>

                <div class="w3-quarter">
                    <label>Periodo a</label>
                    <asp:TextBox ID="txt_PeriodoA" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                
                <div class="w3-quarter">
                    <div class="w3-row">
                        <div class="w3-third">Listino</div>
                        <div class="w3-twothird"><asp:CheckBox ID="chk_Listino" runat="server" Checked/></div>
                    </div>
                    <div class="w3-row">
                        <div class="w3-third">Costi</div>
                        <div class="w3-twothird"><asp:CheckBox ID="chk_Costi" runat="server" Checked/></div>
                    </div>
                    <div class="w3-row">
                        <div class="w3-third">Ricavo</div>
                        <div class="w3-twothird"><asp:CheckBox ID="chk_Ricavo" runat="server" Checked/></div>
                    </div>
                </div>
            </div>

            <div class="w3-row-padding w3-margin-bottom w3-margin-top">
                <div class="w3-threequarter" style="font-size:.8em;"> </div>

                <div class="w3-quarter" style="float:right; text-align:center;">
                    <div class="row">
                        <div class="w3-half">
                            <asp:Button ID="btnEseguiStatistica" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnEseguiStatistica_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </div>
                        <div class="w3-half">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                        </div>
                    </div>
                </div>

            </div>

            <div class="round">
                <asp:GridView ID="gv_statistiche" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" 
                    CssClass="grid" AllowPaging="False"  EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gv_statistiche_RowDataBound">
                    <%--<PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>--%>
                    <Columns>
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-Width="10%"/>
                        <asp:BoundField DataField="NumeroFattura" HeaderText="N. Fattura" HeaderStyle-Width="7%"/>
                        <asp:BoundField DataField="Ordine" HeaderText="Ordine" HeaderStyle-Width="7%"/>
                        <asp:BoundField DataField="CodiceLavoro" HeaderText="Codice" HeaderStyle-Width="7%"/>
                        <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="7%"/>
                        <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="17%"/>
                        <asp:BoundField DataField="Produzione" HeaderText="Produzione" HeaderStyle-Width="17%"/>
                        <asp:BoundField DataField="Contratto" HeaderText="Contratto" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Listino" HeaderText="Listino" DataFormatString="{0:N2}" HeaderStyle-Width="7%"/>
                        <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:N2}" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="Ricavo" HeaderText="Ricavo" DataFormatString="{0:P2}" HeaderStyle-Width="6%" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

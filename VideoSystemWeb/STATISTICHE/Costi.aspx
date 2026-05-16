<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Costi.aspx.cs" Inherits="VideoSystemWeb.STATISTICHE.Costi" %>

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

            // CALCOLO DATE PER CALENDARI
            var d = new Date();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var firstDay = 1;
            var lastDay = new Date(year, month, 0).getDate();


            // CAMBIO MESE
            $("#<%=ddl_Mese.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //CAMBIO ANNO
            $("#<%=ddl_Anno.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //SETTO CALENDARI
            var meseSelezionato = $("#<%=ddl_Mese.ClientID%>").val();
            if (meseSelezionato == '')
                $("#<%=ddl_Anno.ClientID%>").attr("disabled", "disabled");
            else
                $("#<%=ddl_Anno.ClientID%>").removeAttr("disabled");
            
            function changeMonthYear(newMonth, newYear) {
                if (newMonth == '') {
                    $("#<%=ddl_Anno.ClientID%>").attr("disabled", "disabled");

                    $("#<%=txt_PeriodoDa.ClientID%>").val('');
                    $("#<%=txt_PeriodoA.ClientID%>").val('');
                } else {
                    $("#<%=ddl_Anno.ClientID%>").removeAttr("disabled");

                    var lastDay = new Date(newYear, newMonth, 0).getDate();
                    $("#<%=txt_PeriodoDa.ClientID%>").val(firstDay + "/" + newMonth + "/" + newYear);
                    $("#<%=txt_PeriodoA.ClientID%>").val(lastDay + "/" + newMonth + "/" + newYear);
                }
            }
        });

        // AZZERO TUTTI I CAMPI RICERCA
        function azzeraCampiRicerca() {
            $("#<%=txt_Cliente.ClientID%>").val('');
            $("#<%=txt_Produzione.ClientID%>").val('');
            $("#<%=txt_Lavorazione.ClientID%>").val('');
            //$("#< % =txt_Contratto.ClientID %>").val('');
            $("#<%=txt_CodLavorazione.ClientID%>").val('');
            $("#<%=txt_PeriodoDa.ClientID%>").val('');
            $("#<%=txt_PeriodoA.ClientID%>").val('');
            $("#<%=ddl_Genere.ClientID%>").val('');
            $("#<%=ddl_Gruppo.ClientID%>").val('');
            $("#<%=ddl_Sottogruppo.ClientID%>").val('');
            $("#<%=txt_Fornitore.ClientID%>").val('');
            $("#<%=chk_Listino.ClientID%>").prop('checked', true);
            $("#<%=chk_Costi.ClientID%>").prop('checked', true);
            $("#<%=chk_Ricavo.ClientID%>").prop('checked', true);

            $("#<%=ddl_Mese.ClientID%>").val('');
            $("#<%=ddl_Anno.ClientID%>").val('');
        }

        // APRO LAVORAZIONE
        function apriLavorazione(codiceLavoro) {
            $('.loader').show();
            $("#<%=hf_codiceLavoro.ClientID%>").val(codiceLavoro);
            $("#<%=btnVaiALavorazione.ClientID%>").click();
        }
    </script>

    <label>
        <asp:Label ID="lblStatisticaRicavi" runat="server" Text="STATISTICA COSTI" ForeColor="Teal"></asp:Label></label>

        <div class="w3-row">
            <div class="w3-col" style="width:88%">
<!-- FILTRI LAVORAZIONE-->
                <div class="w3-row round" style="padding:5px; margin-bottom:10px;position: relative;">
                    <div class="w3-row">
                        <b>Filtri Lavorazione</b>
                    </div>
                    <div class="w3-row-padding" >
                        <div class="w3-col" style="width:15%">
                            <label>Cliente</label>
                            <asp:TextBox ID="txt_Cliente" runat="server" Width="100%" class="w3-input w3-border " />
                        </div>

                        <div class="w3-col" style="width:15%">
                            <label>Produzione</label>
                            <asp:TextBox ID="txt_Produzione" runat="server" Width="100%" class="w3-input w3-border " />
                        </div>

                        <div class="w3-col" style="width:15%">
                            <label>Lavorazione</label>
                            <asp:TextBox ID="txt_Lavorazione" runat="server" Width="100%" class="w3-input w3-border " />
                        </div>

                        <div class="w3-col" style="width:13%">
                            <label>Cod. Lavorazione</label>
                            <asp:TextBox ID="txt_CodLavorazione" runat="server" Width="100%" class="w3-input w3-border " />
                        </div>

                       <%-- <div class="w3-col" style="width:15%">
                            <label>Contratto</label>
                            <asp:TextBox ID="txt_Contratto" runat="server" Width="100%" class="w3-input w3-border " />
                        </div>--%>

                        <div class="w3-col" style="width:10%">
                            <label>Mese</label>
                            <asp:DropDownList ID="ddl_Mese" runat="server" class="w3-input w3-border">
                                
                                <asp:ListItem Value=""> </asp:ListItem>
                                <asp:ListItem Value="1">Gennaio</asp:ListItem>
                                <asp:ListItem Value="2">Febbraio</asp:ListItem>
                                <asp:ListItem Value="3">Marzo</asp:ListItem>
                                <asp:ListItem Value="4">Aprile</asp:ListItem>
                                <asp:ListItem Value="5">Maggio</asp:ListItem>
                                <asp:ListItem Value="6">Giugno</asp:ListItem>
                                <asp:ListItem Value="7">Luglio</asp:ListItem>
                                <asp:ListItem Value="8">Agosto</asp:ListItem>
                                <asp:ListItem Value="9">Settembre</asp:ListItem>
                                <asp:ListItem Value="10">Ottobre</asp:ListItem>
                                <asp:ListItem Value="11">Novembre</asp:ListItem>
                                <asp:ListItem Value="12">Dicembre</asp:ListItem>

                            </asp:DropDownList>
                        </div>
                        <div class="w3-col" style="width:6%">
                            <label>Anno</label>
                            <asp:DropDownList ID="ddl_Anno" runat="server" class="w3-input w3-border" ></asp:DropDownList>
                        </div>

                        <div class="w3-col" style="width:13%">
                            <label>Periodo da</label>
                            <asp:TextBox ID="txt_PeriodoDa" runat="server" MaxLength="10"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>

                        <div class="w3-col" style="width:13%">
                            <label>Periodo a</label>
                            <asp:TextBox ID="txt_PeriodoA" runat="server" MaxLength="10"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                        </div>
                    </div>
                </div>

<!-- FILTRI COSTI-->
                <div class="w3-row round " style="padding:5px; margin-bottom:10px;">
                    <div class="w3-row">
                        <b>Filtri Costi</b>
                    </div>
                    <div class="w3-row-padding" style="position: relative; ">
                        <div class="w3-quarter">
                            <label>Fornitore</label>
                            <asp:TextBox ID="txt_Fornitore" runat="server"  Width="100%" class="w3-input w3-border" />
                        </div>

                        <div class="w3-quarter">
                            <label>Genere</label>
                            <asp:DropDownList ID="ddl_Genere" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border" />
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
                            <ContentTemplate>
                                <div class="w3-quarter" style="padding:0px 8px">
                                    <label>Gruppo</label>
                                    <asp:DropDownList ID="ddl_Gruppo" runat="server"  Width="100%" class="w3-input w3-border" OnSelectedIndexChanged="ddlGruppo_SelectedIndexChanged" AutoPostBack="true"/>
                                </div>

                                <div class="w3-quarter" style="padding:0px 8px">
                                    <label>Sottogruppo</label>
                                    <asp:DropDownList ID="ddl_Sottogruppo" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

<!-- PULSANTI-->
            <div class="w3-rest w3-padding w3-right-align " style="margin-top:60px;">
                <div class="w3-row">
                    <div class="w3-twothird w3-right-align">Listino</div>
                    <div class="w3-third">
                        <asp:CheckBox ID="chk_Listino" runat="server" Checked />
                    </div>
                </div>
                <div class="w3-row">
                    <div class="w3-twothird w3-right-align">Costi</div>
                    <div class="w3-third">
                        <asp:CheckBox ID="chk_Costi" runat="server" Checked />
                    </div>
                </div>
                <div class="w3-row">
                    <div class="w3-twothird w3-right-align">Ricavo</div>
                    <div class="w3-third">
                        <asp:CheckBox ID="chk_Ricavo" runat="server" Checked />
                    </div>
                </div>

                <div class="w3-row w3-right" style="margin-top: 40px;">
                    <div class="row">
                        <div class="w3-half" style="padding-right:10px">
                            <asp:Button ID="btnEseguiStatistica" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnEseguiStatistica_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                        </div>
                        <div class="w3-half">
                            <asp:Button ID="btnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" OnClick="btnPulisciCampiRicerca_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

<!-- GRIGLIA RISULTATI-->
        <div class="round">
            <div class="w3-container w3-center">
                <table class="w3-table w3-small" style="width:200px">
                    <tr>
                        <th>Tot.Elementi costo</th>
                        <th><asp:TextBox runat="server" class="w3-input w3-border" ID="tbTotElementiGriglia" Text="" ReadOnly="true" Height="15px" /></th>
                    </tr>
                </table>
            </div>

            <div id="rigaTotali" runat="server" visible="false" class="w3-container w3-center" style="font-size: 10pt; width: 100%; position: relative; background-color: #808080; padding:0px;">
                <table class="w3-table w3-small"  CssClass="grid">
                    <tr>
                        <th Width="65%" style="text-align:right">Totale</th>
                        <th Width="4%" runat="server" id="colonnaListino"><asp:label runat="server"  ID="lbl_TotaleListino" Text="" ReadOnly="true" Height="15px" /></th>
                        <th Width="4%" runat="server" id="colonnaCosto"><asp:label runat="server"  ID="lbl_TotaleCosto" Text="" ReadOnly="true" Height="15px" /></th>
                        <th Width="4%" runat="server" id="colonnaRicavo"><asp:label runat="server"  ID="lbl_TotaleRicavo" Text="" ReadOnly="true" Height="15px" /></th>
                    </tr>
                </table>
            </div>

            <asp:GridView ID="gv_statistiche" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;"
                CssClass="grid" AllowPaging="False" EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gv_statistiche_RowDataBound" >
                <%--<PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>--%>
                <Columns>
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-Width="10%" />

                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="3%">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="20px" />
                            <asp:ImageButton ID="btnLavorazione" runat="server" CausesValidation="false" Text="Apri" ImageUrl="~/Images/arrow-right-icon.png" ToolTip="Apri Lavorazione" ImageAlign="AbsMiddle" Height="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Gruppo" HeaderText="Gruppo" HeaderStyle-Width="7%" />
                    <asp:BoundField DataField="Sottogruppo" HeaderText="Sottogruppo" HeaderStyle-Width="9%" />
                    <asp:BoundField DataField="numeroFattura" HeaderText="Num. Fattura" HeaderStyle-Width="6%" />
                    <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" HeaderStyle-Width="13%" />
                    <asp:BoundField DataField="CodiceLavoro" HeaderText="Codice" HeaderStyle-Width="7%" />
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="5%" />
                    <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="17%" />
                    <asp:BoundField DataField="Produzione" HeaderText="Produzione" HeaderStyle-Width="14%" />
                    <%--<asp:BoundField DataField="Contratto" HeaderText="Contratto" HeaderStyle-Width="8%" />--%>
                    <asp:BoundField DataField="Listino" HeaderText="Listino" DataFormatString="{0:N2}" HeaderStyle-Width="4%" />
                    <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:N2}" HeaderStyle-Width="4%" />
                    <asp:BoundField DataField="Ricavo" HeaderText="Ricavo" DataFormatString="{0:P2}" HeaderStyle-Width="4%" />

                    <asp:BoundField DataField="DocumentoAllegato" HeaderText="" />
                    <asp:BoundField DataField="Pregresso" HeaderText="" />
                    <asp:BoundField DataField="CodiceLavoro2" HeaderText="Codice" HeaderStyle-Width="7%" />
                </Columns>
            </asp:GridView>
        </div>

        <asp:HiddenField ID="hf_codiceLavoro" runat="server" EnableViewState="true" />
        <asp:Button runat="server" ID="btnVaiALavorazione" Style="display: none" OnClick="btnVaiALavorazione_Click" />
</asp:Content>

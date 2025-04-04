﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Costi.aspx.cs" Inherits="VideoSystemWeb.STATISTICHE.Costi" %>

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
            <%--$("#<%=hf_NomeCliente.ClientID%>").val('');
            $("#<%=ddl_Cliente.ClientID%>").val('');--%>
            $("#<%=txt_Cliente.ClientID%>").val('');

            <%--$("#<%=hf_NomeProduzione.ClientID%>").val('');
            $("#<%=ddl_Produzione.ClientID%>").val('');--%>
            $("#<%=txt_Produzione.ClientID%>").val('');

            <%--$("#<%=hf_NomeLavorazione.ClientID%>").val('');
            $("#<%=ddl_Lavorazione.ClientID%>").val('');--%>
            $("#<%=txt_Lavorazione.ClientID%>").val('');

           <%-- $("#<%=hf_NomeContratto.ClientID%>").val('');
            $("#<%=ddl_Contratto.ClientID%>").val('');--%>
            $("#<%=txt_Contratto.ClientID%>").val('');

            <%--$("# < % = ddlFatturato.ClientID%>").val('');--%>
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

            
        }

        <%--Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=ddl_Cliente.ClientID%>").val(($("#<%=hf_NomeCliente.ClientID%>").val()));
            $("#<%=ddl_Produzione.ClientID%>").val($("#<%=hf_NomeProduzione.ClientID%>").val());
            $("#<%=ddl_Lavorazione.ClientID%>").val($("#<%=hf_NomeLavorazione.ClientID%>").val());
            $("#<%=ddl_Contratto.ClientID%>").val($("#<%=hf_NomeContratto.ClientID%>").val());

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
            });
        });--%>
    </script>

    <label>
        <asp:Label ID="lblStatisticaRicavi" runat="server" Text="STATISTICA COSTI" ForeColor="Teal"></asp:Label></label>

    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row">
                <div class="w3-col" style="width:88%">
<!-- FILTRI LAVORAZIONE-->
                    <div class="w3-row round" style="padding:5px; margin-bottom:10px;position: relative;">
                        <div class="w3-row">
                            <b>Filtri Lavorazione</b>
                        </div>
                        <div class="w3-row-padding" >
                            <div class="w3-col" style="width:16%">
                                <label>Cliente</label>
                                <%--<div id="divCliente" class="dropdown ">
                                    <asp:HiddenField ID="hf_NomeCliente" runat="server" Value=""/>
                                    <asp:Button ID="ddl_Cliente" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divClienti" Text=""  Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                                    <ul id="elencoClienti" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                                </div>--%>
                                <asp:TextBox ID="txt_Cliente" runat="server" Width="100%" class="w3-input w3-border " />
                            </div>

                            <div class="w3-col" style="width:16%">
                                <label>Produzione</label>
                                <%--<div id="divProduzione" class="dropdown ">
                                    <asp:HiddenField ID="hf_NomeProduzione" runat="server" Value="" />
                                    <asp:Button ID="ddl_Produzione" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divProduzione" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                                    <ul id="elencoProduzioni" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                                </div>--%>
                                <asp:TextBox ID="txt_Produzione" runat="server" Width="100%" class="w3-input w3-border " />
                            </div>

                            <div class="w3-col" style="width:16%">
                                <label>Lavorazione</label>
                                <%--<div id="divLavorazione" class="dropdown ">
                                    <asp:HiddenField ID="hf_NomeLavorazione" runat="server" Value="" />
                                    <asp:Button ID="ddl_Lavorazione" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divLavorazione" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                                    <ul id="elencoLavorazioni" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                                </div>--%>
                                <asp:TextBox ID="txt_Lavorazione" runat="server" Width="100%" class="w3-input w3-border " />
                            </div>

                            <div class="w3-col" style="width:15%">
                                <label>Contratto</label>
                                <%-- <div id="divContratto" class="dropdown ">
                                    <asp:HiddenField ID="hf_NomeContratto" runat="server" Value="" />
                                    <asp:Button ID="ddl_Contratto" runat="server" AutoPostBack="False" Width="100%" CssClass="w3-input w3-border" data-toggle="dropdown" data-boundary="divContratto" Text="" Style="text-overflow: ellipsis; overflow: hidden; height:37px;background-color: white;text-align:left;" />
                                    <ul id="elencoContratti" class="dropdown-menu" runat="server" style="max-height: 350px; overflow: auto;padding-top:0px"></ul>
                                </div>--%>
                                <asp:TextBox ID="txt_Contratto" runat="server" Width="100%" class="w3-input w3-border " />
                            </div>

                            <div class="w3-col" style="width:11%">
                                <%--<label>Fatturato</label>
                                <asp:DropDownList ID="ddlFatturato" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border">
                                    <asp:ListItem Value="" Text="<tutti>" />
                                    <asp:ListItem Value="1" Text="Si" />
                                    <asp:ListItem Value="0" Text="No" />
                                </asp:DropDownList>--%>
                                <label>Cod. Lavorazione</label>
                                <asp:TextBox ID="txt_CodLavorazione" runat="server" Width="100%" class="w3-input w3-border " />
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

                            <div class="w3-quarter">
                                <label>Gruppo</label>
                                <asp:DropDownList ID="ddl_Gruppo" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border" />
                            </div>

                            <div class="w3-quarter">
                                <label>Sottogruppo</label>
                                <asp:DropDownList ID="ddl_Sottogruppo" runat="server" AutoPostBack="False" Width="100%" class="w3-input w3-border" />
                            </div>
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
                            <th>Tot.Elementi</th>
                            <th><asp:TextBox runat="server" class="w3-input w3-border" ID="tbTotElementiGriglia" Text="" ReadOnly="true" Height="15px" /></th>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="gv_statistiche" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;"
                    CssClass="grid" AllowPaging="False" EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="gv_statistiche_RowDataBound">
                    <%--<PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>--%>
                    <Columns>
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-Width="10%" />

                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnOpenDoc" runat="server" CausesValidation="false" ImageUrl="~/Images/Oxygen-Icons.org-Oxygen-Mimetypes-x-office-contact.ico" ToolTip="Visualizza Documento" ImageAlign="AbsMiddle" Height="30px" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Gruppo" HeaderText="Gruppo" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="numeroFattura" HeaderText="Num. Fattura" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="CodiceLavoro" HeaderText="Codice" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="7%" />
                        <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="17%" />
                        <asp:BoundField DataField="Produzione" HeaderText="Produzione" HeaderStyle-Width="14%" />
                        <asp:BoundField DataField="Contratto" HeaderText="Contratto" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Listino" HeaderText="Listino" DataFormatString="{0:N2}" HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:N2}" HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="Ricavo" HeaderText="Ricavo" DataFormatString="{0:P2}" HeaderStyle-Width="5%" />

                        <asp:BoundField DataField="DocumentoAllegato" HeaderText="" />
                        <asp:BoundField DataField="Pregresso" HeaderText="" />

                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="StampaConsulenteLavoro.aspx.cs" Inherits="VideoSystemWeb.REPORT.StampaConsulenteLavoro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            // CALCOLO DATE PER CALENDARI
            var d = new Date();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var firstDay = 1;
            var lastDay = new Date(year, month, 0).getDate();

            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            //SETTO MESE CORRENTE
            $("#<%=ddl_Mese.ClientID%>").prop('selectedIndex', month-1);

            // CAMBIO MESE
            $("#<%=ddl_Mese.ClientID%>").change(function () { 
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //CAMBIO ANNO
            $("#<%=ddl_Anno.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //SETTO CALENDARI
            $("#<%=txt_DataInizio.ClientID%>").val(firstDay + "/" + month + "/" + year);
            $("#<%=txt_DataFine.ClientID%>").val(lastDay + "/" + month + "/" + year);
            $("#<%=ddl_Mese.ClientID%>").val(month);
            $("#<%=ddl_Anno.ClientID%>").val(year);

            function changeMonthYear(newMonth, newYear) {
                var lastDay = new Date(newYear, newMonth, 0).getDate();
                $("#<%=txt_DataInizio.ClientID%>").val(firstDay + "/" + newMonth + "/" + newYear);
                $("#<%=txt_DataFine.ClientID%>").val(lastDay + "/" + newMonth + "/" + newYear);

            }

            
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.loader').hide();

            // CALCOLO DATE PER CALENDARI
            var d = new Date();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var firstDay = 1;
            var lastDay = new Date(year, month, 0).getDate();

            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            // CAMBIO MESE
            $("#<%=ddl_Mese.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //CAMBIO ANNO
            $("#<%=ddl_Anno.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            function changeMonthYear(newMonth, newYear) {
                var lastDay = new Date(newYear, newMonth, 0).getDate();
                $("#<%=txt_DataInizio.ClientID%>").val(firstDay + "/" + newMonth + "/" + newYear);
                $("#<%=txt_DataFine.ClientID%>").val(lastDay + "/" + newMonth + "/" + newYear);

            }
        });
    </script>

    <label><asp:Label ID="lblStampaConsulente" runat="server" Text="STAMPA CONSULENTE DEL LAVORO" ForeColor="Teal"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding" style="position:relative;">
                <div class="w3-quarter">
                    <div class="w3-twothird" style="padding-right:10px">
                        <label>Mese</label>
                        <asp:DropDownList ID="ddl_Mese" runat="server" class="w3-input w3-border">
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
                    <div class="w3-third">
                        <label>Anno</label>
                        <asp:DropDownList ID="ddl_Anno" runat="server" class="w3-input w3-border"></asp:DropDownList>
                    </div>
                </div>

                <div class="w3-quarter">
                    <label>Data inizio</label>
                    <asp:TextBox ID="txt_DataInizio" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Data fine</label>
                    <asp:TextBox ID="txt_DataFine" runat="server" MaxLength="10" Width="100%"  class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <div class="w3-half" style="padding-right:10px">
                        <asp:Button ID="btnRicerca" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" style="position:absolute;top:27px;" OnClick="btnRicerca_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                    </div>
                    <div class="w3-half" style="padding-right:10px">
                        <asp:Button ID="btnStampa" runat="server" class="w3-btn w3-white w3-border w3-border-blue w3-round-large" style="position:absolute;top:27px;right:100px" OnClientClick="$('.loader').show();" Text="Stampa" OnClick="btnStampa_Click" />
                    </div>
                </div>
            </div>
            <br /><br />
            <div class="round">
                <asp:GridView ID="gv_DatiStampa" runat="server" AutoGenerateColumns="False" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" 
                    CssClass="grid" 
                    EmptyDataText="Nessun dato trovato" EmptyDataRowStyle-HorizontalAlign="Center" OnSorting="gv_DatiStampa_OnSorting">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                    <Columns>
                        <asp:BoundField DataField="NomeCollaboratore" HeaderText="NomeCollaboratore"  />
                        <asp:BoundField DataField="IndirizzoCollaboratore" HeaderText="IndirizzoCollaboratore"  />
                        <asp:BoundField DataField="CittaCollaboratore" HeaderText="CittaCollaboratore"  />
                        <asp:BoundField DataField="TelefonoCollaboratore" HeaderText="TelefonoCollaboratore"  />
                        <asp:BoundField DataField="CodFiscaleCollaboratore" HeaderText="CodFiscaleCollaboratore"  />

                        <asp:BoundField DataField="DataLavorazione" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="8%" />
                        <asp:BoundField DataField="Lavorazione" HeaderText="Lavorazione" HeaderStyle-Width="30%" />
                        <asp:BoundField DataField="Produzione" HeaderText="Produzione"  HeaderStyle-Width="23%" />
                        <asp:BoundField DataField="Cliente" HeaderText="Cliente"  HeaderStyle-Width="20%" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione"  HeaderStyle-Width="15%" />
                        <asp:BoundField DataField="Assunzione" HeaderText="Assunzione" DataFormatString="{0:N2}" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Mista" HeaderText="Mista" DataFormatString="{0:N2}" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Diaria" HeaderText="Diaria" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center"/>
                
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

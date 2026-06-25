<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportTLTime.aspx.cs" Inherits="VideoSystemWeb.REPORT.ReportTLTime" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnRicercaTLTime.ClientID%>").click();
                }
            });

            $('.calendar').datetimepicker({
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            // CALCOLO DATE PER CALENDARI
            var d = new Date();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            var firstDay = 1;
            var lastDay = new Date(year, month, 0).getDate();

            //SETTO MESE CORRENTE
            $("#<%=ddl_Mese.ClientID%>").prop('selectedIndex', month - 1);

            // CAMBIO MESE
            $("#<%=ddl_Mese.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //CAMBIO ANNO
            $("#<%=ddl_Anno.ClientID%>").change(function () {
                changeMonthYear($("#<%=ddl_Mese.ClientID%>").val(), $("#<%=ddl_Anno.ClientID%>").val());
            });

            //SETTO CALENDARI
            $("#<%=tbDataDa.ClientID%>").val(firstDay + "/" + month + "/" + year);
            $("#<%=tbDataA.ClientID%>").val(lastDay + "/" + month + "/" + year);
            $("#<%=ddl_Mese.ClientID%>").val(month);
            $("#<%=ddl_Anno.ClientID%>").val(year);

            function changeMonthYear(newMonth, newYear) {
                var lastDay = new Date(newYear, newMonth, 0).getDate();
                $("#<%=tbDataDa.ClientID%>").val(firstDay + "/" + newMonth + "/" + newYear);
                $("#<%=tbDataA.ClientID%>").val(lastDay + "/" + newMonth + "/" + newYear);

            }

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
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

                function changeMonthYear(newMonth, newYear) {
                    var lastDay = new Date(newYear, newMonth, 0).getDate();
                    $("#<%=tbDataDa.ClientID%>").val(firstDay + "/" + newMonth + "/" + newYear);
                    $("#<%=tbDataA.ClientID%>").val(lastDay + "/" + newMonth + "/" + newYear);
                }
            });
        });
    </script>

    <asp:Label ID="lblTLTime" runat="server" Text="TLTIME" ForeColor="SteelBlue"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">

                <div class="w3-quarter" >
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


                <div class="w3-quarter" style="position:relative;">
                    <label>Data Da</label>
                    <asp:TextBox ID="tbDataDa" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data A</label>
                    <asp:TextBox ID="tbDataA" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 70%;">
                                <asp:Button ID="btnRicercaTLTime" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaTLTime_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 30%;">
                                &nbsp;
                                <%--<asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
 
            </div>
            <br />
            
            <div style="text-align: center;">
                <asp:Button ID="btnCreaFileTLTime" runat="server" Text="Export File TLTime" class="w3-panel w3-green w3-border w3-round" OnClick="btnCreaFileTLTime_Click" Visible="false" />
            </div>

            <div class="round">
                <asp:GridView ID="gv_TLTime" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_TLTime_RowDataBound" AllowPaging="false" OnPageIndexChanging="gv_TLTime_PageIndexChanging" PageSize="20"  AllowSorting="false">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaTLTime" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
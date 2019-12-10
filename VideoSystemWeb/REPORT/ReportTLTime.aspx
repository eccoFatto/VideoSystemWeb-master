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

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.calendar').datetimepicker({
                    locale: 'it',
                    format: 'DD/MM/YYYY'
                });
            });
        });
    </script>

    <asp:Label ID="lblTLTime" runat="server" Text="TLTIME" ForeColor="SteelBlue"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
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
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                </div>
            </div>
            <br />
            
            <div style="text-align: center;">
                <asp:Button ID="btnCreaFileTLTime" runat="server" Text="Export File TLTime" class="w3-panel w3-green w3-border w3-round" OnClick="btnCreaFileTLTime_Click" Visible="false" />
            </div>

            <div class="round">
                <asp:GridView ID="gv_TLTime" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_TLTime_RowDataBound" AllowPaging="false" OnPageIndexChanging="gv_TLTime_PageIndexChanging" PageSize="20"  AllowSorting="false">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last"/>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaTLTime" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="collaboratoriPerGiornata.ascx.cs" Inherits="VideoSystemWeb.REPORT.userControl.collaboratoriPerGiornata" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
                    $("#<%=btnRicercaCollaboratori.ClientID%>").click();
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

    <asp:Label ID="lblCollaboratori" runat="server" Text="COLLABORATORI PER GIORNATA" ForeColor="Tomato"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Ricerca</label>
                    <asp:TextBox ID="tbDataRicerca" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 70%;">
                                <asp:Button ID="btnRicercaCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaCollaboratori_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 30%;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="w3-quarter">
                    &nbsp;
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                </div>
            </div>
            <br />
            
            <div style="text-align: center;">
<%--                <asp:Button ID="btnCreaFileTLTime" runat="server" Text="Export File TLTime" class="w3-panel w3-green w3-border w3-round" OnClick="btnCreaFileTLTime_Click" Visible="false" />--%>
            </div>

            <div class="round">
                <asp:GridView ID="gv_Collaboratori" runat="server" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid" OnRowDataBound="gv_Collaboratori_RowDataBound" AllowPaging="false" OnPageIndexChanging="gv_Collaboratori_PageIndexChanging" PageSize="20"  AllowSorting="false" >
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="Inizio" LastPageText="Fine"/>
                </asp:GridView>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnRicercaCollaboratori" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="popup" TagName="Appuntamenti" Src="~/Agenda/userControl/Appuntamenti.ascx" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            $('.calendarPresentazione').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY'
            });

            $('.calendarPresentazione').on('dp.change', function (e) {
                var data = e.date.date() + "/" + (e.date.month()+1) + "/" + e.date.year();
                $("#<%=hf_valoreData.ClientID%>").val(data);
                 $("#<%=btnsearch.ClientID%>").click();
            });
            
            registraPassaggioMouse();
        });

        function aggiornaAgenda() {
            $("#<%=btnsearch.ClientID%>").click();
        }

        function registraPassaggioMouse() {
            $("[id*=gv_scheduler] tr").mouseover(function () {
                $(this).addClass("highlight");
            });
            $("[id*=gv_scheduler] tr").mouseout(function () {
                $(this).removeClass("highlight");
            });

            var rows = $("[id*=gv_scheduler] tr");

            rows.children().hover(function () {
                rows.children().removeClass('highlight');
                var index = $(this).prevAll().length;
                rows.find(':nth-child(' + (index + 1) + ')').addClass('highlight');
            });

            rows.children().mouseout(function () {
                rows.children().removeClass('highlight');
            });
        }

        function mostracella(row, column) {
            $("#<%=hf_data.ClientID%>").val(row);
            $("#<%=hf_risorsa.ClientID%>").val(column);
            $("#<%=btnEditEvent.ClientID%>").click();       
        }

        function chiudiPopup() {
            $("#<%=btn_chiudi.ClientID%>").click();
        }    
    </script>

    <link rel='stylesheet' href='/Css/Agenda.css' />

    <asp:HiddenField ID="hf_valoreData" runat="server" />
    <asp:HiddenField ID="hf_data" runat="server" />
    <asp:HiddenField ID="hf_risorsa" runat="server" />

    <div class="alert alert-success alert-dismissible fade in" role="alert" id="panelSuccesso" runat="server" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <label id="lbl_Successo" class="form-control-sm">Operazione eseguita correttamente</label>
    </div>

    <table style="width: 98%">
        <tr>
            <td style="width: 80%; vertical-align: top;">
                <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" Style="display: none" />
                <asp:UpdatePanel ID="UpdatePanelCal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="round">
                            <asp:GridView ID="gv_scheduler" runat="server" OnRowDataBound="gv_scheduler_RowDataBound" Style="font-size: 10pt; width: 100%; position: relative; background-color: #EEF1F7;" CssClass="grid"></asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width: 20%; vertical-align: top;">
                <div class="calendarPresentazione"></div>
            </td>
        </tr>
    </table>


    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" OnClick="btnEditEvent_Click" />
    
    <asp:UpdatePanel ID="upEvento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
                <asp:Panel runat="server" ID="pnlContainer" style="display:none">
                    
                    <div class="modalBackground"></div>
                    <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopup round" ScrollBars="Auto" style="font-size:13px;">

                        <popup:Appuntamenti id="popupAppuntamenti" runat="server"></popup:Appuntamenti>

                        <p style="text-align: center;">
                            <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class= "w3-green w3-border w3-round" OnClick="btn_chiudi_Click" />
                        </p>
                    </asp:Panel>
                </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditEvent" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

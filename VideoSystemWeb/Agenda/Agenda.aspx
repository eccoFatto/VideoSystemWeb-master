<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
             $input = $(".calendarPresentazione");
             $input.datepicker({
                 format: "dd/mm/yyyy",
                 todayBtn: true,
                 language: "it",
                 autoclose: true,
                 todayHighlight: true
             });
             $input.data('datepicker').hide = function () { };
             $input.datepicker('show');
             $input.on('changeDate', function (e) {
                 $("#<%=hf_valoreData.ClientID%>").val(e.format());
                 $("#<%=btnsearch.ClientID%>").click();
             });

             $(".calendar").datepicker({
                format: "dd/mm/yyyy",
                todayBtn: true,
                language: "it",
                autoclose: true,
                todayHighlight: true
            });

             registraPassaggioMouse();
        });

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
            alert("row:" + row + " column:" + column);
            $("#<%=btnEditEvent.ClientID%>").click();
        }
    </script>

    <style>
        .highlight { background-color: #DDEEFF; }

        .grid td { border-top:dotted 1px #5377A9; }
        .grid td { border-right:dotted 1px #5377A9; }
        .grid th { border-bottom:solid 2px #5377A9 !important; }  
        .grid td.first { border-right:solid 2px #5377A9 !important; }
    </style>

    <table style="width:100%">
        <tr>
            <td style="width:70%;vertical-align:top;">
                <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
                <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" style="display:none" />
                
                <asp:UpdatePanel ID="UpdatePanelCal" runat="server">
                    <ContentTemplate>      
                        <div class="round">
                            <asp:GridView ID="gv_scheduler" runat="server" OnRowDataBound="gv_scheduler_RowDataBound" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid"></asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnsearch" EventName="Click" /> 
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td style="width:30%;vertical-align:top; padding-left:50px;">
                <div class="calendarPresentazione"></div>
                <asp:HiddenField ID="hf_valoreData" runat="server" />
            </td>
        </tr>
    </table>

    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender runat="server" ID="modalPopupEditEvent" TargetControlID="btnEditEvent"
        PopupControlID="PopupEditEvent" BackgroundCssClass="modalBackground" DropShadow="False" OkControlID="OkButton">
    </ajaxToolkit:ModalPopupExtender>
    
    <asp:Panel ID="PopupEditEvent" runat="server" CssClass="containerPopup round" Style="display: none; border: solid 3px #5377A9;background-color:#EEF1F7;height:50%;width:80%;">
        <asp:UpdatePanel ID="upEvento" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlContainer">
                    <center>
                        NUOVO EVENTO
                    </center>
                    <br />
                    <table>
                        <tr>
                            <td style="width:40%">
                                Data Inizio
                                <input type="text" class="roundSmall calendar" id="txtDataInizio" placeholder="DD/MM/YYYY" />
                            </td>
                            <td style="width:30%">
                                Durata (giorni)
                                <asp:TextBox ID="txtDurata" class="roundSmall" runat="server" MaxLength="2" style="width:30px;" onkeypress="return onlyNumbers()"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                Risorsa
                                <td>
                                    <asp:DropDownList ID="ddlRisorse" runat="server"></asp:DropDownList>
                                </td>
                            </td>
                        </tr>

                    </table>
                  </asp:Panel>
              </ContentTemplate>
        </asp:UpdatePanel>
        <div >
            <p style="text-align: center;">
                <asp:Button ID="OkButton" runat="server" Text="Chiudi" CssClass="roundSmall"/>
            </p>
        </div>
    </asp:Panel>
</asp:Content>

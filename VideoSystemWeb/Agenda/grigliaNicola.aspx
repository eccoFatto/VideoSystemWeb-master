<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grigliaNicola.aspx.cs" Inherits="VideoSystemWeb.grigliaNicola" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <link rel="stylesheet" href="./Scripts/fullcalendar/fullcalendar.css" />
    <link rel='stylesheet' href='./Css/bootstrap-datepicker3.min.css' />

    <script src="../Scripts/jquery-3.3.1.min.js"></script>
    <script src='../Scripts/moment.min.js'></script>
    <script src="../Scripts/tether.js" type="text/javascript"></script>
    <script src='../Scripts/bootstrap.min.js'></script>
    <script src='../Scripts/bootstrap-datepicker.min.js'></script>
    <script src='../Scripts/bootstrap-datepicker.it.min.js'></script>
    <script src='../Scripts/fullcalendar/fullcalendar.js'></script>
    <script src='../Scripts/fullcalendar/locale-all.js'></script>
    
    <script>
        $(document).ready(function () {
            $input = $(".calendarPresentazione");
            $input.datepicker({
                    format: "dd/mm/yyyy",
                    todayBtn: true,
                    language: "it",
                    autoclose: true,
                    todayHighlight: true
            })
            $input.data('datepicker').hide = function () {};
            $input.datepicker('show');
            $input.on('changeDate', function (e) {
                $("#<%=hf_valoreData.ClientID%>").val(e.format());
                $("#<%=btnsearch.ClientID%>").click();
            });
        });
    </script>
    <style>
        .round
        {
         border: 1px solid black;
          -webkit-border-radius: 8px;
          -moz-border-radius: 8px;
          border-radius: 8px;
          overflow: hidden;
          border-color:#AEAEAE;
        }

    </style>
    <table style="width:100%">
        <tr>
            <td style="width:70%;vertical-align:top;">
                <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
                <asp:Button ID="btnsearch" runat="server" Text="SEARCH" OnClick="btnsearch_Click" style="display:none"/>
                <asp:UpdatePanel ID="UpdatePanelCal" runat="server">
                    <ContentTemplate>      
                        <div class="round">
                            <asp:GridView ID="gv_scheduler" runat="server" OnRowDataBound="gv_scheduler_RowDataBound" style="font-size:10pt; width:100%;position:relative"></asp:GridView>
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
</asp:Content>

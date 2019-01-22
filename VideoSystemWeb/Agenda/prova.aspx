<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prova.aspx.cs" Inherits="VideoSystemWeb.Agenda.prova" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
</head>
<body>

    <link rel='stylesheet' href='../Css/bootstrap-datepicker3.min.css' />

    <script src="../Scripts/jquery-3.3.1.min.js"></script>
    <script src='../Scripts/moment.min.js'></script>
    <script src="../Scripts/tether.js" type="text/javascript"></script>
    <script src='../Scripts/bootstrap.min.js'></script>
    <script src='../Scripts/bootstrap-datepicker.min.js'></script>
    <script src='../Scripts/bootstrap-datepicker.it.min.js'></script>
-    <script>
        $(document).ready(function () {
            $input = $(".calendarPresentazione");
            $input.datepicker({
                format: "dd/mm/yyyy",
                todayBtn: true,
                language: "it",
                autoclose: true,
                todayHighlight: true
            });
            $input.data('datepicker').hide = function () {};
            $input.datepicker('show');
            $input.on('changeDate', function (e) {
                $("#<%=hf_valoreData.ClientID%>").val(e.format());
                $("#<%=btnsearch.ClientID%>").click();
            });
        });

        function mostracella(row, column) {
            alert("row:" + row + " column:" + column);
            $("#<%=btnEditEvent.ClientID%>").click();
        }

        $('.calendar').datepicker({
                format: "dd/mm/yyyy",
                todayBtn: true,
                language: "it",
                autoclose: true,
                todayHighlight: true
         });

         function onlyNumbers(a) { var b = event || a, c = b.which || b.keyCode; return 44 == c || (45 == c || (46 == c || c >= 48 && c <= 57)) }
    </script>

    <style>
        .round
        {
          -webkit-border-radius: 8px;
          -moz-border-radius: 8px;
          border-radius: 8px;
          overflow: hidden;
         
          border: solid 2px #5377A9;
        }

        .roundSmall
        {
         border: 1px solid #AEAEAE;
          -webkit-border-radius: 4px;
          -moz-border-radius: 4px;
          border-radius: 4px;
          overflow: hidden;
        }

        .modalBackground {
	        background-color:Gray;
	        filter:alpha(opacity=70);
	        opacity:0.7;
        }
    </style>
    <form runat="server">
    <table style="width:100%">
        <tr>
            <td style="width:70%;vertical-align:top;">
                <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />
                <asp:Button ID="btnsearch" runat="server" OnClick="btnsearch_Click" style="display:none"/>
                
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

    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender runat="server" ID="modalPopupEditEvent" TargetControlID="btnEditEvent"
    PopupControlID="PopupEditEvent" BackgroundCssClass="modalBackground" DropShadow="False" OkControlID="OkButton">
    </ajaxToolkit:ModalPopupExtender>
    
    <asp:Panel ID="PopupEditEvent" runat="server" CssClass="containerPopup round" Style="display: none; border: solid 3px #5377A9;background-color:#fff;height:50%;width:80%;">
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
                                <td><input type="text" id="txtRisorsa" class="roundSmall" style="width:70px;"/></td>
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
    </form>
</body>
</html>

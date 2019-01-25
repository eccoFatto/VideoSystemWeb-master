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
    
    <asp:Panel ID="PopupEditEvent" runat="server" CssClass="containerPopup round" Style="display: none; border: solid 3px #5377A9;background-color:#EEF1F7;width:80%;">
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
                                <asp:Label ID="lbl_DataInizioLavorazione" runat="server" Text="Data inizio lavorazione"></asp:Label>
                                <asp:TextBox ID="txt_DataInizioLavorazione" runat="server" CssClass="roundSmall calendar" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_DataFineLavorazione" runat="server" Text="Data fine lavorazione"></asp:Label>
                                <asp:TextBox class="roundSmall calendar" ID="txt_FineLavorazione" runat="server" placeholder="DD/MM/YYYY"/>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_DurataLavorazione" runat="server" Text="Durata lavorazione"></asp:Label>
                                <asp:TextBox ID="txt_DurataLavorazione" class="roundSmall" runat="server" MaxLength="2" style="width:30px;" onkeypress="return onlyNumbers()"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="width:30%">
                                <asp:Label ID="lbl_Risorsa" runat="server" Text="Risorsa"></asp:Label>      
                                <asp:DropDownList ID="ddl_Risorse" runat="server"></asp:DropDownList>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente"></asp:Label>
                                <asp:DropDownList ID="ddl_cliente" runat="server"></asp:DropDownList>
                            </td>
                            <td style="width:30%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lbl_DurataViaggioAndata" runat="server" Text="Durata viaggio andata"></asp:Label>
                                <asp:TextBox ID="txt_DurataViaggioAndata" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_DurataViaggioRitorno" runat="server" Text="Durata viaggio ritorno"></asp:Label>
                                <input type="text" class="roundSmall" id="txt_DurataViaggioRitorno" placeholder="DD/MM/YYYY" />
                            </td>
                            <td style="width:30%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lbl_DataInizioImpegno" runat="server" Text="Data inizio impegno"></asp:Label>                              
                                <asp:TextBox ID="txt_DataInizioImpegno" runat="server" CssClass="roundSmall calendar"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_DataFineImpegno" runat="server" Text="Data fine impegno"></asp:Label>                       
                                <asp:TextBox ID="txt_DataFineImpegno" runat="server" CssClass="roundSmall calendar"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lbl_ImpegnoOrario" runat="server" Text="Impegno orario"></asp:Label>
                                <asp:CheckBox ID="chk_ImpegnoOrario" runat="server" />
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_ImpegnoOrarioDa" runat="server" Text="Impegno orario da"></asp:Label>                               
                                <asp:TextBox ID="txt_ImpegnoOrarioDa" runat="server" CssClass="roundSmall calendar"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_ImpegnoOrarioA" runat="server" Text="Impegno orario a"></asp:Label>
                                <asp:TextBox ID="txt_ImpegnoOrarioA" runat="server" CssClass="roundSmall calendar"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lbl_Produzione" runat="server" Text="Produzione"></asp:Label>
                                <asp:TextBox ID="txt_Produzione" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_lavorazione" runat="server" Text="Lavorazione"></asp:Label>
                                <asp:TextBox ID="txt_Lavorazione" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lbl_indirizzo" runat="server" Text="Indirizzo"></asp:Label>
                                <asp:TextBox ID="txt_Indirizzo" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_luogo" runat="server" Text="Luogo"></asp:Label>
                                <asp:TextBox ID="txt_Luogo" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                            <td style="width:30%">
                                <asp:Label ID="lbl_CodiceLavoro" runat="server" Text="Codice lavoro"></asp:Label>
                                <asp:TextBox ID="txt_CodiceLavoro" runat="server" CssClass="roundSmall"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lbl_nota" runat="server" Text="Nota"></asp:Label>
                                <asp:TextBox ID="tb_Nota" style="width:90%" Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox>
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

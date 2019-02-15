<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agenda.aspx.cs" Inherits="VideoSystemWeb.Agenda.Agenda" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="popup" TagName="Appuntamento" Src="~/Agenda/userControl/Appuntamento.ascx" %>
<%@ Register TagPrefix="popup" TagName="Offerta" Src="~/Agenda/userControl/Offerta.ascx" %>
<%@ Register TagPrefix="popup" TagName="Lavorazione" Src="~/Agenda/userControl/Lavorazione.ascx" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        $(document).ready(function () {
            $('.calendarAgenda').datetimepicker({
                inline: true,
                locale: 'it',
                format: 'DD/MM/YYYY',
                showTodayButton: true
            });

            $('.calendarAgenda').on('dp.change', function (e) {
                var data = e.date.date() + "/" + (e.date.month() + 1) + "/" + e.date.year();
                $("#<%=hf_valoreData.ClientID%>").val(data);
                $("#<%=btnsearch.ClientID%>").click();
                
            });

            registraPassaggioMouse();

            //FUNZIONI DA ESEGUIRE DOPO IL POSTBACK PARZIALE
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                //mostraUltimaTabSelezionata();

                $(".filtroColonna").each(function () {
                    nascondiColonna(this, this.value);
                });

                $("#<%=mostraAgenda.ClientID%>").mouseover(function() {
                    $("#<%=pnlContainer.ClientID%>").hide();
                });

                $("#<%=mostraAgenda.ClientID%>").mouseout(function() {
                    $("#<%=pnlContainer.ClientID%>").show();
                });
            });
        });

        function nascondiColonna(element, sottotipo) {
            $('#<%=gv_scheduler.ClientID%> tr').each(function () {
                if ($(element).is(':checked')) {
                    $(this).find('.' + sottotipo).show();
                } else {
                    $(this).find('.' + sottotipo).hide();
                }
            });
        }

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


        function openTabEvento(evt, tipoName) {
            
            //document.getElementById('<%=hf_tabSelezionata.ClientID %>').value = tipoName;
            var i, x, tablinks;
            x = document.getElementsByClassName("tabEvento");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablink");
            for (i = 0; i < x.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
            }
            document.getElementById(tipoName).style.display = "block";
            evt.currentTarget.className += " w3-red";
        }

        <%--function mostraUltimaTabSelezionata() {
            var tabSelezionata = document.getElementById('<%= hf_tabSelezionata.ClientID%>').value;
            alert(tabSelezionata);
            document.getElementById(tabSelezionata).style.display = "block";
        }--%>

    </script>

    <link rel='stylesheet' href='/Css/Agenda.css' />

    <asp:HiddenField ID="hf_valoreData" runat="server" />
    <asp:HiddenField ID="hf_data" runat="server" />
    <asp:HiddenField ID="hf_risorsa" runat="server" />
    <asp:HiddenField ID="hf_tabSelezionata" runat="server" />

    <div class="alert alert-success alert-dismissible fade in" role="alert" id="panelSuccesso" runat="server" style="display: none">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <label id="lbl_Successo" class="form-control-sm">Operazione eseguita correttamente</label>
    </div>


    <table style="width: 99%; height: 100%;">
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
                <div class="calendarAgenda" style="margin-left: 20px;"></div>
                <br />
                <div class="w3-container" style="margin-left: 20px; text-align: center">
                    <b>LEGENDA</b>
                    <div style="width: 100%; text-align: left" runat="server" id="divLegenda">
                    </div>
                </div>
                <br />
                <div class="w3-container" style="margin-left: 20px; text-align: center">
                    <b>FILTRO COLONNE AGENDA</b>
                    <div style="width: 100%; text-align: left" runat="server" id="divFiltroAgenda">
                    </div>
                </div>
            </td>
        </tr>
    </table>


    <asp:Button runat="server" ID="btnEditEvent" Style="display: none" OnClick="btnEditEvent_Click" />

    <asp:UpdatePanel ID="upEvento" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <div runat="server" id="pnlContainer" style="display: none">

                <div class="modalBackground"></div>
                <asp:Panel runat="server" ID="innerContainer" CssClass="containerPopup round" ScrollBars="Auto" Style="font-size: 13px;">

                    <div class="alert alert-danger alert-dismissible fade in out" role="alert" runat="server" id="panelErrore" style="display: none">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                        <asp:Label ID="lbl_MessaggioErrore" runat="server" CssClass="form-control-sm"></asp:Label>
                    </div>

                    <div class="w3-container">
                        <div class="w3-bar w3-blue w3-round">
                            <div class="w3-bar-item w3-button tablink w3-red" onclick="openTabEvento(event, 'Appuntamento')">Appuntamento</div>
                            <div class="w3-bar-item w3-button tablink" onclick="openTabEvento(event, 'Offerta')">Offerta</div>
                            <div class="w3-bar-item w3-button tablink" onclick="openTabEvento(event, 'Lavorazione')">Lavorazione</div>
                            <div >
                                <asp:Image ID="mostraAgenda" runat="server" ImageUrl="~/Images/agenda.png" style="position:absolute;right:25px; top:5px;"/>
                            </div> 
                        </div>

                        <div id="Appuntamento" class="w3-container w3-border tabEvento w3-padding-small">
                            <popup:Appuntamento ID="popupAppuntamento" runat="server"></popup:Appuntamento>
                        </div>

                        <div id="Offerta" class="w3-container w3-border tabEvento w3-padding-small" style="display: none">
                            <popup:Offerta ID="popupOfferta" runat="server"></popup:Offerta>
                        </div>

                        <div id="Lavorazione" class="w3-container w3-border tabEvento w3-padding-small" style="display: none">
                            <popup:Lavorazione ID="popupLavorazione" runat="server"></popup:Lavorazione>
                        </div>
                    </div>

                    <div style="position: absolute; width: 100%; bottom: 10px; text-align: center;">
                        <asp:Button ID="btnSalva" runat="server" Text="Salva" class=" w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnSalva_Click" />
                        <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btn_chiudi_Click" />

                        <asp:Button ID="btnElimina" runat="server" Text="Elimina" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="btnElimina_Click" OnClientClick="return confermaEliminazione();" />
                        <asp:Button ID="btnOfferta" runat="server" Text="Trasforma in offerta" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnOfferta_Click" OnClientClick="return confermaCambioStato()" Visible="false" />
                        <asp:Button ID="btnLavorazione" runat="server" Text="Trasforma in lavorazione" class="w3-btn w3-white w3-border w3-border-purple w3-round-large" OnClientClick="return confermaCambioStato()" OnClick="btnLavorazione_Click" Visible="false" />
                        <asp:Button ID="btnRiposo" runat="server" Text="Riposo" class="w3-btn w3-white w3-border w3-border-orange w3-round-large" OnClick="btnRiposo_Click" Visible="false" />
                        
                    </div>

                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditEvent" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

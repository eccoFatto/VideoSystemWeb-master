<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attrezzature.aspx.cs" Inherits="VideoSystemWeb.MAGAZZINO.Attrezzature" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $('.loader').hide();

            $(window).keydown(function (e) {
                if (e.keyCode == 13) {
<%--                    if ($("#<%=hf_tipoOperazione.ClientID%>").val() != 'MODIFICA' && $("#<%=hf_tipoOperazione.ClientID%>").val() != 'INSERIMENTO') {
                        $("#<%=btnRicercaProtocollo.ClientID%>").click();
                    }
                    else {
                        $("#<%=btnRicercaLavorazioni.ClientID%>").click();
                    }--%>
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
    
    <label><asp:Label ID="lblProtocolli" runat="server" Text="ATTREZZATURE" ForeColor="SteelBlue"></asp:Label></label>
    <asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
        <ContentTemplate>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Codice Video System</label>
                    <asp:TextBox ID="tbCodiceVS" runat="server" MaxLength="10" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Categoria</label>
                    <asp:DropDownList ID="ddlTipoCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Sub Categoria</label>
                    <asp:DropDownList ID="ddlTipoSubCategoria" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Descrizione</label>
                    <asp:TextBox ID="tbDescrizione" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Seriale</label>
                    <asp:TextBox ID="tbSeriale" runat="server" MaxLength="20" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>Data Acquisto</label>
                    <asp:TextBox ID="tbDataAcquisto" runat="server" MaxLength="10" class="w3-input w3-border calendar" placeholder="GG/MM/AAAA"></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Posizione Magazzino</label>
                    <asp:DropDownList ID="ddlTipoPosizioneMagazzino" runat="server" AutoPostBack="True" Width="100%" class="w3-input w3-border">
                    </asp:DropDownList>
                </div>
                <div class="w3-quarter">
                    <label>Marca</label>
                    <asp:TextBox ID="tbMarca" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="w3-row-padding">
                <div class="w3-quarter">
                    <label>Modello</label>
                    <asp:TextBox ID="tbModello" runat="server" MaxLength="100" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Note</label>
                    <asp:TextBox ID="tbNote" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
                </div>
                <div class="w3-quarter">
                    <label>Garanzia</label><br />
                    <asp:CheckBox ID="cbGaranzia" runat="server" CssClass="w3-check"></asp:CheckBox>
                </div>
                <div class="w3-quarter">
                    <label>Disponibile</label><br />
                    <asp:CheckBox ID="cbDisponibile" runat="server" CssClass="w3-check"></asp:CheckBox>
                </div>
            </div>
            <div class="w3-row-padding w3-margin-bottom">
                <div class="w3-half">
                    <label>&nbsp;</label>
                    &nbsp;
                </div>
                <div class="w3-quarter" style="position:relative;">
                    <label>&nbsp;</label>
                    &nbsp;
                </div>
                <div class="w3-quarter">
                    <label>&nbsp;</label>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Button ID="btnRicercaAttrezzatura" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaAttrezzatura_Click" OnClientClick="$('.loader').show();" Text="Ricerca" />
                            </td>
                            <td style="width: 40%;">
                                <div id="divBtnInserisciAttrezzatura" runat="server">
                                    <div id="clbtnInserisciAttrezzatura" class="w3-btn w3-white w3-border w3-border-red w3-round-large" onclick="inserisciAttrezzatura();" >Inserisci</div>
                                </div>
                            </td>
                            <td style="width: 20%;">
                                <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;" OnClientClick="azzeraCampiRicerca();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
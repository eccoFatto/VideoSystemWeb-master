<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagClientiFornitori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagClientiFornitori" %>
<h1><asp:Label ID="lblTipoAzienda" runat="server" Text=""></asp:Label></h1>
<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        
        <div class="w3-row-padding">
            <div class="w3-quarter">
                <label>Ragione Sociale</label>
                <asp:TextBox ID="tbRagioneSociale" runat="server" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Partita Iva</label>
                <asp:TextBox ID="TbPiva" runat="server" MaxLength="11" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Codice Fiscale</label>
                <asp:TextBox ID="tbCF" runat="server" MaxLength="16" class="w3-input w3-border" placeholder=""></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Tipo</label>
                <asp:DropDownList ID="ddlTipoAzienda" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlTipoAzienda_SelectedIndexChanged" class="w3-input w3-border">
                </asp:DropDownList>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                <label>Referente</label>
                <asp:TextBox ID="TbReferente" runat="server" class="w3-input w3-border" placeholder="" ></asp:TextBox>
            </div>
            <div class="w3-quarter">&nbsp;</div>
            <div class="w3-quarter">&nbsp;</div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaAziende" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="btnRicercaAziende_Click" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnInserisciAzienda" runat="server" class="w3-btn w3-white w3-border w3-border-red w3-round-large" Text="Inserisci" OnClientClick="inserisciFornitore();" OnClick="btnInserisciAzienda_Click" />
                        </td>
                        <td style="width:20%;">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;"  OnClientClick="azzeraCampiRicerca();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>   

        <div class="round">
            <asp:GridView ID="gv_aziende" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_aziende_RowDataBound">
            </asp:GridView>
        </div>
        
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRicercaAziende" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="BtnPulisciCampiRicerca" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlTipoAzienda" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

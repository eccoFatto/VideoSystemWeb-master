<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagCollaboratori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%--<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />    --%>


<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <table style="width:600px;align-content:center;">
            <tr>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Cognome</div></td>
                <td style="width:100px;">
                    <div >
                        <asp:TextBox ID="tbCognome" runat="server" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Nome</div></td>
                <td style="width:100px;">
                    <asp:TextBox ID="tbNome" runat="server" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">CF</div></td>
                <td style="width:100px;">
                    <asp:TextBox ID="tbCF" runat="server" MaxLength="16" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Citta</div></td>
                <td style="width:100px;">
                    <asp:TextBox ID="tbCitta" runat="server" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Qualifica</div></td>
                <td style="width:100px;">
                    <asp:DropDownList ID="ddlQualifiche" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlQualifiche_SelectedIndexChanged" class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>OPERATORE</asp:ListItem>
                        <asp:ListItem>FONICO</asp:ListItem>
                        <asp:ListItem>REGISTA</asp:ListItem>
                        <asp:ListItem>ALTRO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Societa</div></td>
                <td style="width:100px;">
                    <asp:TextBox ID="TbSocieta" runat="server" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">P.Iva</div></td>
                <td style="width:100px;">
                        <asp:TextBox ID="TbPiva" runat="server" MaxLength="11" class="w3-panel w3-white w3-border w3-hover-orange w3-round"></asp:TextBox>
                </td>
                <td colspan="2" style="width:150px;">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:50%;" class="w3-panel w3-green w3-border w3-round">                    
                                <asp:LinkButton ID="lbRicercaCollaboratori" runat="server" OnClick="lbRicercaCollaboratori_Click">Ricerca</asp:LinkButton>
                            </td>
                            <td style="width:50%;" class="w3-panel w3-red w3-border w3-round">
                                <asp:LinkButton ID="lbPulisciCampiRicerca" runat="server" OnClick="lbPulisciCampiRicerca_Click">X</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="round">
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid">
            </asp:GridView>
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbRicercaCollaboratori" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlQualifiche" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>
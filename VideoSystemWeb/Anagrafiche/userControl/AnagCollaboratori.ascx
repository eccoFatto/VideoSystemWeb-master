﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagCollaboratori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" />    
    <style>
        .highlight {
            background-color: #DDEEFF;
        }

        .grid td {
            border-top: dotted 1px #5377A9;
        }

        .grid td {
            border-right: dotted 1px #5377A9;
        }

        .grid th {
            border-bottom: solid 2px #5377A9 !important;
        }

        .grid td.first {
            border-right: solid 2px #5377A9 !important;
        }
    </style>

<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        <table style="width:600px;align-content:center;">
            <tr>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Cognome</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="tbCognome" runat="server" ></asp:TextBox>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Nome</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="tbNome" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">CF</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="tbCF" runat="server" MaxLength="16"></asp:TextBox>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Citta</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="tbCitta" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Qualifica</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:DropDownList ID="ddlQualifiche" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlQualifiche_SelectedIndexChanged">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem>OPERATORE</asp:ListItem>
                            <asp:ListItem>FONICO</asp:ListItem>
                            <asp:ListItem>REGISTA</asp:ListItem>
                            <asp:ListItem>ALTRO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">Societa</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="TbSocieta" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td style="width:50px;"><div class="w3-panel w3-yellow w3-border w3-round">P.Iva</div></td>
                <td style="width:100px;">
                    <div class="w3-panel w3-white w3-border w3-hover-orange w3-round">
                        <asp:TextBox ID="TbPiva" runat="server" MaxLength="11"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2" style="width:150px;">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:50%;">                    
                                <div class="w3-panel w3-green w3-border w3-round">
                                    <asp:LinkButton ID="lbRicercaCollaboratori" runat="server" OnClick="lbRicercaCollaboratori_Click">Ricerca</asp:LinkButton>
                                </div>
                            </td>
                            <td style="width:50%;">
                                <div class="w3-panel w3-red w3-border w3-round">
                                    <asp:LinkButton ID="lbPulisciCampiRicerca" runat="server" OnClick="lbPulisciCampiRicerca_Click">X</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="round">
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid"></asp:GridView>
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbRicercaCollaboratori" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlQualifiche" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>
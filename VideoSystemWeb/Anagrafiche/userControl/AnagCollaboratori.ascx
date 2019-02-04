<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnagCollaboratori.ascx.cs" Inherits="VideoSystemWeb.Anagrafiche.userControl.AnagCollaboratori" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script>
    function mostraCollaboratore(row) {
    //alert("id collaboratore:" + row);
    $("#<%=hf_idColl.ClientID%>").val(row);
    $("#<%=btnEditCollaboratore.ClientID%>").click();
    }

    function openDettaglioAnagrafica(tipoName) {
        var i;
        var x = document.getElementsByClassName("collab");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";  
        }
        document.getElementById(tipoName).style.display = "block";  
    }
</script>


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
                            <td style="width:40%;" class="w3-panel w3-green w3-border w3-round w3-center">                    
                                <asp:LinkButton ID="lbRicercaCollaboratori" runat="server" OnClick="lbRicercaCollaboratori_Click" >Ricerca</asp:LinkButton>
                            </td>
                            <td style="width:40%;" class="w3-panel w3-grey w3-border w3-round w3-center">                    
                                <asp:LinkButton ID="lbInserisciCollaboratori" runat="server" OnClick="lbInserisciCollaboratori_Click" >Inserisci</asp:LinkButton>
                            </td>
                            <td style="width:20%;" class="w3-panel w3-red w3-border w3-round w3-center">
                                <asp:LinkButton ID="lbPulisciCampiRicerca" runat="server" OnClick="lbPulisciCampiRicerca_Click" >&times;</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="round">
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_collaboratori_RowDataBound">
            </asp:GridView>
        </div>


        <asp:Button runat="server" ID="btnEditCollaboratore" Style="display: none" onclick="EditCollaboratore_Click"/>
        <asp:HiddenField ID="hf_idColl" runat="server" />
        <asp:UpdatePanel  ID="upCollaboratore" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" >
            <ContentTemplate>
                <div>
                    <asp:Panel  runat="server" ID="pnlContainer" visible="false">
                        <div class="modalBackground"></div>
                        <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopup round" ScrollBars="Auto">
                            <div class="intestazionePopup" style="width:100%; text-align:center">
                                GESTIONE COLLABORATORI
                            </div>
                            <br />
                            <div class="errorMessage" style="width:100%; text-align:center">
                                <asp:Label ID="lbl_MessaggioErrore" runat="server" Text="Controllare i campi evidenziati" Visible="false" ></asp:Label>
                            </div>
                            
                            <div class="w3-container">
                                <div class="w3-bar w3-red w3-round">
                                    <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Anagrafica')">Anagrafica</div>
                                    <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Qualifiche')">Qualifiche</div>
                                    <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Indirizzi')">Indirizzi</div>
                                    <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Telefoni')">Telefoni</div>
                                    <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Email')">Email</div>
                                </div>
                              <div id="Anagrafica" class="w3-container w3-border collab">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:75%">
                                            <table>
                                                <tr>
                                                    <td colspan="2" class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Cognome" runat="server" Text="Cognome"></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_Cognome" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2" class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Nome" runat="server" Text="Nome" ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_Nome" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true" Width="100%"></asp:TextBox>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_CF" runat="server" Text="CF" ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_CF" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Nazione" runat="server" Text="Cognome"></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_Nazione" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_ComuneNascita" runat="server" Text="Nome" ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_ComuneNascita" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_ProvinciaNascita" runat="server" Text="CF" ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_ProvinciaNascita" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_DataNascita" runat="server" Text="Data Nasc."></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_DataNascita" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true" TextMode="Date"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_ComuneRiferimento" runat="server" Text="Comune Rif." ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_ComuneRiferimento" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_PartitaIva" runat="server" Text="P.Iva" ></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_PartitaIva" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td class=".column" runat="server">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Societa" runat="server" Text="Societa"></asp:Label></div>
                                                        <asp:TextBox ID="tbMod_NomeSocieta" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round fieldMedium" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class=".column" runat="server" style="align-content:center">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Assunto" runat="server" Text="Assunto" ></asp:Label></div>
                                                    </td>
                                                    <td class=".column" runat="server" style="align-content:center;align-items:center">
                                                        <asp:CheckBox ID="cbMod_Assunto" runat="server" Enabled="false"></asp:CheckBox>
                                                    </td>
                                                    <td class=".column" runat="server" style="align-content:center">
                                                        <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Attivo" runat="server" Text="Attivo" ></asp:Label></div>
                                                    </td>
                                                    <td class=".column" runat="server" style="align-content:center;align-items:center">
                                                        <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false"></asp:CheckBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                        <td style="width:25%; vertical-align:top">
                                            <div class="w3-container">
                                              <h2></h2>
                                              <div class="w3-card-4" style="width:90%">
                                                <asp:Image ID="imgCollaboratore" runat="server" Width="100%" />
                                                <div class="w3-container w3-center">
                                                  <p></p>
                                                </div>
                                              </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class=".column" runat="server">
                                            <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Note" runat="server" Text="Note"></asp:Label></div>
                                            <asp:TextBox ID="tbMod_Note" runat="server" CssClass="w3-panel w3-white w3-border w3-hover-orange w3-round" ReadOnly="true" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                              </div>
                                <div id="Qualifiche" class="w3-container collab" style="display:none">
                                    <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Qualifiche" runat="server" Text="Qualifiche"></asp:Label></div>
                                    <asp:ListBox ID="lbMod_Qualifiche" runat="server" CssClass="w3-panel w3-white w3-border w3-round" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                                </div>

                                <div id="Indirizzi" class="w3-container collab" style="display:none">
                                    <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Indirizzi" runat="server" Text="Indirizzi"></asp:Label></div>
                                    <asp:ListBox ID="lbMod_Indirizzi" runat="server" CssClass="w3-panel w3-white w3-border w3-round" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                                </div>

                                <div id="Telefoni" class="w3-container collab" style="display:none">
                                    <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Telefoni" runat="server" Text="Telefoni"></asp:Label></div>
                                    <asp:ListBox ID="lbMod_Telefoni" runat="server" CssClass="w3-panel w3-white w3-border w3-round" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                                </div>
                                <div id="Email" class="w3-container collab" style="display:none">
                                    <div class="w3-panel w3-yellow w3-border w3-round"><asp:Label ID="lbl_Email" runat="server" Text="Email"></asp:Label></div>
                                    <asp:ListBox ID="lbMod_Email" runat="server" CssClass="w3-panel w3-white w3-border w3-round" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                                </div>

                            </div>

                            <div style="text-align: center;">
                                <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnModifica_Click" />
                                <asp:Button ID="btnSalva" runat="server" Text="Salva" class="w3-panel w3-green w3-border w3-round" OnClick="btnSalva_Click" Visible="false"/>
                                <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false"/>
                            </div>
                            <p style="text-align: center;">
                                <asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-panel w3-green w3-border w3-round" OnClick="btn_chiudi_Click" OnClientClick="return confirm('Confermi chiusura pagina?')"/>
                            </p>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnEditCollaboratore" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbRicercaCollaboratori" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlQualifiche" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>
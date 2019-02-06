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

    function openUploadImg(id) {
      var x = document.getElementById(id);
      if (x.className.indexOf("w3-show") == -1) {
        x.className += " w3-show";
      } else { 
        x.className = x.className.replace(" w3-show", "");
      }
}
</script>

<asp:UpdatePanel ID="UpdatePanelRicerca" runat="server">
    <ContentTemplate> 
        
        <div class="w3-row-padding">
            <div class="w3-quarter">
                <label>Cognome</label>
                <asp:TextBox ID="tbCognome" runat="server" class="w3-input w3-border" placeholder="Cognome"></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Nome</label>
                <asp:TextBox ID="tbNome" runat="server" class="w3-input w3-border" placeholder="Nome"></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Codice Fiscale</label>
                <asp:TextBox ID="tbCF" runat="server" MaxLength="16" class="w3-input w3-border" placeholder="Codice Fiscale"></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Città</label>
                <asp:TextBox ID="tbCitta" runat="server" class="w3-input w3-border" placeholder="Città"></asp:TextBox>
            </div>
        </div>
        
          <div class="w3-row-padding w3-margin-bottom">
            <div class="w3-quarter">
                <label>Qualifica</label>
                <asp:DropDownList ID="ddlQualifiche" runat="server" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="ddlQualifiche_SelectedIndexChanged" class="w3-input w3-border" placeholder="Qualifica">
                </asp:DropDownList>
            </div>
            <div class="w3-quarter">
                <label>Società</label>
                <asp:TextBox ID="TbSocieta" runat="server" class="w3-input w3-border" placeholder="Società" ></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label>Partita Iva</label>
                <asp:TextBox ID="TbPiva" runat="server" MaxLength="11" class="w3-input w3-border" placeholder="Partita Iva"></asp:TextBox>
            </div>
            <div class="w3-quarter">
                <label></label>
                <table style="width:100%;">
                    <tr>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnRicercaCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-green w3-round-large" OnClick="lbRicercaCollaboratori_Click" Text="Ricerca" />
                        </td>
                        <td style="width:40%;">                    
                            <asp:Button ID="btnInserisciCollaboratori" runat="server" class="w3-btn w3-white w3-border w3-border-red w3-round-large" OnClick="lbInserisciCollaboratori_Click" Text="Inserisci" />
                        </td>
                        <td style="width:20%;">
                            <asp:Button ID="BtnPulisciCampiRicerca" runat="server" class="w3-btn w3-circle w3-red" Text="&times;"  OnClick="PulisciCampiRicerca_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>   

        <div class="round">
            <asp:GridView ID="gv_collaboratori" runat="server" style="font-size:10pt; width:100%;position:relative;background-color:#EEF1F7;" CssClass="grid" OnRowDataBound="gv_collaboratori_RowDataBound">
            </asp:GridView>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRicercaCollaboratori" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="ddlQualifiche" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>


        <asp:Button runat="server" ID="btnEditCollaboratore" Style="display: none" onclick="EditCollaboratore_Click"/>
        <asp:HiddenField ID="hf_idColl" runat="server" />
        <%--<ajaxToolkit:ToolkitScriptManager runat="server" ID="tksm1" ></ajaxToolkit:ToolkitScriptManager>--%>
         <%--<asp:UpdatePanel  ID="upCollaboratore" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" >--%>
        <asp:UpdatePanel ID="upColl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel  runat="server" ID="pnlContainer" visible="false">
                    <div class="modalBackground"></div>
                    <asp:Panel  runat="server" ID="innerContainer" CssClass="containerPopupStandard round" ScrollBars="Auto">
                        <div class="w3-container w3-center w3-xlarge">
                            GESTIONE COLLABORATORI
                        </div>
                        <br />
                        <div class="errorMessage" style="width: 100%; text-align: center">
                            <asp:Label ID="lbl_MessaggioErrore" runat="server" Visible="false"></asp:Label>
                        </div>
                            
                        <div class="w3-container">
                            <div class="w3-bar w3-red w3-round">
                                <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Anagrafica')">Anagrafica</div>
                                <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Qualifiche')">Qualifiche</div>
                                <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Indirizzi')">Indirizzi</div>
                                <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Telefoni')">Telefoni</div>
                                <div class="w3-bar-item w3-button w3-red" onclick="openDettaglioAnagrafica('Email')">Email</div>
                                <div class="w3-bar-item w3-button w3-red w3-right"><asp:Button ID="btn_chiudi" runat="server" Text="Chiudi" class="w3-button w3-green w3-small w3-round" OnClick="btn_chiudi_Click" OnClientClick="return confirm('Confermi chiusura pagina?')"/></div>
                            </div>
                            <div id="Anagrafica" class="w3-container w3-border collab">
                            <table style="width:100%">
                                <tr>
                                    <td style="width:75%">
                                        <div class="w3-row-padding">
                                            <div class="w3-half">
                                                <label>Cognome</label>
                                                <asp:TextBox ID="tbMod_Cognome" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" ></asp:TextBox>
                                            </div>
                                            <div class="w3-half">
                                                <label>Nome</label>
                                                <asp:TextBox ID="tbMod_Nome" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <div class="w3-row-padding">
                                            <div class="w3-quarter">
                                                <label>Codice Fiscale</label>
                                                <asp:TextBox ID="tbMod_CF" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Nazione</label>
                                                <asp:TextBox ID="tbMod_Nazione" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Comune Nascita</label>
                                                <asp:TextBox ID="tbMod_ComuneNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Provincia Nascita</label>
                                                <asp:TextBox ID="tbMod_ProvinciaNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <div class="w3-row-padding">
                                            <div class="w3-quarter">
                                                <label>Data Nascita</label>
                                                <asp:TextBox ID="tbMod_DataNascita" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" TextMode="Date"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Comune Riferimento</label>
                                                <asp:TextBox ID="tbMod_ComuneRiferimento" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Partita Iva</label>
                                                <asp:TextBox ID="tbMod_PartitaIva" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="w3-quarter">
                                                <label>Società</label>
                                                <asp:TextBox ID="tbMod_NomeSocieta" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        
                                        <div class="w3-row-padding">
                                            <div class="w3-half">
                                                <label>Assunto</label>
                                                <asp:CheckBox ID="cbMod_Assunto" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                                            </div>
                                            <div class="w3-half">
                                                <label>Attivo</label>
                                                <asp:CheckBox ID="cbMod_Attivo" runat="server" Enabled="false" class="w3-check"></asp:CheckBox>
                                            </div>
                                        </div>

                                        <div class="w3-container">
                                            <label>Note</label>
                                            <asp:TextBox ID="tbMod_Note" runat="server" class="w3-input w3-border" placeholder="" ReadOnly="true" Width="99%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </div>

                                    </td>
                                    <td style="width:25%; vertical-align:top">
                                        <div class="w3-container">
                                            <h2></h2>
                                            <div class="w3-card-4" style="width:90%">
                                            <asp:Image ID="imgCollaboratore" runat="server" Width="100%" />
                                            <div class="w3-container w3-center">
                                                <p>
                                                    <div onclick="openUploadImg('divUploadImg')" class="w3-button w3-block w3-center-align">
                                                     Carica Immagine</div>
                                                    <div id="divUploadImg" class="w3-container w3-hide">
                                                        <asp:FileUpload ID="fuImg" runat="server" Font-Size="X-Small" class="w3-button w3-yellow w3-round w3-margin" />
                                                        <asp:Button ID="uploadButton" runat="server" class="w3-button w3-yellow w3-round w3-margin" OnClick="Button1_Click" Text="Carica Immagine" />
                                                        <asp:Label ID="lblImage" runat="server" Font-Size="XX-Small"></asp:Label>
                                                     </div>
                                                </p>
                                            </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <div id="Qualifiche" class="w3-container w3-border collab" style="display:none">
                                <label>Qualifiche</label>
                                <asp:ListBox ID="lbMod_Qualifiche" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                            </div>

                            <div id="Indirizzi" class="w3-container w3-border collab" style="display:none">
                                <label>Indirizzi</label>
                                <asp:ListBox ID="lbMod_Indirizzi" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                            </div>

                            <div id="Telefoni" class="w3-container w3-border collab" style="display:none">
                                <label>Telefoni</label>
                                <asp:ListBox ID="lbMod_Telefoni" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                            </div>
                            <div id="Email" class="w3-container  w3-border collab" style="display:none">
                                <label>E-Mail</label>
                                <asp:ListBox ID="lbMod_Email" runat="server" class="w3-input w3-border w3-margin" ReadOnly="true" Width="99%" Rows="3"></asp:ListBox>
                            </div>

                        </div>

                        <div style="text-align: center;">
                            <asp:Button ID="btnModifica" runat="server" Text="Modifica" class="w3-panel w3-green w3-border w3-round" OnClick="btnModifica_Click" />
                            <asp:Button ID="btnSalva" runat="server" Text="Salva" class="w3-panel w3-green w3-border w3-round" OnClick="btnSalva_Click" Visible="false"/>
                            <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" class="w3-panel w3-green w3-border w3-round" OnClick="btnAnnulla_Click" Visible="false"/>
                        </div>
                        <p style="text-align: center;">
                            
                        </p>
                    </asp:Panel>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="uploadButton" />
                <asp:AsyncPostBackTrigger ControlID="btnEditCollaboratore" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btn_chiudi" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>


